using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.highlight
{
    public class ColorHighlighter : IHighlightMethod
    {
        private String htmlColor;
        public ColorHighlighter(String htmlColor)
        {
            this.htmlColor = htmlColor;
        }

        public String highlight(String text)
        {
            return String.Format(" <span style=\"color:{0}\">{1}</span> ", htmlColor, text);
        }
    }
}