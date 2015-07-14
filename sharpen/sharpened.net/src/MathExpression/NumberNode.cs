/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class NumberNode : Expression
	{
		protected internal double val = 0.0;

		public NumberNode(double d)
		{
			val = d;
		}

		public NumberNode(string s)
		{
			val = double.ParseDouble(s);
		}

		/// <summary>Returns the value.</summary>
		/// <remarks>Returns the value.</remarks>
		public override double Eval(VarMap v, FuncMap f)
		{
			return val;
		}

		public virtual double GetValue()
		{
			return val;
		}

		public virtual void SetValue(double d)
		{
			val = d;
		}

		public override Expression[] GetChildren()
		{
			// TODO Auto-generated method stub
			return new Expression[0];
		}

		public override string ToString()
		{
			string s = string.Format("%.3f", val);
			string temp = s.IndexOf(".") < 0 ? s : s.ReplaceAll("0*$", string.Empty).ReplaceAll
				("\\.$", string.Empty);
			temp = temp.IndexOf(".") < 0 ? temp + ".0" : temp;
			return temp;
		}

		//"" +val;
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
			return new MathExpression.NumberNode(val);
		}
	}
}
