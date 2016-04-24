using System;
using System;
using System.IO;
using System.Text;

namespace SearchEnging.utils.nlp
{
    /********************************************************8
 *	Author: Andrew Deren
 *	Date: July, 2004
 *	http://www.adersoftware.com
 * 
 *	StringTokenizer class. You can use this class in any way you want
 * as long as this header remains in this file.
 * 
 **********************************************************/
    /// <summary>
    /// StringTokenizer tokenized string (or stream) into tokens.
    /// </summary>
    public class StringTokenizer
    {
        const char EOF = (char)0;

        int line;
        int column;
        int pos;	// position within data

        string data;

        bool ignoreWhiteSpace;
        char[] symbolChars;

        int saveLine;
        int saveCol;
        int savePos;

        public StringTokenizer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            data = reader.ReadToEnd();

            Reset();
        }

        public StringTokenizer(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;

            Reset();
        }

        /// <summary>
        /// gets or sets which characters are part of TokenKind.Symbol
        /// </summary>
        public char[] SymbolChars
        {
            get { return this.symbolChars; }
            set { this.symbolChars = value; }
        }

        /// <summary>
        /// if set to true, white space characters will be ignored,
        /// but EOL and whitespace inside of string will still be tokenized
        /// </summary>
        public bool IgnoreWhiteSpace
        {
            get { return this.ignoreWhiteSpace; }
            set { this.ignoreWhiteSpace = value; }
        }

        private void Reset()
        {
            this.ignoreWhiteSpace = false;
            this.symbolChars = new char[] { '=', '+', '-', '/', ',', '.', '\'', '\"', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', ']', ':', ';', '<', '>', '?', '|', '\\' };

            line = 1;
            column = 1;
            pos = 0;
        }

        protected char LA(int count)
        {
            if (pos + count >= data.Length)
                return EOF;
            else
                return data[pos + count];
        }

        protected char Consume()
        {
            char ret = data[pos];
            pos++;
            column++;

            return ret;
        }

        protected Token CreateToken(TokenKind kind, string value)
        {
            return new Token(kind, value, line, column);
        }

        protected Token CreateToken(TokenKind kind)
        {
            string tokenData = data.Substring(savePos, pos - savePos);
            return new Token(kind, tokenData, saveLine, saveCol);
        }

        public Token Next()
        {
        ReadToken:

            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenKind.EOF, string.Empty);

                case ' ':
                case '\t':
                    {
                        if (this.ignoreWhiteSpace)
                        {
                            Consume();
                            goto ReadToken;
                        }
                        else
                            return ReadWhitespace();
                    }
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber();

                case '\r':
                    {
                        StartRead();
                        Consume();
                        if (LA(0) == '\n')
                            Consume();	// on DOS/Windows we have \r\n for new line

                        line++;
                        column = 1;

                        return CreateToken(TokenKind.EOL);
                    }
                case '\n':
                    {
                        StartRead();
                        Consume();
                        line++;
                        column = 1;

                        return CreateToken(TokenKind.EOL);
                    }

                case '"':
                    {
                        return ReadString();
                    }

