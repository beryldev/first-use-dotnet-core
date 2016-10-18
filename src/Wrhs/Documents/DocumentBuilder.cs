using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;

namespace Wrhs.Documents
{
    public abstract class DocumentBuilder<TDocument, TDocLine> where TDocLine : DocumentLine
    {
        public event EventHandler<IEnumerable<ValidationResult>> OnAddLineFail;

        public event EventHandler<IEnumerable<ValidationResult>> OnUpdateLineFail;

        protected IValidator<DocumentBuilderAddLineCommand> addLineValidator;

        protected List<TDocLine> lines = new List<TDocLine>();

        public IEnumerable<TDocLine> Lines 
        { 
            get 
            { 
                return lines.ToArray(); 
            } 
        }

        public DocumentBuilder(IValidator<DocumentBuilderAddLineCommand> addLineValidator)
        {
            this.addLineValidator = addLineValidator;
        }

        public abstract TDocument Build();

        int lastId = 0;
        protected void AddValidatedLine(DocumentBuilderAddLineCommand command)
        {
            lastId++;
            var line = CommandToDocumentLine(command);
            line.Id = lastId;
            lines.Add(line);
        }

        protected abstract TDocLine CommandToDocumentLine(DocumentBuilderAddLineCommand command);

        protected abstract DocumentBuilderAddLineCommand DocumentLineToAddLineCommand(TDocLine line);

        public virtual void AddLine(DocumentBuilderAddLineCommand command)
        {
            var validationResults = addLineValidator.Validate(command);
            if(validationResults.Count() > 0)
            {
                OnAddLineFail?.Invoke(this, validationResults);
                return;
            }

            AddValidatedLine(command);
        }

        public virtual void RemoveLine(TDocLine line)
        {
            lines.Remove(line);
        }

        public virtual void UpdateLine(TDocLine line)
        {
            var command = DocumentLineToAddLineCommand(line);
            var validationResults = addLineValidator.Validate(command);
            if(validationResults.Count() > 0)
            {
                OnUpdateLineFail?.Invoke(this, validationResults);
                return;
            }

            var lineToUpdate = lines
                .Where(item=>item.Id == line.Id)
                .FirstOrDefault();
            
            var indexOfLine = lines.IndexOf(lineToUpdate);
            lines.RemoveAt(indexOfLine);
            lines.Insert(indexOfLine, line);
        }
    }
    
}