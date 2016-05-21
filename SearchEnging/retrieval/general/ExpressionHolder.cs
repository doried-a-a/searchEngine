using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.general
{
    public class ExpressionHolder
    {
        private IBooleanExpression expression;
        private Dictionary<String, Term> terms;

        public ExpressionHolder(IBooleanExpression expression, Dictionary<String, Term> terms)
        {
            this.expression = expression;
            this.terms = terms;
        }

        public IBooleanExpression getExpression()
        {
            return this.expression;
        }

        public Dictionary<String, Term> getTerms()
        {
            return this.terms;
        }
    }
}