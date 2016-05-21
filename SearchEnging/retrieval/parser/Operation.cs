using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEnging.retrieval.parser
{
    public class Operation:IToken
    {
        private char symbol;
        public Operation(char symbol)
        {
            this.symbol = symbol;
        }

        public char getSymbol()
        {
            return this.symbol;
        }

        public String getText()
        {
            return symbol + "";
        }
    }
}