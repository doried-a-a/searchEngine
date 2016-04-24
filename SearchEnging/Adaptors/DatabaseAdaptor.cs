using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.Model;

namespace SearchEnging.Adaptors
{
    public abstract class DatabaseAdaptor
    {
        public static SearchEngineModelContainer container = new SearchEngineModelContainer();

        public static Document addDocument(String uri,String dateIndexed,String title)
        {
            try
            {
                Document doc = new Document()
                {
                    DateIndexed = dateIndexed,
                    Title = title,
                    Uri = uri
                };

                container.AddToDocumentSet(doc);
                container.SaveChanges();
                return doc;
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static bool clearAllDocuments()
        {
            try
            {
                container.ExecuteStoreCommand("delete from DocumentSet");
                //while (container.DocumentSet.Count() > 0)
                //{
                //    container.DocumentSet.DeleteObject(container.DocumentSet.First());
                //}
                //container.Refresh(System.Data.Objects.RefreshMode.StoreWins, container.DocumentSet);
                container = new SearchEngineModelContainer();
                return true;
            }
            catch(Exception ee)
            {
                throw ee;
                //return false;
            }
        }



        public static Word findWord(String value)
        {
            var res = from word in container.WordSet
                      where  word!=null && word.Value.Equals(value)
                      select word;
            return res.FirstOrDefault();
        }


        public static Word addOrGetWord(String value)
        {
            try
            {
                Word word = findWord(value);
                if (word == null)
                {
                    word = new Word()
                    {
                        Value = value
                    };
                }
                return word;
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static bool clearAllWords()
        {
            try
            {
                container.ExecuteStoreCommand("delete from WordSet");
                //container.Refresh(System.Data.Objects.RefreshMode.StoreWins, container.WordSet);
                container = new SearchEngineModelContainer();
                return true;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static DocumentWordRelationship getDocumentWordRelationship(Document doc, Word word)
        {
            try
            {
                var res = from rel in container.DocumentWordRelationshipSet
                          where rel.Word != null && rel.WordValue == word.Value && rel.Document != null &&
                              rel.DocumentId == doc.DocumentId
                          select rel;
                return res.FirstOrDefault();
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static DocumentWordRelationship addWordToDocument(Document doc, Word word,bool saveChanges=true)
        {
            try
            {
                DocumentWordRelationship rel = getDocumentWordRelationship(doc, word);
                if (rel == null)
                {
                    rel = new DocumentWordRelationship()
                    {
                        Document = doc,
                        Word = word,
                        Frequency = 1
                    };
                }
                else
                {
                    rel.Frequency++;
                }
                if(saveChanges)
                    container.SaveChanges();
                return rel;
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }


        public static DocumentWordRelationship setWordFreqInDocument(Document doc, Word word,int freq,bool saveChanges=true)
        {
            try
            {
                DocumentWordRelationship rel = getDocumentWordRelationship(doc, word);
                if (rel == null)
                {
                    rel = new DocumentWordRelationship()
                    {
                        Document = doc,
                        Word = word,
                        Frequency = freq
                    };
                }
                else
                {
                    rel.Frequency = freq;
                }
                if(saveChanges)
                    container.SaveChanges();
                return rel;
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static List<DocumentWordRelationship> getDocumentsContainingWord(String w)
        {
            try
            {
                var res = from rel in container.DocumentWordRelationshipSet
                          where rel.WordValue == w
                          select rel;
                return res.ToList();
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static List<Document> getAllDocuments()
        {
            try
            {
                return container.DocumentSet.ToList();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static Document findDocumentByPath(String path)
        {
            try{
                var res = from doc in container.DocumentSet
                          where doc.Uri.Equals(path)
                          select doc;
                return res.FirstOrDefault();
            }
            catch(Exception ee){
                throw ee;
            }
        }

        public static int getTotalWordOccurences(String word)
        {
            try
            {
                var res = from rel in container.DocumentWordRelationshipSet
                          where rel.WordValue == word
                          select rel.Frequency;
                return res.Sum();
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static int getDocumentFrequency(String word)
        {
            try
            {
                var res = from rel in container.DocumentWordRelationshipSet
                          where rel.WordValue == word
                          select rel;
                return res.Count();
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static int getDocumentsCount()
        {
            return container.DocumentSet.Count();
        }

        public int getWordsCount()
        {
            return container.WordSet.Count();
        }

        public static bool clearAllDocumentWordRelations()
        {
            try
            {
                container.ExecuteStoreCommand("delete from DocumentWordRelationshipSet");
                //container.Refresh(System.Data.Objects.RefreshMode.StoreWins, container.DocumentWordRelationshipSet);
                container = new SearchEngineModelContainer();
                return true;
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        public static bool saveChanges()
        {
            try
            {
                container.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}