using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.general
{
    public class Not:IBooleanExpression
    {
        IBooleanExpression exp;

        public Not(IBooleanExpression exp)
        {
            this.exp = exp;
        }

        public bool evaluate()
        {
            return !this.exp.evaluate();
        }

        public String toString()
        {
            return "-" + exp.toString();
        }

        public IBooleanExpression getExpression()
        {
            return this.exp;
        }

    }
}