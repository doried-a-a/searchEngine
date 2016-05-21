using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace SearchEnging.retrieval.general
{
    public class OrExpression : IBooleanExpression
    {
        private List<IBooleanExpression> exps;

        public OrExpression()
        {
            exps = new List<IBooleanExpression>();
        }

        public OrExpression(List<IBooleanExpression> exps)
        {
            this.exps = exps;
        }


        public OrExpression(IBooleanExpression exp1, IBooleanExpression exp2)
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
            foreach(var exp in exps){
                if (exp.evaluate())
                    return true;
            }
            return false;
        }
        
        public String toString()
        {
            String str = "";
            foreach (var exp in exps)
                str = str + exp.toString() + " + ";
            if (str.Length > 0)
                str = str.Substring(0,str.Length - 1);
            str = "(" + str + ")";
            return str;
        }

        public List<IBooleanExpression> getExpressions()
        {
            return this.exps;
        }


        public bool getIsDnf()
        {
            foreach (var exp in this.exps)
            {
                if (!(exp is AndExpression))
                    return false;
                AndExpression and = exp as AndExpression;
                foreach (var and_child in and.getExpressions())
                {
                    if (and_child is Term)
                        continue;
                    if (!(and_child is Not))
                        return false;
                    var not = and_child as Not;
                    if (!(not.getExpression() is Term))
                        return false;
                }
            }
            return true;
        }

    }
}