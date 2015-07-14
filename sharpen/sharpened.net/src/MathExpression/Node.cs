/*
 * Converted from Java
 */

using System.Collections.Generic;
using Sharpen;

namespace MathExpression
{
	/// <summary>Created by Paul on 4/18/2015.</summary>
	/// <remarks>Created by Paul on 4/18/2015.</remarks>
	public class Node<T>
	{
		private T data;

		private IList<MathExpression.Node<T>> children;

		public Node(T data)
		{
			this.data = data;
		}

		public virtual T GetData()
		{
			return data;
		}

		public virtual void SetData(T data)
		{
			this.data = data;
		}

		public virtual IList<MathExpression.Node<T>> GetChildren()
		{
			return children;
		}

		public virtual MathExpression.Node<T> Get(int location)
		{
			if (location >= children.Count)
			{
				return null;
			}
			return children[location];
		}

		public virtual bool IsLeaf()
		{
			return children.Count == 0;
		}

		public virtual void SetChildren(IList<MathExpression.Node<T>> children)
		{
			this.children = children;
		}

		public virtual void AddChild(MathExpression.Node<T> child)
		{
			this.children.AddItem(child);
		}
	}
}