                default:
                    {
                        if (Char.IsLetter(ch) || ch == '_')
                            return ReadWord();
                        else if (IsSymbol(ch))
                        {
                            StartRead();
                            Consume();
                            return CreateToken(TokenKind.Symbol);
                        }
                        else
                        {
                            StartRead();
                            Consume();
                            return CreateToken(TokenKind.Unknown);
                        }
                    }

            }
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            saveLine = line;
            saveCol = column;
            savePos = pos;
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns></returns>
        protected Token ReadWhitespace()
        {
            StartRead();

            Consume(); // consume the looked-ahead whitespace char

            while (true)
            {
                char ch = LA(0);
                if (ch == '\t' || ch == ' ')
                    Consume();
                else
                    break;
            }

            return CreateToken(TokenKind.WhiteSpace);

        }

        /// <summary>
        /// reads number. Number is: DIGIT+ ("." DIGIT*)?
        /// </summary>
        /// <returns></returns>
        protected Token ReadNumber()
        {
            StartRead();

            bool hadDot = false;

            Consume(); // read first digit

            while (true)
            {
                char ch = LA(0);
                if (Char.IsDigit(ch))
                    Consume();
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    Consume();
                }
                else
                    break;
            }

            return CreateToken(TokenKind.Number);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        protected Token ReadWord()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetter(ch) || ch == '_')
                    Consume();
                else
                    break;
            }

            return CreateToken(TokenKind.Word);
        }

        /// <summary>
        /// reads all characters until next " is found.
        /// If "" (2 quotes) are found, then they are consumed as
        /// part of the string
        /// </summary>
        /// <returns></returns>
        protected Token ReadString()
        {
            StartRead();

            Consume(); // read "

            while (true)
            {
                char ch = LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')	// handle CR in strings
                {
                    Consume();
                    if (LA(0) == '\n')	// for DOS & windows
                        Consume();

                    line++;
                    column = 1;
                }
                else if (ch == '\n')	// new line in quoted string
                {
                    Consume();

                    line++;
                    column = 1;
                }
                else if (ch == '"')
                {
                    Consume();
                    if (LA(0) != '"')
                        break;	// done reading, and this quotes does not have escape character
                    else
                        Consume(); // consume second ", because first was just an escape
                }
                else
                    Consume();
            }

            return CreateToken(TokenKind.QuotedString);
        }

        /// <summary>
        /// checks whether c is a symbol character.
        /// </summary>
        protected bool IsSymbol(char c)
        {
            for (int i = 0; i < symbolChars.Length; i++)
                if (symbolChars[i] == c)
                    return true;

            return false;
        }
    }


    /// <summary>
    /// ********************************************************
    ///   *	Author: Andrew Deren
    ///   *	Date: July, 2004
    ///   *	http://www.adersoftware.com
    ///   * 
    ///   *	StringTokenizer class. You can use this class in any way you want
    ///   * as long as this header remains in this file.
    ///   * 
    ///   **********************************************************
    /// </summary>
    /// <remarks>
    /// modified by Jes1111 to be specific to CSS
    /// </remarks>
    public enum TokenKind
    {
        /// <summary>
        ///   The unknown.
        /// </summary>
        Unknown,

        /// <summary>
        ///   The word.
        /// </summary>
        Word,

        /// <summary>
        ///   The number.
        /// </summary>
        Number,

        /// <summary>
        ///   The quoted string.
        /// </summary>
        QuotedString,

        /// <summary>
        ///   The white space.
        /// </summary>
        WhiteSpace,

        /// <summary>
        ///   The symbol.
        /// </summary>
        Symbol,

        /// <summary>
        ///   The eol.
        /// </summary>
        EOL,

        /// <summary>
        ///   The eof.
        /// </summary>
        EOF,

        /// <summary>
        ///   The comment.
        /// </summary>
        Comment,

        /// <summary>
        ///   The selector.
        /// </summary>
        Selector,

        /// <summary>
        ///   The block.
        /// </summary>
        Block,

        /// <summary>
        ///   The at rule.
        /// </summary>
        AtRule,

        /// <summary>
        ///   The import rule.
        /// </summary>
        ImportRule,

        /// <summary>
        ///   The error.
        /// </summary>
        Error
    }

    /// <summary>
    /// The token.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Token
    {
        #region Constants and Fields

        /// <summary>
        ///   The column.
        /// </summary>
        private readonly int column;

        /// <summary>
        ///   The kind.
        /// </summary>
        private readonly TokenKind kind;

        /// <summary>
        ///   The line.
        /// </summary>
        private readonly int line;

        /// <summary>
        ///   The value.
        /// </summary>
        private readonly string value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="kind">
        /// The kind.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="line">
        /// The line.
        /// </param>
        /// <param name="column">
        /// The column.
        /// </param>
        /// <returns>
        /// A void value...
        /// </returns>
        public Token(TokenKind kind, string value, int line, int column)
        {
            this.kind = kind;
            this.value = value;
            this.line = line;
            this.column = column;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the column.
        /// </summary>
        /// <value>The column.</value>
        /// <remarks>
        /// </remarks>
        public int Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        ///   Gets the kind.
        /// </summary>
        /// <value>The kind.</value>
        /// <remarks>
        /// </remarks>
        public TokenKind Kind
        {
            get
            {
                return this.kind;
            }
        }

        /// <summary>
        ///   Gets the line.
        /// </summary>
        /// <value>The line.</value>
        /// <remarks>
        /// </remarks>
        public int Line
        {
            get
            {
                return this.line;
            }
        }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        /// <value>The value.</value>
        /// <remarks>
        /// </remarks>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        #endregion
    }

}

