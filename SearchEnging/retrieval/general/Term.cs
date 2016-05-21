using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.general
{
    public class Term:IBooleanExpression
    {
        private String word;
        private bool value;

        public Term(String word,bool value)
        {
            this.word = word;
            this.value = value;
        }

        public bool evaluate()
        {
            return this.value;
        }

        public void setValue(bool newValue)
        {
            this.value = newValue;
        }

        public void setWord(String word)
        {
            this.word = word;
        }

        public String getWord()
        {
            return this.word;
        }


        public String toString()
        {
            return word;// value ? "1" : "0";
        }
    }
}