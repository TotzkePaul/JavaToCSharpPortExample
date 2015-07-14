/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class OpNode : Expression
	{
		protected internal Expression[] children = null;

		protected internal Operator @operator;

		public OpNode(Expression[] children, Operator oper)
		{
			this.@operator = oper;
			this.children = children;
		}

		public override double Eval(VarMap v, FuncMap f)
		{
			double[] values = new double[children.Length];
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = children[i].Eval(v, f);
			}
			return @operator.Eval(values);
		}

		public override Expression[] GetChildren()
		{
			return children;
		}

		public override string ToString()
		{
			string mySymbol = @operator.GetSymbol();
			if (children.Length == 1)
			{
				if (@operator.IsLeftAssociative())
				{
					return children[0].ToString() + mySymbol;
				}
				else
				{
					return mySymbol + children[0].ToString();
				}
			}
			if (children.Length == 2)
			{
				return children[0].ToString() + mySymbol + children[1].ToString();
			}
			else
			{
				return "ERROR";
			}
		}

		public override bool IsLeaf()
		{
			// TODO Auto-generated method stub
			return false;
		}

		public override int GetPrecedence()
		{
			// TODO Auto-generated method stub
			return @operator.GetPrecednce();
		}

		public override Expression DeepCopy()
		{
			// TODO Auto-generated method stub
			Expression[] dcChildren = new Expression[children.Length];
			for (int i = 0; i < dcChildren.Length; i++)
			{
				dcChildren[i] = children[i].DeepCopy();
			}
			return new MathExpression.OpNode(dcChildren, @operator);
		}
	}
}
