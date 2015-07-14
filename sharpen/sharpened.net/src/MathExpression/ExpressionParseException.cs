/*
 * Converted from Java
 */

using Sharpen;

namespace MathExpression
{
	/// <summary>Exception thrown if expression cannot be parsed correctly.</summary>
	/// <remarks>Exception thrown if expression cannot be parsed correctly.</remarks>
	/// <seealso cref="com.graphbuilder.math.ExpressionTree">com.graphbuilder.math.ExpressionTree
	/// 	</seealso>
	[System.Serializable]
	public class ExpressionParseException : RuntimeException
	{
		private const long serialVersionUID = -1163684931776634074L;

		private string descrip = null;

		private int index = 0;

		public ExpressionParseException(string descrip, int index)
		{
			this.descrip = descrip;
			this.index = index;
		}

		public virtual string GetDescription()
		{
			return descrip;
		}

		public virtual int GetIndex()
		{
			return index;
		}

		public override string ToString()
		{
			return "(" + index + ") " + descrip;
		}
	}
}
