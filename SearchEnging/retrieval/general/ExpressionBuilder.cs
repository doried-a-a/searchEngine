using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.retrieval.parser;

namespace SearchEnging.retrieval.general
{
    public class ExpressionBuilder
    {
   
        /// <summary>
        /// This builds the expression
        /// </summary>
        /// <returns></returns>
        public ExpressionHolder buildExpression(List<IToken> infix_tokens)
        {
            Parser parser = new Parser();

            List<IToken> postfix_Tokens = parser.convertInfixToPostfix(infix_tokens);

            Stack<IBooleanExpression> stack = new Stack<IBooleanExpression>();
            
            Dictionary<String, Term> terms = new Dictionary<string, Term>();

            foreach (var token in postfix_Tokens)
            {
                if (token is Word)
                {
                    Word word = token as Word;

                    if (terms.ContainsKey(word.getWord()) == false)
                        terms[word.getWord()] = new Term(word.getWord(), false);
                    Term term = terms[word.getWord()];
                    stack.Push(term);
                    continue;
                }

                Operation operation = (Operation)token;
                char symbol = operation.getSymbol();

                if (symbol == Parser.AND || symbol == Parser.OR)
                {
                    try
                    {
                        IBooleanExpression left = stack.Pop();
                        IBooleanExpression right = stack.Pop();
                        IBooleanExpression result = null;
                        if (symbol == Parser.AND)
                            result = new AndExpression(left, right);
                        else
                            result = new OrExpression(left, right);

                        stack.Push(result);
                        continue;
                    }
                    catch
                    {
                        throw new Exception("Parsing error.");
                    }
                }

                else if (symbol == Parser.NOT)
                {
                    try
                    {
                        IBooleanExpression right = stack.Pop();
                        IBooleanExpression result = new Not(right);
                        stack.Push(result);
                        continue;
                    }
                    catch
                    {
                        throw new Exception("Parsing error.");
                    }
                }
                else
                {
                    throw new Exception("Unexpected operation was found [ " + symbol + " ]");
                }
            }

            if (stack.Count != 1)
            {
                throw new Exception("Parsing error : stack does not has one and only one element left, it has " + stack.Count + " element(s)");
            }

            IBooleanExpression exp = stack.Pop();

            return new ExpressionHolder(exp, terms);
        }


        public static ExpressionHolder convertToDNF(ExpressionHolder expHolder)
        {
            OrExpression dnf = new OrExpression();

            var exp = expHolder.getExpression();
            var terms = expHolder.getTerms().Values;

            for (int i = 0; i < Math.Pow(2, terms.Count); i++)
            {
                for (int j = 0; j < terms.Count; j++)
                {
                    terms.ElementAt(j).setValue((i & (1 << j)) != 0);
                }

                if (exp.evaluate())
                {
                    AndExpression and = new AndExpression();
                    for (int j = 0; j < terms.Count; j++)
                    {
                        if ((i & (1 << j)) != 0)
                            and.addExpression(terms.ElementAt(j));
                        else
                            and.addExpression(new Not(terms.ElementAt(j)));
                    }
                    dnf.addExpression(and);
                }
            }
            return new ExpressionHolder(dnf, expHolder.getTerms());
        }
    }
}