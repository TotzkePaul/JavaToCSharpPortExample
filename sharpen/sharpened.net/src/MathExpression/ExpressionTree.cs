/*
 * Converted from Java
 */

using System;
using MathExpression;
using Sharpen;

namespace MathExpression
{
	public class ExpressionTree
	{
		public static bool debuggging = false;

		public ExpressionTree()
		{
		}

		/// <summary>Returns an expression-tree that represents the expression string.</summary>
		/// <remarks>Returns an expression-tree that represents the expression string.  Returns null if the string is empty.
		/// 	</remarks>
		/// <exception cref="ExpressionParseException">If the string is invalid.</exception>
		public static Expression Parse(string s)
		{
			if (s == null)
			{
				throw new ExpressionParseException("Expression string cannot be null.", -1);
			}
			AList<Lexeme> lexemes = Lexify(s);
			//Make tokens
			System.Console.Out.WriteLine("Lexemes:");
			foreach (Lexeme item in lexemes)
			{
				System.Console.Out.Write(item.ToString());
			}
			Operator[] operators = GetOperators();
			string nl = Runtime.GetProperty("line.separator");
			System.Console.Out.WriteLine(nl + "Operators:");
			foreach (Operator oper in operators)
			{
				System.Console.Out.Write(oper.ToString() + " ");
			}
			System.Console.Out.Write(nl);
			//build tree
			Stack<Expression> terms = new Stack<Expression>();
			Stack<Expression> exprOps = new Stack<Expression>();
			Stack<Expression> exprList = new Stack<Expression>();
			Stack<Operator> opStack = new Stack<Operator>();
			Stack<Lexeme> operands = new Stack<Lexeme>();
			// contains expression nodes
			Stack<Lexeme> ops = new Stack<Lexeme>();
			// contains open brackets ( and operators ^,*,/,+,-
			//boolean term = true; // indicates a term should come next, not an operator
			//boolean signed = false; // indicates if the current term has been signed
			//boolean negate = false; // indicates if the sign of the current term is negated
			bool inTermOrPostOp = false;
			//1. While there are still tokens to be read in
			for (int i = 0; i < lexemes.Count; i++)
			{
				//1.1 Get the next token.
				Expression contents;
				Operator currentOperator;
				Lexeme current = lexemes[i];
				switch (current.GetType())
				{
					case Lexeme.NUMBER:
					{
						//1.2.1 A number: push it onto the value stack.
						inTermOrPostOp = true;
						operands.AddItem(current);
						if (debuggging)
						{
							System.Console.Out.WriteLine("push term: " + current.GetValue());
						}
						terms.AddItem(new NumberNode(current.GetValue()));
						break;
					}

					case Lexeme.WORD:
					{
						//1.2.2 A variable: get its value, and push onto the value stack.
						//maybe variable, maybe function name?
						operands.AddItem(current);
						inTermOrPostOp = true;
						terms.AddItem(new VariableNode(current.GetValue()));
						if (debuggging)
						{
							System.Console.Out.WriteLine("push term: " + current.GetValue());
						}
						break;
					}

					case Lexeme.LPAREN:
					{
						//1.2.3 A left parenthesis: push it onto the operator stack.
						inTermOrPostOp = false;
						//is function
						if (!operands.IsEmpty() && operands.Peek().GetType() == Lexeme.WORD)
						{
							Expression funcName = terms.Pop();
							currentOperator = GetOperatorFromLexeme(operators, operands.Peek(), inTermOrPostOp
								);
							opStack.AddItem(currentOperator);
							if (debuggging)
							{
								System.Console.Out.WriteLine("push: " + currentOperator.GetSymbol());
							}
						}
						ops.AddItem(current);
						currentOperator = GetOperatorFromLexeme(operators, current, inTermOrPostOp);
						opStack.AddItem(currentOperator);
						if (debuggging)
						{
							System.Console.Out.WriteLine("push: " + currentOperator.GetSymbol());
						}
						break;
					}

					case Lexeme.COMMA:
					{
						operands.AddItem(current);
						while (!opStack.IsEmpty() && (!opStack.Peek().GetSymbol().Equals("(") && !opStack
							.Peek().GetSymbol().Equals(",")))
						{
							Operator topOperator = opStack.Pop();
							if (debuggging)
							{
								System.Console.Out.WriteLine("pop: " + topOperator.GetSymbol());
							}
							Expression[] children;
							if (topOperator.IsBinary())
							{
								Expression operand1 = terms.Pop();
								Expression operand2 = terms.Pop();
								children = new Expression[] { operand2, operand1 };
							}
							else
							{
								//check assoc here
								Expression operand = terms.Pop();
								children = new Expression[] { operand };
							}
							terms.AddItem(new OpNode(children, topOperator));
						}
						inTermOrPostOp = false;
						if (!opStack.IsEmpty())
						{
							opStack.Pop();
						}
						else
						{
							// remove '(' paren from stack;
							throw new ExpressionParseException("Expression string cannot missing leading (" +
								 current, i);
						}
						currentOperator = GetOperatorFromLexeme(operators, current, inTermOrPostOp);
						opStack.AddItem(currentOperator);
						//terms.add(terms.pop());
						break;
					}

					case Lexeme.RPAREN:
					{
						//1.2.4 A right parenthesis:
						operands.AddItem(current);
						while (!opStack.IsEmpty() && (!opStack.Peek().GetSymbol().Equals("(") && !opStack
							.Peek().GetSymbol().Equals(",")))
						{
							Operator topOperator = opStack.Pop();
							if (debuggging)
							{
								System.Console.Out.WriteLine("pop: " + topOperator.GetSymbol());
							}
							Expression[] children;
							if (topOperator.IsBinary())
							{
								Expression operand1 = terms.Pop();
								Expression operand2 = terms.Pop();
								children = new Expression[] { operand2, operand1 };
							}
							else
							{
								//check assoc here
								Expression operand = terms.Pop();
								children = new Expression[] { operand };
							}
							terms.AddItem(new OpNode(children, topOperator));
						}
						inTermOrPostOp = true;
						//maybe check if paren?
						if (!opStack.IsEmpty())
						{
							opStack.Pop();
						}
						else
						{
							// remove '(' paren from stack;
							throw new ExpressionParseException("Expression string cannot missing leading (", 
								i);
						}
						if (!opStack.IsEmpty() && opStack.Peek().IsFunc())
						{
							AList<Expression> args = new AList<Expression>();
							while (!terms.IsEmpty())
							{
								if (args.Count < opStack.Peek().GetOperandsSize())
								{
									args.AddItem(terms.Pop());
								}
								else
								{
									break;
								}
							}
							//check if args.size()==opStack.peek().getOperandsSize()
							Collections.Reverse(args);
							terms.AddItem(new FuncNode(opStack.Peek().GetSymbol(), Sharpen.Collections.ToArray
								(args, new Expression[args.Count]), opStack.Pop()));
						}
						else
						{
							contents = terms.Pop();
							terms.AddItem(new ParensNode(new Expression[] { contents }));
						}
						break;
					}

					case Lexeme.OPERATOR:
					{
						Lexeme thisOp = current;
						currentOperator = GetOperatorFromLexeme(operators, current, inTermOrPostOp);
						inTermOrPostOp = false;
						operands.AddItem(current);
						//Operator topOp;
						while (!opStack.IsEmpty() && opStack.Peek().GetPrecednce() >= currentOperator.GetPrecednce
							())
						{
							//Lexeme poppedOp = ops.pop();
							Operator topOperator = opStack.Pop();
							Expression[] children;
							if (topOperator.IsBinary())
							{
								Expression operand1 = terms.Pop();
								Expression operand2 = terms.Pop();
								children = new Expression[] { operand2, operand1 };
							}
							else
							{
								//check assoc here
								Expression operand = terms.Pop();
								children = new Expression[] { operand };
							}
							terms.AddItem(new OpNode(children, topOperator));
						}
						//Apply the operator to the operands, in the correct order.
						//4 Push the result onto the value stack.
						//might have to go x->pop(), y->pop(); push(x) for right assoc unary
						//ops.add(current);
						opStack.AddItem(currentOperator);
						break;
					}

					default:
					{
						throw new ExpressionParseException("Unknown token type", -1);
					}
				}
			}
			//2. While the operator stack is not empty
			while (!opStack.IsEmpty())
			{
				Operator poppedOp = opStack.Pop();
				Expression[] children;
				if (poppedOp.IsBinary())
				{
					Expression operand1 = terms.Pop();
					Expression operand2 = terms.Pop();
					children = new Expression[] { operand2, operand1 };
				}
				else
				{
					//check assoc here
					Expression operand = terms.Pop();
					children = new Expression[] { operand };
				}
				terms.AddItem(new OpNode(children, poppedOp));
			}
			return terms.Peek();
		}

