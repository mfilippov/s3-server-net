using System.IO;
using System.Xml.Linq;
using Nancy;

namespace Api.Infrastructure
{
    public class XDocumentResponse : Response
    {
        public XDocumentResponse(XDocument document)
        {
            ContentType = "text/xml";
            Contents = document.Save;

        }
    }
}