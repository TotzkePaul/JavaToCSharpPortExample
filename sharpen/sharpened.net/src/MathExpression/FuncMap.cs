/*
 * Converted from Java
 */

using System;
using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class FuncMap
	{
		private string[] name = new string[50];

		private Function[] func = new Function[50];

		private int numFunc = 0;

		private bool caseSensitive = false;

		public FuncMap()
		{
		}

		public FuncMap(bool caseSensitive)
		{
			this.caseSensitive = caseSensitive;
		}

		/// <summary>Adds the mappings for many common functions.</summary>
		/// <remarks>Adds the mappings for many common functions.  The names are specified in all lowercase letters.
		/// 	</remarks>
		public virtual void LoadDefaultFunctions()
		{
		}

		/// <summary>Returns a function based on the name and the specified number of parameters.
		/// 	</summary>
		/// <remarks>Returns a function based on the name and the specified number of parameters.
		/// 	</remarks>
		/// <exception cref="Sharpen.RuntimeException">If no supporting function can be found.
		/// 	</exception>
		public virtual Function GetFunction(string funcName, int numParam)
		{
			for (int i = 0; i < numFunc; i++)
			{
				if (func[i].AcceptNumParam(numParam) && (caseSensitive && name[i].Equals(funcName
					) || !caseSensitive && Sharpen.Runtime.EqualsIgnoreCase(name[i], funcName)))
				{
					return func[i];
				}
			}
			throw new RuntimeException("function not found: " + funcName + " " + numParam);
		}

		/// <summary>Assigns the name to map to the specified function.</summary>
		/// <remarks>Assigns the name to map to the specified function.</remarks>
		/// <exception cref="System.ArgumentException">If any of the parameters are null.</exception>
		public virtual void SetFunction(string funcName, Function f)
		{
			if (funcName == null)
			{
				throw new ArgumentException("function name cannot be null");
			}
			if (f == null)
			{
				throw new ArgumentException("function cannot be null");
			}
			for (int i = 0; i < numFunc; i++)
			{
				if (caseSensitive && name[i].Equals(funcName) || !caseSensitive && Sharpen.Runtime.EqualsIgnoreCase
					(name[i], funcName))
				{
					func[i] = f;
					return;
				}
			}
			if (numFunc == name.Length)
			{
				string[] tmp1 = new string[2 * numFunc];
				Function[] tmp2 = new Function[tmp1.Length];
				for (int i_1 = 0; i_1 < numFunc; i_1++)
				{
					tmp1[i_1] = name[i_1];
					tmp2[i_1] = func[i_1];
				}
				name = tmp1;
				func = tmp2;
			}
			name[numFunc] = funcName;
			func[numFunc] = f;
			numFunc++;
		}

		/// <summary>Returns true if the case of the function names is considered.</summary>
		/// <remarks>Returns true if the case of the function names is considered.</remarks>
		public virtual bool IsCaseSensitive()
		{
			return caseSensitive;
		}

		/// <summary>Returns an array of exact length of the function names stored in this map.
		/// 	</summary>
		/// <remarks>Returns an array of exact length of the function names stored in this map.
		/// 	</remarks>
		public virtual string[] GetFunctionNames()
		{
			string[] arr = new string[numFunc];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = name[i];
			}
			return arr;
		}

		/// <summary>Returns an array of exact length of the functions stored in this map.</summary>
		/// <remarks>
		/// Returns an array of exact length of the functions stored in this map.  The returned
		/// array corresponds to the order of the names returned by getFunctionNames.
		/// </remarks>
		public virtual Function[] GetFunctions()
		{
			Function[] arr = new Function[numFunc];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = func[i];
			}
			return arr;
		}

		/// <summary>Removes the function-name and the associated function from the map.</summary>
		/// <remarks>
		/// Removes the function-name and the associated function from the map.  Does nothing if the function-name
		/// is not found.
		/// </remarks>
		public virtual void Remove(string funcName)
		{
			for (int i = 0; i < numFunc; i++)
			{
				if (caseSensitive && name[i].Equals(funcName) || !caseSensitive && Sharpen.Runtime.EqualsIgnoreCase
					(name[i], funcName))
				{
					for (int j = i + 1; j < numFunc; j++)
					{
						name[j - 1] = name[j];
						func[j - 1] = func[j];
					}
					numFunc--;
					name[numFunc] = null;
					func[numFunc] = null;
					break;
				}
			}
		}
	}
}
