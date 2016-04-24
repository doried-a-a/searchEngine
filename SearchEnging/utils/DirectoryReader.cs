using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils
{
    public class DirectoryReader
    {
        public static List<Collection.TextDocument> getDocuments(String path)
        {
            String [] filenames = System.IO.Directory.GetFiles(path);
            
            List<Collection.TextDocument> docs = new List<Collection.TextDocument>();

            foreach (String filename in filenames)
            {
                String filePath = System.IO.Path.Combine(path, filename);
                String fileContent = Inputs.utils.Reader.readFile(filePath);
                docs.Add(new Collection.TextDocument(fileContent, filePath, filename, DateTime.Now.ToShortDateString()));
            }

            return docs;
        }
    }
}