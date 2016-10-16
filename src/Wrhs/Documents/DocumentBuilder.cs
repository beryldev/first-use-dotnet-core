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

        protected IValidator<DocumentBuilderUpdateLineCommand> updateLineValidator;

        protected List<TDocLine> lines = new List<TDocLine>();

        public DocumentBuilder(IValidator<DocumentBuilderAddLineCommand> addLineValidator, 
            IValidator<DocumentBuilderUpdateLineCommand> updateLineValidator)
        {
            this.addLineValidator = addLineValidator;
            this.updateLineValidator = updateLineValidator;
        }

        public abstract TDocument Build();

        protected abstract void AddValidatedLine(DocumentBuilderAddLineCommand command);

        protected abstract DocumentBuilderUpdateLineCommand DocumentLineToUpdateCommand(TDocLine line);

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
            var command = DocumentLineToUpdateCommand(line);
            var validationResults = updateLineValidator.Validate(command);
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