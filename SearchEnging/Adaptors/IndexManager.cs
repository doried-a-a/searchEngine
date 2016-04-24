using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.Model;
using SearchEnging.utils.Collection;
using SearchEnging.utils;

namespace SearchEnging.Adaptors
{
    public class IndexManager
    {
        List<DocumentWrapper> docs;
        private static IndexManager instance;

        public static IndexManager getInstance()
        {
            if (instance == null)
                instance = new IndexManager();
            return instance;
        }

        private IndexManager()
        {

            //to force reindexing, uncomment this line

            //this.clearAll();


            String datapath = "D:\\Data\\original";
            docs = new List<DocumentWrapper>();

            List<TextDocument> txtdocs = DirectoryReader.getDocuments(datapath);
            List<Document> dbDocs = DatabaseAdaptor.getAllDocuments();

            bool isAllIndexed = true;

            //for each text document, search in the db for a document with the same path. if all documents are in the
            // database, then no need to reindex.
            foreach (TextDocument txtDoc in txtdocs)
            {
                // finding a document in the db with the same path means it is indexed
                Document doc = DatabaseAdaptor.findDocumentByPath(txtDoc.getPath());
                if (doc == null)
                {
                    isAllIndexed = false;
                    break;
                }
                else
                    docs.Add(new DocumentWrapper(doc, txtDoc));
            }

            //there still a single case, if the database contains documents more than what we want to index, then reindex.
            if (isAllIndexed==false || dbDocs.Count > txtdocs.Count)
            {
                this.clearAll();
                this.docs = new List<DocumentWrapper>();

                foreach (TextDocument txtDoc in txtdocs)
                {
                    DocumentWrapper wrapper = this.addDocument(txtDoc.getText(), txtDoc.getTitle(), txtDoc.getPath(), txtDoc.getDate());
                    this.indexDocument(wrapper);
                }
            }
            // in the following case, all documents are already indexed, so just load them
        }

        public DocumentWrapper getDocumentWrapper(Document doc)
        {
            var res = from d in docs
                      where d.getDocument().DocumentId == doc.DocumentId
                      select d;
            return res.FirstOrDefault();
        }

        public void clearAll()
        {
            DatabaseAdaptor.clearAllDocumentWordRelations();
            DatabaseAdaptor.clearAllWords();
            DatabaseAdaptor.clearAllDocuments();
            int docCnt = DatabaseAdaptor.container.DocumentSet.Count();
            int wordCnt = DatabaseAdaptor.container.WordSet.Count();
            int relCnt = DatabaseAdaptor.container.DocumentWordRelationshipSet.Count();
        }

        public DocumentWrapper addDocument(String text,String title, String path, String date)
        {
            Document doc = DatabaseAdaptor.addDocument(path, date, title);
            DocumentWrapper wrapper = new DocumentWrapper(doc, new TextDocument(text, path, title, date));
            docs.Add(wrapper);
            return wrapper;
        }

        public void indexDocument(DocumentWrapper doc)
        {
            List<DocumentWord> docWords = doc.getTextDocument().getDocumentWords();
            int all = docWords.Count;
            int sofar = 0;
            foreach (DocumentWord word in docWords)
            {
                if (doc.getDocument() == null)
                {
                    var df = 1;
                }
                Word w = DatabaseAdaptor.addOrGetWord(word.getWord());
                DatabaseAdaptor.setWordFreqInDocument(doc.getDocument(),w,word.getWordFreq(),true);
                sofar++;
            }
            DatabaseAdaptor.saveChanges();
        }

        public int getTotalWordOccurences(String w)
        {
            return DatabaseAdaptor.getTotalWordOccurences(w);
        }

        public int getDocumentFryequency(String w)
        {
            return DatabaseAdaptor.getDocumentFrequency(w);
        }

        public List<DocumentWrapper> getDocumentsContainingWord(String word)
        {
            List<DocumentWordRelationship> list = DatabaseAdaptor.getDocumentsContainingWord(word);
            List<DocumentWrapper> ret = new List<DocumentWrapper>();
            foreach (DocumentWordRelationship rel in list)
                ret.Add(getDocumentWrapper(rel.Document));
            return ret;
        }
    }
}