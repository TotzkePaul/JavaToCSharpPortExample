/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class FuncNode : Expression
	{
		protected internal Expression[] children = null;

		protected internal Operator @operator;

		protected internal string name;

		public FuncNode(string name, Expression[] children, Operator oper)
		{
			CheckBeforeAccept(children);
			this.children = children;
			this.@operator = oper;
			this.name = name;
		}

		public override double Eval(VarMap v, FuncMap f)
		{
			double[] values = new double[children.Length];
			//System.out.print("func children "+children.length);
			for (int i = 0; i < values.Length; i++)
			{
				values[i] = children[i].Eval(v, f);
			}
			return @operator.Eval(values);
		}

		public override string ToString()
		{
			string myName = name + "(";
			string sep = string.Empty;
			foreach (Expression child in children)
			{
				myName += sep;
				sep = ",";
				myName += child.ToString();
			}
			myName += ")";
			return myName;
		}

		public override Expression[] GetChildren()
		{
			return children;
		}

		public override bool IsLeaf()
		{
			// TODO Auto-generated method stub
			for (int i = 0; i < children.Length; i++)
			{
				if (!children[i].IsLeaf())
				{
					return false;
				}
			}
			return true;
		}

		public override int GetPrecedence()
		{
			// TODO Auto-generated method stub
			return int.MaxValue;
		}

		public override Expression DeepCopy()
		{
			// TODO Auto-generated method stub
			Expression[] dcChildren = new Expression[children.Length];
			for (int i = 0; i < dcChildren.Length; i++)
			{
				dcChildren[i] = children[i].DeepCopy();
			}
			return new MathExpression.FuncNode(name, dcChildren, @operator);
		}
	}
}
