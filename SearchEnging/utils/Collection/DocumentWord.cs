using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.Collection
{
    public class DocumentWord
    {
        private String wordValue;
        private int wordFreq;

        public DocumentWord(String word)
        {
            this.wordValue = word;
            this.wordFreq = 0;
        }

        public DocumentWord(String word,int freq)
        {
            this.wordValue = word;
            this.wordFreq = freq;
        }

        public String getWord()
        {
            return this.wordValue;
        }

        public int getWordFreq(){
            return this.wordFreq;
        }

    }
}