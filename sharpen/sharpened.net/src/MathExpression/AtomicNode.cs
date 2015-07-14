/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class AtomicNode
	{
		protected internal Expression child = null;

		protected internal Operator @operator;

		protected internal string name;

		public AtomicNode(string name, Expression child, Operator oper)
		{
			//setChild(child);
			this.@operator = oper;
			this.name = name;
		}

		public virtual double Eval(VarMap v, FuncMap f)
		{
			double a = child.Eval(v, f);
			return @operator.Eval(new double[] { a });
		}

		public virtual Expression[] GetChildren()
		{
			// TODO Auto-generated method stub
			return null;
		}

		public virtual bool IsLeaf()
		{
			// TODO Auto-generated method stub
			return false;
		}
	}
}
