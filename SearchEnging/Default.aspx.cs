using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SearchEnging.utils;
using SearchEnging.utils.Collection;
using SearchEnging.Adaptors;
using SearchEnging.utils.highlight;

namespace SearchEnging
{
    public partial class _Default : System.Web.UI.Page
    {
        IndexManager mgr = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            mgr = IndexManager.getInstance();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            

            var query = txtSearch.Text;
            var processedQuery = utils.nlp.TextProcessor.processText(query);

            List<DocumentWrapper> docs = mgr.getDocumentsContainingWord(processedQuery);
            lblResultsCount.Text = String.Format("Found {0} result(s).", docs.Count);

           
            WordHighligter highlighter = new WordHighligter(new BoldColorHighlighter("Red"));
            
            foreach (var doc in docs)
            {
                String highlightedText = highlighter.highlistString(doc.getText(), query);
                

                lblResults.Text += "" + new BoldHilighter().highlight(doc.getTitle() + " [" + doc.getTextDocument().getPath() + "]") +
                    "<br>"
                    + highlightedText +
                    "<hr><br>";
            }
        }
    }
}

