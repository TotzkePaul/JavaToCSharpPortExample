/*
 * Converted from Java
 */

using Sharpen;

namespace MathExpression
{
	public class Lexeme
	{
		public const int WORD = 1;

		public const int NUMBER = 2;

		public const int OPERATOR = 3;

		public const int LPAREN = 4;

		public const int RPAREN = 5;

		public const int COMMA = 6;

		internal int type;

		internal string value;

		public Lexeme(int type, string value)
		{
			this.type = type;
			this.value = value;
		}

		public virtual string GetValue()
		{
			return value;
		}

		public virtual int GetType()
		{
			return type;
		}

		public override string ToString()
		{
			string typeName;
			switch (type)
			{
				case WORD:
				{
					typeName = "WORD";
					return "<" + typeName + ": " + value + ">";
				}

				case NUMBER:
				{
					typeName = "NUMBER";
					return "<" + typeName + ": " + value + ">";
				}

				case OPERATOR:
				{
					typeName = "OPERATOR";
					return "<" + typeName + ": " + value + ">";
				}

				case LPAREN:
				{
					typeName = "LPAREN";
					return "<" + typeName + ">";
				}

				case RPAREN:
				{
					typeName = "RPAREN";
					return "<" + typeName + ">";
				}

				case COMMA:
				{
					typeName = "COMMA";
					return "<" + typeName + ">";
				}

				default:
				{
					typeName = "ERROR";
					return "<" + typeName + ": " + value + ">";
					break;
				}
			}
		}
	}
}
