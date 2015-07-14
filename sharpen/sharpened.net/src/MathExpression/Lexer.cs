/*
 * Converted from Java
 */

using System.IO;
using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class Lexer
	{
		/// <exception cref="System.IO.IOException"></exception>
		public static AList<Lexeme> Tokenize(string s)
		{
			StreamTokenizer tokenizer = new StreamTokenizer(new StringReader(s));
			tokenizer.OrdinaryChar('-');
			// Don't parse minus as part of numbers.
			AList<Lexeme> tokBuf = new AList<Lexeme>();
			while (tokenizer.NextToken() != StreamTokenizer.TT_EOF)
			{
				switch (tokenizer.ttype)
				{
					case StreamTokenizer.TT_NUMBER:
					{
						tokBuf.AddItem(new Lexeme(Lexeme.NUMBER, tokenizer.sval.ToString()));
						break;
					}

					case StreamTokenizer.TT_WORD:
					{
						tokBuf.AddItem(new Lexeme(Lexeme.WORD, tokenizer.sval));
						break;
					}

					default:
					{
						// operator
						if ((char)tokenizer.ttype.ToString().Equals("("))
						{
							tokBuf.AddItem(new Lexeme(Lexeme.LPAREN, (char)tokenizer.ttype.ToString()));
						}
						else
						{
							if ((char)tokenizer.ttype.ToString().Equals(")"))
							{
								tokBuf.AddItem(new Lexeme(Lexeme.RPAREN, (char)tokenizer.ttype.ToString()));
							}
							else
							{
								if ((char)tokenizer.ttype.ToString().Equals(","))
								{
									tokBuf.AddItem(new Lexeme(Lexeme.COMMA, (char)tokenizer.ttype.ToString()));
								}
								else
								{
									tokBuf.AddItem(new Lexeme(Lexeme.OPERATOR, (char)tokenizer.ttype.ToString()));
								}
							}
						}
						break;
						break;
					}
				}
			}
			return tokBuf;
		}

		internal enum TokenType
		{
			WORD,
			INT,
			DECIMAL,
			OPERATOR,
			UNSET
		}

		public static string[] GetAllMatches(string input)
		{
			Sharpen.Pattern p = Sharpen.Pattern.Compile("([0-9]*\\.[0-9]+|[0-9]+|[a-zA-Z]+|[^\\w\\s])"
				);
			Matcher m = p.Matcher(input);
			AList<string> matches = new AList<string>();
			while (m.Find())
			{
				matches.AddItem(m.Group());
			}
			string[] matchArr = new string[matches.Count];
			return Sharpen.Collections.ToArray(matches, matchArr);
		}

		public static AList<Lexeme> Lexify(string s)
		{
			//Pattern pat = Pattern.compile("0-9]*\\.[0-9]+|[0-9]+|[A-z]+|[\\W\\S]");
			//Matcher m = pat.matcher(s);
			int position = 0;
			string[] tokens = GetAllMatches(s);
			//String[] tokens = s.split("([0-9]*\\.[0-9]+|[0-9]+|[A-z]+|[\\W\\S])");
			foreach (string token in tokens)
			{
				System.Console.Out.WriteLine(token);
			}
			AList<Lexeme> tokBuf = new AList<Lexeme>();
			//try {
			foreach (string token_1 in tokens)
			{
				position += token_1.Length;
				Lexer.TokenType type = Lexer.TokenType.UNSET;
				if (token_1.Matches("^[0-9]+$"))
				{
					type = Lexer.TokenType.INT;
				}
				else
				{
					if (token_1.Matches("^[0-9]*\\.[0-9]+$"))
					{
						type = Lexer.TokenType.DECIMAL;
					}
					else
					{
						if (token_1.Matches("^([a-zA-Z]+)$"))
						{
							type = Lexer.TokenType.WORD;
						}
						else
						{
							if (token_1.Matches("^([^\\w\\s])$"))
							{
								type = Lexer.TokenType.OPERATOR;
							}
						}
					}
				}
				switch (type)
				{
					case Lexer.TokenType.INT:
					{
						// int
						tokBuf.AddItem(new Lexeme(Lexeme.NUMBER, token_1));
						break;
					}

					case Lexer.TokenType.DECIMAL:
					{
						// double
						tokBuf.AddItem(new Lexeme(Lexeme.NUMBER, token_1));
						break;
					}

					case Lexer.TokenType.WORD:
					{
						tokBuf.AddItem(new Lexeme(Lexeme.WORD, token_1));
						break;
					}

					case Lexer.TokenType.OPERATOR:
					{
						// operator
						if (token_1.Equals("("))
						{
							tokBuf.AddItem(new Lexeme(Lexeme.LPAREN, token_1));
						}
						else
						{
							if (token_1.Equals(")"))
							{
								tokBuf.AddItem(new Lexeme(Lexeme.RPAREN, token_1));
							}
							else
							{
								if (token_1.Equals(","))
								{
									tokBuf.AddItem(new Lexeme(Lexeme.COMMA, token_1));
								}
								else
								{
									tokBuf.AddItem(new Lexeme(Lexeme.OPERATOR, token_1));
								}
							}
						}
						break;
					}
				}
			}
			return tokBuf;
		}
	}
}
