using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.general
{
    public interface IBooleanExpression
    {
        bool evaluate();
        String toString();
    }
}