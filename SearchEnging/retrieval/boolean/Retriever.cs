using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.Adaptors;
using SearchEnging.retrieval.general;

namespace SearchEnging.retrieval.boolean
{
    public class Retriever
    {
        public List<DocumentWrapper> getDocumentsContainingWords(List<String> wordsToContain,List<String> wordsToExclude)
        {
            List<DocumentWrapper> list = new List<DocumentWrapper>();
            if (wordsToContain.Count == 0 && wordsToExclude.Count==0)
                return list;

            IndexManager manager = IndexManager.getInstance();

            if (wordsToContain.Count > 0)
            {
                var containingFirstWord = manager.getDocumentsContainingWord(wordsToContain[0]);
                foreach (var doc in containingFirstWord)
                    list.Add(doc);
            }
            else
            {
                var all = manager.getAllDocuments();
                foreach(var doc in all)
                    list.Add(doc);
            }

            for (int i = 1; i < wordsToContain.Count; i++)
            {
                String word = wordsToContain[i];
                var docsContainingThisWord = manager.getDocumentsContainingWord(word);
                list = list.Intersect(docsContainingThisWord).ToList();
            }

            // Excluding documents that contain any of the unwanted words
            foreach (var word in wordsToExclude)
            {
                var docsContainingThisWord = manager.getDocumentsContainingWord(word);
                list = list.Except(docsContainingThisWord).ToList();
            }

            return list;
        }

        public List<DocumentWrapper> getDocumentsForBooleanQuery(BooleanQuery query)
        {
            if (query.getDnf().getExpressions().Count == 0)
                return new List<DocumentWrapper>();

            List<DocumentWrapper> docs = new List<DocumentWrapper>();

            IndexManager manager = IndexManager.getInstance();

            while (true)
            {
                AndExpression termGorup = query.getNextAnd();

                if (termGorup == null)
                    break;
                List<String> wordsToInclude = new List<String>();
                List<String> wordsToExclude = new List<String>();
                
                foreach (var exp in termGorup.getExpressions())
                {
                    // each expression in the AND term is either a final term or a not of a final term
                    // if it is a final term:
                    if (exp is Term)
                        wordsToInclude.Add(((Term)exp).getWord());
                    // else, it is a not of a final term
                    else
                        wordsToExclude.Add(((Term)(((Not)exp).getExpression())).getWord());
                }

                var docsSatisfyingThisAndGorup = this.getDocumentsContainingWords(wordsToInclude,wordsToExclude);

                docs = docs.Union(docsSatisfyingThisAndGorup).ToList();
            }

            return docs;
        }
    }
}