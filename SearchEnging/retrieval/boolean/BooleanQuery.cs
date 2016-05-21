using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.retrieval.general;

namespace SearchEnging.retrieval
{
    public class BooleanQuery
    {
        /// <summary>
        /// this OrExpression should consists of and expressions only, and those and expressions consists of either terms,
        /// or not's , these notss should have terms only
        /// </summary>
        private OrExpression dnfQuery;
        private int currentAnd = 0;

        public BooleanQuery(OrExpression dnfExpression)
        {
            // checking if valid dnf
            if (!dnfExpression.getIsDnf())
                throw new Exception("Given OrExpression is not in DNF format");
            this.dnfQuery = dnfExpression;
        }

        public OrExpression getDnf()
        {
            return this.dnfQuery;
        }

        public AndExpression getNextAnd()
        {
            if (currentAnd >= dnfQuery.getExpressions().Count)
                return null;

            var and = dnfQuery.getExpressions()[currentAnd];
            currentAnd++;
            return (AndExpression) and;

        }

    }
        
  
}