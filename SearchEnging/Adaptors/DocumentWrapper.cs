using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.Model;
using SearchEnging.utils.Collection;

namespace SearchEnging.Adaptors
{
    public class DocumentWrapper
    {
        private Document document;
        private TextDocument textDocument;

        public DocumentWrapper(Document doc, TextDocument textDoc)
        {
            this.document = doc;
            this.textDocument = textDoc;
        }

        public Document getDocument()
        {
            return this.document;
        }

        public TextDocument getTextDocument()
        {
            return this.textDocument;
        }

        public String getTitle()
        {
            return textDocument.getTitle();
        }

        public String getDate()
        {
            return textDocument.getDate();
        }

        public String getText()
        {
            return textDocument.getText();
        }
    }
}