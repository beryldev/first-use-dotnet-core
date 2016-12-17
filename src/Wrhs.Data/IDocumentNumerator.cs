using Wrhs.Common;

namespace Wrhs.Data
{
    public interface IDocumentNumerator
    {
         void SetContext(WrhsContext context);

         Document AssignNumber(Document document);
    }
}