		private static Expression Build(string s, int indexErrorOffset)
		{
			return null;
		}

		private static Operator GetOperatorFromLexeme(Operator[] ops, Lexeme lex, bool isBinary
			)
		{
			foreach (Operator op in ops)
			{
				if (lex.GetType() == Lexeme.OPERATOR && op.IsBinary() == isBinary && lex.GetValue
					().Equals(op.GetSymbol()))
				{
					return op;
				}
				else
				{
					if (lex.GetType() == Lexeme.LPAREN && op.GetSymbol().Equals("("))
					{
						return op;
					}
					else
					{
						if (lex.GetType() == Lexeme.RPAREN && op.GetSymbol().Equals(")"))
						{
							return op;
						}
						else
						{
							if (lex.GetType() == Lexeme.WORD && op.GetSymbol().Equals(lex.GetValue()))
							{
								return op;
							}
							else
							{
								if (lex.GetType() == Lexeme.COMMA && op.GetSymbol().Equals(","))
								{
									return op;
								}
							}
						}
					}
				}
			}
			throw new ExpressionParseException("Expression string cannot be tokenized at " + 
				lex.ToString() + " isBinary:" + isBinary, -1);
		}

		private static Operator[] GetOperatorsFromLexeme(Operator[] ops, Lexeme lex)
		{
			AList<Operator> list = new AList<Operator>();
			foreach (Operator op in ops)
			{
				if (lex.GetType() == Lexeme.OPERATOR && lex.GetValue().Equals(op.symbol))
				{
					list.AddItem(op);
				}
			}
			return Sharpen.Collections.ToArray(list, new Operator[list.Count]);
		}

