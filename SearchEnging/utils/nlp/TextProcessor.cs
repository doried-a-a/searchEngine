using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.nlp
{
    public class TextProcessor
    {
        private static RemoveStopWordsClass stopwordsDetector = new RemoveStopWordsClass();
        private static ISRI isri = new ISRI();

        public static String processText(String str)
        {
            String processed = "";
            StringTokenizer tok = new StringTokenizer(str);

            while (true)
            {
                Token token = tok.Next();
                if (token.Kind == TokenKind.EOF)
                    break;

                TokenKind[] allowed = {TokenKind.Word  , TokenKind.Number};
                if( !allowed.Contains(token.Kind))
                    continue;

                if (stopwordsDetector.isStopWord(token.Value))
                    continue;

                processed += isri.Stemming(token.Value) + " ";
            }
            processed = processed.Trim();

           

            

            return processed;

        }
            
    }
}