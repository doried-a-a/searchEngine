using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace SearchEnging.utils.Collection
{
    public class TextDocument
    {
        private String text;
        private String title;
        private String path;
        private String date;

        List<DocumentWord> list = new List<DocumentWord>();

        private void computeWords()
        {
            list = new List<DocumentWord>();

            String pprocessedText = nlp.TextProcessor.processText(text);
            String[] terms = pprocessedText.Split(' ');
            String[] words = terms.Distinct().ToArray();

            foreach (String word in words)
            {
                int count = terms.Where(e => e.Equals(word)).Count();
                list.Add(new DocumentWord(word, count));
            }
        }

        public TextDocument(String text)
        {
            this.text = text;
            computeWords();
        }

        public TextDocument(String text,String path)
        {
            this.text = text;
            this.path = path;
            computeWords();
        }

        public TextDocument(String text,String path,String title,String date)
        {
            this.text = text;
            this.path = path;
            this.title = title;
            this.date = date;
            computeWords();
        }

        public String getText(){
            return this.text;
        }

        public String getTitle()
        {
            return this.title;
        }

        public String getDate()
        {
            return this.date;
        }

        public String getPath()
        {
            return this.path;
        }

        public List<DocumentWord> getDocumentWords()
        {
            return list; 
        }
    }
}