/*
 * Converted from Java
 */

using MathExpression;
using Sharpen;

namespace MathExpression
{
	public interface Function
	{
		/// <summary>Takes the specified double array as input and returns a double value.</summary>
		/// <remarks>
		/// Takes the specified double array as input and returns a double value.  Functions
		/// that accept a variable number of inputs can take numParam to be the number of inputs.
		/// </remarks>
		double Of(double[] param, int numParam);

		/// <summary>
		/// Returns true if the numParam is an accurate representation of the number of inputs
		/// the function processes.
		/// </summary>
		/// <remarks>
		/// Returns true if the numParam is an accurate representation of the number of inputs
		/// the function processes.
		/// </remarks>
		bool AcceptNumParam(int numParam);
	}
}
