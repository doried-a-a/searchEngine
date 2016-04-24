using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.highlight
{
    public class BoldColorHighlighter:IHighlightMethod
    {
        private String htmlColor;

        public BoldColorHighlighter(String htmlColor)
        {
            this.htmlColor = htmlColor;
        }

        public String highlight(String text)
        {
            return " <span style=\"color:" + this.htmlColor + "\"><b>" + text + "</b></span> ";
        }
        
    }
}