using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.parser
{
    public class Word:IToken
    {
        private String word;
        public Word(String word)
        {
            this.word = word;
        }

        public String getWord()
        {
            return word;
        }

        public void setWord(String word)
        {
            this.word = word;
        }

        public String getText()
        {
            return word;
        }
    }
}