using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.highlight
{
    public class BoldHilighter : IHighlightMethod
    {
        public String highlight(String text)
        {
            return "<b>" + text + "</b>";
        }
    }
}