using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.Inputs.utils
{
    public class Reader
    {
        public static String readFile(String filePath)
        {
            String fileType = filePath.Substring(filePath.LastIndexOf("."));
            if (fileType.Equals(".docx"))
            {
                DocxToText converter = new DocxToText(filePath);
                String text = converter.ExtractText();
                return text;
            }
            else throw new Exception("Unsupported file type " + fileType);
        }
    }
}