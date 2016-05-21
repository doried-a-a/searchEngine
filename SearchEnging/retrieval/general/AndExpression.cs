using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace SearchEnging.retrieval.general
{
    public class AndExpression : IBooleanExpression
    {
        private List<IBooleanExpression> exps;

        public AndExpression()
        {
            exps = new List<IBooleanExpression>();
        }

        public AndExpression(List<IBooleanExpression> exps)
        {
            this.exps = exps;
        }

        public AndExpression(IBooleanExpression exp1, IBooleanExpression exp2)
        {
            this.exps = new List<IBooleanExpression>();
            exps.Add(exp1);
            exps.Add(exp2);
        }

        public void addExpression(IBooleanExpression exp)
        {
            this.exps.Add(exp);
        }

        public bool evaluate()
        {
            foreach (var exp in exps)
            {
                if (exp.evaluate()==false)
                    return false;
            }
            return true;
        }

        public String toString()
        {
            String str = "";
            foreach (var exp in exps)
                str = str + exp.toString() + "";
            str = "(" + str + ")";
            return str;
        }

        public List<IBooleanExpression> getExpressions()
        {
            return this.exps;
        }


    }
}