		private static Operator[] GetOperators()
		{
			return new Operator[] { new _Operator_277("+", 1, true), new _Operator_284("-", 1
				, true), new _Operator_291("*", 2, true), new _Operator_298("/", 2, true), new _Operator_305
				("%", 2, true), new _Operator_311("!", 3, true, false), new _Operator_319("-", 3
				, false, false), new _Operator_326("+", 3, false, false), new _Operator_333("(", 
				0, true, false), new _Operator_343(")", 4, true, false), new _Operator_354(",", 
				0, true, false), new _Operator_365("log", 0, 2, true), new _Operator_376("pow", 
				0, 2, true) };
		}

		private sealed class _Operator_277 : Operator
		{
			public _Operator_277(string baseArg1, int baseArg2, bool baseArg3) : base(baseArg1
				, baseArg2, baseArg3)
			{
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine(inputs[0] + "+" + inputs[1]);
				}
				return inputs[0] + inputs[1];
			}
		}

		private sealed class _Operator_284 : Operator
		{
			public _Operator_284(string baseArg1, int baseArg2, bool baseArg3) : base(baseArg1
				, baseArg2, baseArg3)
			{
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine(inputs[0] + "-" + inputs[1]);
				}
				return inputs[0] - inputs[1];
			}
		}

		private sealed class _Operator_291 : Operator
		{
			public _Operator_291(string baseArg1, int baseArg2, bool baseArg3) : base(baseArg1
				, baseArg2, baseArg3)
			{
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine(inputs[0] + "*" + inputs[1]);
				}
				return inputs[0] * inputs[1];
			}
		}

		private sealed class _Operator_298 : Operator
		{
			public _Operator_298(string baseArg1, int baseArg2, bool baseArg3) : base(baseArg1
				, baseArg2, baseArg3)
			{
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine(inputs[0] + "/" + inputs[1]);
				}
				return inputs[0] / inputs[1];
			}
		}

		private sealed class _Operator_305 : Operator
		{
			public _Operator_305(string baseArg1, int baseArg2, bool baseArg3) : base(baseArg1
				, baseArg2, baseArg3)
			{
			}

			public override double Eval(double[] inputs)
			{
				return inputs[0] % inputs[1];
			}
		}

		private sealed class _Operator_311 : Operator
		{
			public _Operator_311(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine(inputs[0] + "!");
				}
				double x = inputs[0] + 1.0;
				return Math.Sqrt(2 * Math.PI / x) * Math.Pow((x / Math.E), x);
			}
		}

		private sealed class _Operator_319 : Operator
		{
			public _Operator_319(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			//unary -
			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine("-" + inputs[0]);
				}
				return -inputs[0];
			}
		}

		private sealed class _Operator_326 : Operator
		{
			public _Operator_326(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			//unary +
			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.WriteLine("+" + inputs[0]);
				}
				return inputs[0];
			}
		}

		private sealed class _Operator_333 : Operator
		{
			public _Operator_333(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			public override bool IsAtomicOperator()
			{
				return false;
			}

			public override double Eval(double[] inputs)
			{
				throw new ExpressionParseException("Expression string '(' cannot eval ", -1);
			}
		}

		private sealed class _Operator_343 : Operator
		{
			public _Operator_343(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			public override bool IsAtomicOperator()
			{
				return false;
			}

			public override double Eval(double[] inputs)
			{
				throw new ExpressionParseException("Expression string ')' cannot eval ", -1);
			}
		}

		private sealed class _Operator_354 : Operator
		{
			public _Operator_354(string baseArg1, int baseArg2, bool baseArg3, bool baseArg4)
				 : base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			//return inputs[0];
			public override bool IsAtomicOperator()
			{
				return false;
			}

			public override double Eval(double[] inputs)
			{
				throw new ExpressionParseException("Expression string ',' cannot eval ", -1);
			}
		}

		private sealed class _Operator_365 : Operator
		{
			public _Operator_365(string baseArg1, int baseArg2, int baseArg3, bool baseArg4) : 
				base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			//return inputs[0];
			public override bool IsAtomicOperator()
			{
				return false;
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.Write("log(" + inputs[0] + ")/log(" + inputs[1] + ")");
				}
				return Math.Log(inputs[0]) / Math.Log(inputs[1]);
			}
		}

		private sealed class _Operator_376 : Operator
		{
			public _Operator_376(string baseArg1, int baseArg2, int baseArg3, bool baseArg4) : 
				base(baseArg1, baseArg2, baseArg3, baseArg4)
			{
			}

			public override bool IsAtomicOperator()
			{
				return false;
			}

			public override double Eval(double[] inputs)
			{
				if (this.opLogs)
				{
					System.Console.Out.Write("pow(" + inputs[0] + "/" + inputs[1] + ")");
				}
				return Math.Pow(inputs[0], inputs[1]);
			}
		}

		private static string FormatNumber(string str)
		{
			if (!Sharpen.Pattern.Compile("\\-?[0-9]*(\\.[0-9])|\\-?([0-9]+)").Matcher(str).Matches
				())
			{
				return str;
			}
			double dbl = double.ParseDouble(str);
			double f = dbl;
			int i = (int)f;
			if (f == i)
			{
				return Sharpen.Extensions.ToString(i);
			}
			return str;
		}

		public static string[] GetAllMatches(string input)
		{
			Sharpen.Pattern p = Sharpen.Pattern.Compile("([0-9]*\\.[0-9]+|[0-9]+|[a-zA-Z]+|[^\\w\\s])"
				);
			Matcher m = p.Matcher(input);
			AList<string> matches = new AList<string>();
			while (m.Find())
			{
				matches.AddItem(m.Group());
			}
			string[] matchArr = new string[matches.Count];
			return Sharpen.Collections.ToArray(matches, matchArr);
		}

		public static AList<Lexeme> Lexify(string s)
		{
			int position = 0;
			string[] tokens = GetAllMatches(s);
			foreach (string token in tokens)
			{
				System.Console.Out.WriteLine(token);
			}
			AList<Lexeme> tokBuf = new AList<Lexeme>();
			//try {
			foreach (string token_1 in tokens)
			{
				position += token_1.Length;
				Lexer.TokenType type = Lexer.TokenType.UNSET;
				if (token_1.Matches("^[0-9]+$"))
				{
					type = Lexer.TokenType.INT;
				}
				else
				{
					if (token_1.Matches("^[0-9]*\\.[0-9]+$"))
					{
						type = Lexer.TokenType.DECIMAL;
					}
					else
					{
						if (token_1.Matches("^([a-zA-Z]+)$"))
						{
							type = Lexer.TokenType.WORD;
						}
						else
						{
							if (token_1.Matches("^([^\\w\\s])$"))
							{
								type = Lexer.TokenType.OPERATOR;
							}
						}
					}
				}
				switch (type)
				{
					case Lexer.TokenType.INT:
					{
						// int
						tokBuf.AddItem(new Lexeme(Lexeme.NUMBER, token_1));
						break;
					}

					case Lexer.TokenType.DECIMAL:
					{
						// double
						tokBuf.AddItem(new Lexeme(Lexeme.NUMBER, token_1));
						break;
					}

					case Lexer.TokenType.WORD:
					{
						tokBuf.AddItem(new Lexeme(Lexeme.WORD, token_1));
						break;
					}

					case Lexer.TokenType.OPERATOR:
					{
						// operator
						if (token_1.Equals("("))
						{
							tokBuf.AddItem(new Lexeme(Lexeme.LPAREN, token_1));
						}
						else
						{
							if (token_1.Equals(")"))
							{
								tokBuf.AddItem(new Lexeme(Lexeme.RPAREN, token_1));
							}
							else
							{
								if (token_1.Equals(","))
								{
									tokBuf.AddItem(new Lexeme(Lexeme.COMMA, token_1));
								}
								else
								{
									tokBuf.AddItem(new Lexeme(Lexeme.OPERATOR, token_1));
								}
							}
						}
						break;
					}
				}
			}
			return tokBuf;
		}
	}
}
