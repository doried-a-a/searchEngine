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
using SearchEnging.retrieval.boolean;
using SearchEnging.retrieval;
using SearchEnging.retrieval.general;
using SearchEnging.retrieval.parser;

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

            String text = txtSearch.Text;

            //Boolean
            //==============================================================================

            // converting query to string of tokens (words + And, Or,Not and () sumbols)
            try
            {
                while (text.Contains(' '))
                    text = text.Replace(" ", "");

                Parser parser = new Parser();
                List<IToken> tokens = parser.convertStringToTokens(text);
                // stemming words
                foreach (IToken token in tokens)
                    if (token is Word)
                    {
                        Word word = token as Word;
                        word.setWord(utils.nlp.TextProcessor.stemWord(word.getWord()));
                    }
                // this converts the tokens to a boolean expression IBooleanExpression, 
                ExpressionBuilder builder = new ExpressionBuilder();
                //the following will return the IBooleanExpression encapsulated by a holder which also contains the terms
                var expHolder = builder.buildExpression(tokens);
                //converting the resulting IBooleanExpression to DNF (Disjoint Normal Form)
                expHolder = ExpressionBuilder.convertToDNF(expHolder);

                var exp = expHolder.getExpression() as OrExpression;
                var terms = expHolder.getTerms().Values; // this terms are stemmed

                // retriever
                var retriever = new retrieval.boolean.Retriever();
                BooleanQuery bQuery = new BooleanQuery(exp);

                List<DocumentWrapper> docs = retriever.getDocumentsForBooleanQuery(bQuery);

                lblResultsCount.Text = String.Format("Found {0} result(s).", docs.Count);

                WordHighligter highlighter = new WordHighligter(new BoldColorHighlighter("Red"));

                foreach (var doc in docs)
                {
                    String highlightedText = highlighter.highlistString(doc.getText(), text);

                    lblResults.Text += "" + new BoldHilighter().highlight(doc.getTitle() + " [" + doc.getTextDocument().getPath() + "]") +
                        "<br>"
                        + highlightedText +
                        "<hr><br>";
                }
            }
            catch (Exception ee)
            {
                lblResults.Text = "";
                lblResultsCount.Text = "Error while parsing query. Enter the query correctly please.";
            }
        }
    }
}

