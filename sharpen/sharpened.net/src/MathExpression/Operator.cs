/*
 * Converted from Java
 */

using Sharpen;

namespace MathExpression
{
	public abstract class Operator
	{
		internal bool leftassociative;

		internal bool binary;

		internal bool isFunc;

		internal bool opLogs = false;

		internal int precedence;

		internal int numOfOperands;

		internal string symbol;

		public Operator(string symbol, int precedence, bool leftassociative) : this(symbol
			, precedence, leftassociative, true)
		{
		}

		public Operator(string symbol, int precedence, bool leftassociative, bool binary)
		{
			this.symbol = symbol;
			this.precedence = precedence;
			this.leftassociative = leftassociative;
			this.binary = binary;
			if (binary)
			{
				numOfOperands = 2;
			}
			else
			{
				numOfOperands = 1;
			}
			this.isFunc = false;
		}

		public Operator(string symbol, int precedence, int numOfOperands, bool isFunc)
		{
			this.symbol = symbol;
			this.precedence = precedence;
			this.leftassociative = true;
			this.binary = false;
			this.numOfOperands = numOfOperands;
			this.isFunc = isFunc;
		}

		public abstract double Eval(double[] inputs);

		public virtual bool IsAtomicOperator()
		{
			return true;
		}

		public virtual int GetOperandsSize()
		{
			return numOfOperands;
		}

		public virtual string GetSymbol()
		{
			return symbol;
		}

		public virtual int GetPrecednce()
		{
			return precedence;
		}

		public virtual bool IsLeftAssociative()
		{
			return leftassociative;
		}

		public virtual bool IsFunc()
		{
			return isFunc;
		}

		public virtual bool IsBinary()
		{
			return binary;
		}

		public override string ToString()
		{
			string assoc = (leftassociative) ? "Left" : "Right";
			int operands = (binary) ? 2 : 1;
			return "[\"" + symbol + "\": precedence:" + precedence + " assoc:" + assoc + " operands:"
				 + operands + "]";
		}
	}
}
