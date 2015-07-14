/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class ParensNode : Expression
	{
		protected internal Expression[] children = null;

		public ParensNode(Expression[] children)
		{
			CheckBeforeAccept(children);
			this.children = children;
		}

		public override double Eval(VarMap v, FuncMap f)
		{
			double value = children[0].Eval(v, f);
			//System.out.print("("+value+")");
			return value;
		}

		//children[0].eval(v, f);
		public override Expression[] GetChildren()
		{
			// TODO Auto-generated method stub
			return children;
		}

		public override string ToString()
		{
			return "(" + children[0].ToString() + ")";
		}

		public override bool IsLeaf()
		{
			// TODO Auto-generated method stub
			if (children != null && children.Length == 1 && children[0] is NumberNode)
			{
				return true;
			}
			return false;
		}

		public override int GetPrecedence()
		{
			// TODO Auto-generated method stub
			return int.MaxValue;
		}

		public override Expression DeepCopy()
		{
			Expression[] dcChildren = new Expression[children.Length];
			for (int i = 0; i < dcChildren.Length; i++)
			{
				dcChildren[i] = children[i].DeepCopy();
			}
			return new MathExpression.ParensNode(dcChildren);
		}
	}
}
