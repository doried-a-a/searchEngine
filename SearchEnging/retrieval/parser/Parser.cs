using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SearchEnging.retrieval.general;

namespace SearchEnging.retrieval.parser
{
    public class Parser
    {
        public static char OR = '+';
        public static char AND = '&';
        public static char NOT = '-';
        public static char OPEN = '(';
        public static char CLOSE = ')';

        Dictionary<char, int> priority;

        public Parser()
        {
            priority = new Dictionary<char, int>();
            priority.Add(OR,0);
            priority.Add(AND, 1);
            priority.Add(NOT,2);
        }


        public List<IToken> convertStringToTokens(String str)
        {
            List<IToken> tokens = new List<IToken>();
            int idx = 0;
            while (idx < str.Length)
            {
                char ch = str[idx];
                if (ch == OR || ch == AND || ch == NOT || ch==OPEN || ch==CLOSE)
                {
                    tokens.Add(new Operation(ch));
                    idx++;
                }
                else
                {
                    String word = "";
                    while (idx<str.Length)
                    {
                        ch = str[idx];
                        if (ch == OR || ch == AND || ch == NOT || ch == OPEN || ch == CLOSE)
                            break;
                        word = word + ch;
                        idx++;
                    }
                    tokens.Add(new Word(word));

                }
            }
            return tokens;
        }

        public List<IToken> convertInfixToPostfix(List<IToken> original)
        {
            Stack<char> q = new Stack<char>();
            List<IToken> result = new List<IToken>();

            foreach (IToken token in original)
            {
                if (token is Word)
                {
                    result.Add(token);
                    continue;
                }

                char op = ((Operation)token).getSymbol();

                if (q.Count == 0 || q.Peek() == OPEN || op == OPEN)
                {
                    q.Push(op);
                    continue;
                }

                char head;
                if (op==CLOSE){
                    while(true){
                        if (q.Count == 0)
                            throw new Exception("Parseing error");
                        head = q.Pop();
                        if (head == OPEN)
                            break;
                        result.Add(new Operation(head));
                    }
                    continue;
                }
                head = q.Peek();
                if (priority[head] <= priority[op])
                {
                    
                    q.Push(op);
                    continue;
                }
                else
                {
                    while (q.Count > 0 && q.Peek() != OPEN && priority[q.Peek()]>priority[op])
                    {
                        result.Add(new Operation(q.Pop()));
                    }
                    q.Push(op);
                    continue;
                }
            }
            while (q.Count > 0)
                result.Add(new Operation(q.Pop()));

            return result;
        }
    }
}