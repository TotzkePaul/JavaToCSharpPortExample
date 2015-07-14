/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class VariableNode : Expression
	{
		protected internal double val = 0.0;

		protected internal string name;

		public VariableNode(string name)
		{
			this.name = name;
		}

		/// <summary>Returns the value.</summary>
		/// <remarks>Returns the value.</remarks>
		public override double Eval(VarMap v, FuncMap f)
		{
			return v.GetValue(name);
		}

		public override Expression[] GetChildren()
		{
			// TODO Auto-generated method stub
			return new Expression[0];
		}

		public override string ToString()
		{
			return name;
		}

		public override bool IsLeaf()
		{
			// TODO Auto-generated method stub
			return true;
		}

		public override int GetPrecedence()
		{
			// TODO Auto-generated method stub
			return 0;
		}

		public override Expression DeepCopy()
		{
			// TODO Auto-generated method stub
			return new MathExpression.VariableNode(name);
		}
	}
}
