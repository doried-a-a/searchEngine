using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.utils.nlp;

namespace SearchEnging.utils.highlight
{
    public class WordHighligter
    {
        private IHighlightMethod method;
        public WordHighligter(IHighlightMethod method)
        {
            this.method = method;
        }

        public String highlistString(String text, String queryText)
        {
            List<String> queryWords = TextProcessor.processText(queryText).Split(' ').ToList();

            StringTokenizer tokenizer = new StringTokenizer(text);

            String ret = "";
            bool prevTokenWasWhiteSpace = false;

            int numToKeep = 10;
            int addMore = 0;

            List<String> prevWords = new List<string>();

            tokenizer.IgnoreWhiteSpace = false;

            while(true){
                
                Token token = tokenizer.Next();
                if (token.Kind == TokenKind.EOF)
                    break;

                if (queryWords.Contains(TextProcessor.processText(token.Value)))
                {

                    if (prevWords.Count > 0)
                    {
                        ret += "....";
                        foreach (String word in prevWords)
                            ret += word + " ";
                    }
                    prevWords = new List<string>();

                    ret += method.highlight(token.Value);
                    addMore = numToKeep;
                }
                else
                {
                    if (token.Kind == TokenKind.EOL || token.Kind == TokenKind.WhiteSpace)
                        continue;

                    //{
                    //    prevTokenWasWhiteSpace = true;
                    //    continue;
                    //}
                    //else
                    //{
                    //    prevTokenWasWhiteSpace = false;
                    //}

                    if(addMore==0)
                        prevWords.Add(token.Value);

                    if (prevWords.Count > numToKeep)
                        prevWords.RemoveAt(0);

                    if (addMore > 0)
                    {
                        ret += " " + token.Value;
                        if (addMore == 1)
                            ret += ".....<br>";
                        addMore--;
                    }

                }
            }
            return ret;
        }
    }
}