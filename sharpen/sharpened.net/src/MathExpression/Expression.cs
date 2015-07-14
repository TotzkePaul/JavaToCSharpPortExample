/*
 * Converted from Java
 */

using System;
using System.Text;
using MathExpression;
using Sharpen;

namespace MathExpression
{
	public abstract class Expression
	{
		protected internal Expression parent = null;

		/// <summary>Returns the result of evaluating the expression tree rooted at this node.
		/// 	</summary>
		/// <remarks>Returns the result of evaluating the expression tree rooted at this node.
		/// 	</remarks>
		public abstract double Eval(VarMap v, FuncMap f);

		public abstract Expression[] GetChildren();

		public abstract int GetPrecedence();

		public abstract Expression DeepCopy();

		//public abstract String getName();
		public abstract bool IsLeaf();

		public virtual Expression ReduceExpression(VarMap v, FuncMap f)
		{
			if (this.IsLeaf())
			{
				return null;
			}
			Expression newExpr = this.DeepCopy();
			Expression reductase = DeepestReducableNode(newExpr);
			Expression replacement = new NumberNode(reductase.Eval(v, f));
			//System.out.println("copy: " +newExpr);
			//System.out.println("rem " +reductase);
			//System.out.println("add "+ replacement);
			if (newExpr == reductase)
			{
				//System.out.println(" copy == high p");
				return replacement;
			}
			else
			{
				//System.out.println("replacement");
				ReplaceNodeInTree(newExpr, reductase, replacement);
				return newExpr;
			}
		}

		private void ReplaceNodeInTree(Expression tree, Expression rem, Expression add)
		{
			Expression[] treeChildren = tree.GetChildren();
			for (int i = 0; i < treeChildren.Length; i++)
			{
				if (treeChildren[i] == rem)
				{
					treeChildren[i] = add;
				}
				else
				{
					ReplaceNodeInTree(treeChildren[i], rem, add);
				}
			}
		}

		private Expression DeepestReducableNode(Expression node)
		{
			if (node.IsLeaf())
			{
				return node;
			}
			else
			{
				Expression[] myChildren = node.GetChildren();
				Expression firstHighestPrec = null;
				bool allLeafNode = true;
				for (int i = 0; i < myChildren.Length; i++)
				{
					if (!myChildren[i].IsLeaf())
					{
						allLeafNode = false;
					}
					Expression current = DeepestReducableNode(myChildren[i]);
					if (firstHighestPrec == null || firstHighestPrec.GetPrecedence() < myChildren[i].
						GetPrecedence())
					{
						firstHighestPrec = current;
					}
				}
				if (firstHighestPrec == null || allLeafNode)
				{
					return node;
				}
				else
				{
					return firstHighestPrec;
				}
			}
		}

		//return null;
		/// <summary>Returns true if this node is a descendent of the specified node, false otherwise.
		/// 	</summary>
		/// <remarks>
		/// Returns true if this node is a descendent of the specified node, false otherwise.  By this
		/// methods definition, a node is a descendent of itself.
		/// </remarks>
		public virtual bool IsDescendent(Expression x)
		{
			Expression y = this;
			while (y != null)
			{
				if (y == x)
				{
					return true;
				}
				y = y.parent;
			}
			return false;
		}

		/// <summary>Returns the parent of this node.</summary>
		/// <remarks>Returns the parent of this node.</remarks>
		public virtual Expression GetParent()
		{
			return parent;
		}

		public virtual void Print()
		{
			Print(string.Empty, false);
		}

		public virtual void Print(string prefix, bool isTail)
		{
			Expression[] children = GetChildren();
			System.Console.Out.WriteLine(prefix + (isTail ? "â””â”€â”€ " : "â”œâ”€â”€ ") + ToString
				());
			for (int i = 0; i < children.Length - 1; i++)
			{
				children[i].Print(prefix + (isTail ? "    " : "â”‚   "), false);
			}
			if (children.Length > 0)
			{
				children[(children.Length - 1)].Print(prefix + (isTail ? "    " : "â”‚   "), true
					);
			}
		}

		/// <summary>
		/// Protected method used to verify that the specified expression can be included as a child
		/// expression of this node.
		/// </summary>
		/// <remarks>
		/// Protected method used to verify that the specified expression can be included as a child
		/// expression of this node.
		/// </remarks>
		/// <exception cref="System.ArgumentException">If the specified expression is not accepted.
		/// 	</exception>
		protected internal virtual void CheckBeforeAccept(Expression[] x)
		{
			if (x == null)
			{
				throw new ArgumentException("expression cannot be null");
			}
			foreach (Expression child in x)
			{
				if (child.parent != null)
				{
					throw new ArgumentException("expression must be removed parent");
				}
				if (IsDescendent(child))
				{
					throw new ArgumentException("cyclic reference");
				}
			}
		}

		/// <summary>Returns a string that represents the expression tree rooted at this node.
		/// 	</summary>
		/// <remarks>Returns a string that represents the expression tree rooted at this node.
		/// 	</remarks>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			ToString(this, sb);
			return sb.ToString();
		}

		private static void ToString(Expression x, StringBuilder sb)
		{
			return;
		}
	}
}
