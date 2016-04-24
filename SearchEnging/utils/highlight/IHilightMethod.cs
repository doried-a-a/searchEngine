using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.utils.highlight
{
    public interface IHighlightMethod
    {
        String highlight(String text);
    }
}