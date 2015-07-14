package mathExpression;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Stack;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import mathExpression.Lexer.TokenType;

public class ExpressionTree {
	public static boolean debuggging = false;
	private ExpressionTree() {}
	/**
	Returns an expression-tree that represents the expression string.  Returns null if the string is empty.

	@throws ExpressionParseException If the string is invalid.
	*/
	@SuppressWarnings("unused")
	public static Expression parse(String s) {
		if (s == null)
			throw new ExpressionParseException("Expression string cannot be null.", -1);
		ArrayList<Lexeme> lexemes = lexify(s);
		//Make tokens
		System.out.println("Lexemes:");
		for(Lexeme item : lexemes){
			System.out.print(item.toString());
		}
		Operator[] operators = getOperators();
		String nl = System.getProperty("line.separator");
		System.out.println(nl + "Operators:");
		for(Operator oper : operators){
			System.out.print(oper.toString() + " " );
		}
		System.out.print(nl);
		//build tree
		Stack<Expression> terms = new Stack<Expression>();
		Stack<Expression> exprOps = new Stack<Expression>();
		Stack<Expression> exprList = new Stack<Expression>();
		Stack<Operator> opStack = new Stack<Operator>();
		Stack<Lexeme> operands = new Stack<Lexeme>(); // contains expression nodes
		Stack<Lexeme> ops = new Stack<Lexeme>(); // contains open brackets ( and operators ^,*,/,+,-
		

		//boolean term = true; // indicates a term should come next, not an operator
		//boolean signed = false; // indicates if the current term has been signed
		//boolean negate = false; // indicates if the sign of the current term is negated
		boolean inTermOrPostOp = false;
		
		//1. While there are still tokens to be read in
		for(int i =0; i<lexemes.size(); i++){
			//1.1 Get the next token.
			Expression contents;
			Operator currentOperator;
			Lexeme current = lexemes.get(i);
			
			
			switch(current.getType()){
			case Lexeme.NUMBER:
				//1.2.1 A number: push it onto the value stack.
				inTermOrPostOp = true;
				operands.add(current);
				if(debuggging)System.out.println("push term: "+current.getValue());
				terms.add(new NumberNode(current.getValue()));
				break;
			case Lexeme.WORD:
				//1.2.2 A variable: get its value, and push onto the value stack.
				//maybe variable, maybe function name?
				operands.add(current);
				inTermOrPostOp = true;
				terms.add(new VariableNode(current.getValue()));
				if(debuggging)System.out.println("push term: "+current.getValue());
				break;
			case Lexeme.LPAREN:
				//1.2.3 A left parenthesis: push it onto the operator stack.
				inTermOrPostOp = false;
				
				//is function
				if(!operands.isEmpty() && operands.peek().getType()==Lexeme.WORD){
					Expression funcName =  terms.pop();
					currentOperator = getOperatorFromLexeme(operators, operands.peek(), inTermOrPostOp);
					opStack.add(currentOperator);
					if(debuggging)System.out.println("push: "+ currentOperator.getSymbol());
				}
				
				ops.add(current);
				currentOperator = getOperatorFromLexeme(operators, current, inTermOrPostOp);
				opStack.add(currentOperator);
				if(debuggging)System.out.println("push: "+ currentOperator.getSymbol());
				break;
			case Lexeme.COMMA:
				operands.add(current);
				while(!opStack.isEmpty()
						&& (!opStack.peek().getSymbol().equals("(") 
								&& !opStack.peek().getSymbol().equals(",") )){
					Operator topOperator = opStack.pop();
					if(debuggging)System.out.println("pop: "+topOperator.getSymbol());
					Expression[] children;
					if(topOperator.isBinary()){
						Expression operand1 = terms.pop();
						Expression operand2 = terms.pop();
						children = new Expression[]{operand2, operand1};
					} else {//check assoc here
						Expression operand = terms.pop();
						children = new Expression[]{operand};
						
					}
					terms.add(new OpNode(children, topOperator));
				}
				inTermOrPostOp = false;
				if(!opStack.isEmpty()){
					opStack.pop(); // remove '(' paren from stack;
				} else {
					throw new ExpressionParseException("Expression string cannot missing leading (" + current,i);
				}
				currentOperator = getOperatorFromLexeme(operators, current, inTermOrPostOp);
				opStack.add(currentOperator);
				//terms.add(terms.pop());
				break;
			case Lexeme.RPAREN:
				//1.2.4 A right parenthesis:
/*				     1 While the thing on top of the operator stack is not a left parenthesis,
			             1 Pop the operator from the operator stack.
			             2 Pop the value stack twice, getting two operands.
			             3 Apply the operator to the operands, in the correct order.
			             4 Push the result onto the value stack.
			         2 Pop the left parenthesis from the operator stack, and discard it.*/
				operands.add(current);
				while(!opStack.isEmpty()
						&& (!opStack.peek().getSymbol().equals("(") 
								&& !opStack.peek().getSymbol().equals(",") )){
					Operator topOperator = opStack.pop();
					if(debuggging)System.out.println("pop: "+topOperator.getSymbol());
					Expression[] children;
					if(topOperator.isBinary()){
						Expression operand1 = terms.pop();
						Expression operand2 = terms.pop();
						children = new Expression[]{operand2, operand1};
					} else {//check assoc here
						Expression operand = terms.pop();
						children = new Expression[]{operand};
						
					}
					terms.add(new OpNode(children, topOperator));
				}
				inTermOrPostOp = true; //maybe check if paren?
				if(!opStack.isEmpty()){
					opStack.pop(); // remove '(' paren from stack;
				} else {
					throw new ExpressionParseException("Expression string cannot missing leading (",i);
				}
				if(!opStack.isEmpty() && opStack.peek().isFunc()){
					ArrayList<Expression> args = new ArrayList<>();
					while(!terms.isEmpty()){
						if(args.size()<opStack.peek().getOperandsSize()){
							args.add(terms.pop());
						} else {
							break;
						}
					}
					//check if args.size()==opStack.peek().getOperandsSize()
					Collections.reverse(args);
					terms.add(new FuncNode(opStack.peek().getSymbol(), args.toArray(new Expression[args.size()]), opStack.pop()));
				} else {
					contents = terms.pop();
					terms.add(new ParensNode(new Expression[]{contents}));
				}
				break;
			case Lexeme.OPERATOR:
/*	       1.2.5 An operator (call it thisOp):
		         1 While the operator stack is not empty, and the top thing on the
		           operator stack has the same or greater precedence as thisOp,
		           1 Pop the operator from the operator stack.
		           2 Pop the value stack twice, getting two operands.
		           3 Apply the operator to the operands, in the correct order.
		           4 Push the result onto the value stack.
		         2 Push thisOp onto the operator stack.*/
				Lexeme thisOp = current;
				currentOperator = getOperatorFromLexeme(operators, current, inTermOrPostOp);
				inTermOrPostOp = false;
				operands.add(current);
				
				//Operator topOp;
				while(!opStack.isEmpty() && opStack.peek().getPrecednce()>=currentOperator.getPrecednce()){
					//Lexeme poppedOp = ops.pop();
					Operator topOperator = opStack.pop();
					Expression[] children;
					if(topOperator.isBinary()){
						Expression operand1 = terms.pop();
						Expression operand2 = terms.pop();
						children = new Expression[]{operand2, operand1};
					} else {
						//check assoc here
						Expression operand = terms.pop();
						children = new Expression[]{operand};
						
					}
					terms.add(new OpNode(children, topOperator));
					//Apply the operator to the operands, in the correct order.
					//4 Push the result onto the value stack.
					//might have to go x->pop(), y->pop(); push(x) for right assoc unary
				}
				//ops.add(current);
				opStack.add(currentOperator);
				break;
			default:
				throw new ExpressionParseException("Unknown token type", -1);
			}
			
		}
		//2. While the operator stack is not empty
		while(!opStack.isEmpty()){
/*		    1 Pop the operator from the operator stack.
		    2 Pop the value stack twice, getting two operands.
		    3 Apply the operator to the operands, in the correct order.
		    4 Push the result onto the value stack.*/
			Operator poppedOp = opStack.pop();
			Expression[] children;
			if(poppedOp.isBinary()){
				Expression operand1 = terms.pop();
				Expression operand2 = terms.pop();
				children = new Expression[]{operand2, operand1};
			} else {
				//check assoc here
				Expression operand = terms.pop();
				children = new Expression[]{operand};
				
			}
			terms.add(new OpNode(children, poppedOp));
			
		}
/*		3. At this point the operator stack should be empty, and the value
		   stack should have only one value in it, which is the final result.*/

		return terms.peek();
	}

	@SuppressWarnings("unused")
	private static Expression build(String s, int indexErrorOffset) {
		
		return null;
	}
	
	private static Operator getOperatorFromLexeme(Operator[] ops, Lexeme lex, boolean isBinary){
		for(Operator op: ops){
			if(lex.getType()==Lexeme.OPERATOR && op.isBinary() == isBinary 
					&& lex.getValue().equals(op.getSymbol()) ){
				return op;
			} else if(lex.getType()==Lexeme.LPAREN && op.getSymbol().equals("(")) {
				return op;
			}else if(lex.getType()==Lexeme.RPAREN && op.getSymbol().equals(")")) {
				return op;
			}else if(lex.getType()==Lexeme.WORD && op.getSymbol().equals(lex.getValue())) {
				return op;
			} else if(lex.getType()==Lexeme.COMMA && op.getSymbol().equals(",")) {
				return op;
			}
		}
		throw new ExpressionParseException("Expression string cannot be tokenized at "+
				lex.toString()+ " isBinary:"+isBinary, -1); 
	}
	@SuppressWarnings("unused")
	private static Operator[] getOperatorsFromLexeme(Operator[] ops, Lexeme lex ){
		ArrayList<Operator> list = new ArrayList<>();
		for(Operator op: ops){
			if(lex.getType()==Lexeme.OPERATOR && lex.getValue().equals(op.symbol)){
				list.add( op);
			}
		}
		return list.toArray(new Operator[list.size()]);
	}
	
	
	
	private static Operator[] getOperators(){
		return new Operator[] {
				new Operator("+",1,true){
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println(inputs[0] + "+" + inputs[1]);
						return inputs[0] + inputs[1];
					}
				},
				new Operator("-",1,true){
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println(inputs[0] + "-" + inputs[1]);
						return inputs[0] - inputs[1];
					}
				},
				new Operator("*",2,true){
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println(inputs[0] + "*" + inputs[1]);
						return inputs[0] * inputs[1];
					}
				},
				new Operator("/",2,true){
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println(inputs[0] + "/" + inputs[1]);
						return inputs[0] / inputs[1];
					}
				},
				new Operator("%",2,true){
					@Override
					public double eval(double[] inputs){
						return inputs[0] % inputs[1];
					}
				},
				new Operator("!",3,true,false){
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println(inputs[0]+"!");
						double x = inputs[0] +1.0;
						return Math.sqrt(2*Math.PI/x)*Math.pow((x/Math.E), x);
					}
				},
				new Operator("-",3,false,false){//unary -
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println("-"+inputs[0]);
						return -inputs[0];
					}
				},
				new Operator("+",3,false,false){//unary +
					@Override
					public double eval(double[] inputs){
						if(this.opLogs)System.out.println("+"+inputs[0]);
						return inputs[0];
					}
				},
				new Operator("(",0,true,false){
					@Override
					public boolean isAtomicOperator(){
						return false;
					}
					
					public double eval(double[] inputs){
						throw new ExpressionParseException("Expression string '(' cannot eval ", -1);
					}
				},
				new Operator(")",4,true,false){
					@Override
					public boolean isAtomicOperator(){
						return false;
					}
					
					public double eval(double[] inputs){
						throw new ExpressionParseException("Expression string ')' cannot eval ", -1);
						//return inputs[0];
					}
				},
				new Operator(",",0,true,false){
					@Override
					public boolean isAtomicOperator(){
						return false;
					}
					
					public double eval(double[] inputs){
						throw new ExpressionParseException("Expression string ',' cannot eval ", -1);
						//return inputs[0];
					}
				},
				new Operator("log",0,2,true){
					@Override
					public boolean isAtomicOperator(){
						return false;
					}
					
					public double eval(double[] inputs){
						if(this.opLogs)System.out.print("log("+inputs[0]+")/log("+inputs[1]+")");
						return Math.log(inputs[0])/Math.log(inputs[1]);
					}
				},
				new Operator("pow",0,2,true){
					@Override
					public boolean isAtomicOperator(){
						return false;
					}
					
					public double eval(double[] inputs){
						if(this.opLogs)System.out.print("pow("+inputs[0]+"/"+inputs[1]+")");
						return Math.pow(inputs[0], inputs[1]);
					}
				}
		};
		
	}
	@SuppressWarnings("unused")
	private static String formatNumber(String str)
    {
        if(!Pattern.compile("\\-?[0-9]*(\\.[0-9])|\\-?([0-9]+)").matcher(str).matches()){
            return str;
        }
        Double dbl = Double.parseDouble(str);
        double f = dbl.doubleValue();
        final int i=(int) f;
        if(f==i)  return Integer.toString(i);
        return str;
    }
    public static String[] getAllMatches(String input){
    	Pattern p = Pattern.compile("([0-9]*\\.[0-9]+|[0-9]+|[a-zA-Z]+|[^\\w\\s])"); 
    	Matcher m = p.matcher(input);
    	ArrayList<String> matches = new ArrayList<String>();
    	while(m.find()){
    	    matches.add(m.group());
    	}
    	String[] matchArr = new String[matches.size()];
    	return matches.toArray(matchArr);
    }
    
    public static ArrayList<Lexeme> lexify(String s) {
    	@SuppressWarnings("unused")
		int position =0;
    	String[] tokens = getAllMatches(s);
    	for(String token : tokens){
    		System.out.println (token );
    	}
    	
    	ArrayList<Lexeme> tokBuf = new ArrayList<>();
    	//try {
        for (String token : tokens) {
        	position +=token.length();
        	TokenType type = TokenType.UNSET;
        	if(token.matches("^[0-9]+$")){
        		type = TokenType.INT;
        	} else if (token.matches("^[0-9]*\\.[0-9]+$")) {
        		type = TokenType.DECIMAL;
        	} else if (token.matches("^([a-zA-Z]+)$")) {
        		type = TokenType.WORD;
        	} else if (token.matches("^([^\\w\\s])$")) {
        		type = TokenType.OPERATOR;
        	}
            switch(type) {
                case INT: // int
                    tokBuf.add(new Lexeme(Lexeme.NUMBER, token));
                    break;
                case DECIMAL: // double
                    tokBuf.add(new Lexeme(Lexeme.NUMBER, token));
                    break;
                case WORD:
                    tokBuf.add(new Lexeme(Lexeme.WORD,token));
                    break;
                case OPERATOR:  // operator
                	if(token.equals("(")){
                		tokBuf.add(new Lexeme(Lexeme.LPAREN , token)) ;
                	} else if(token.equals(")")){
                		tokBuf.add(new Lexeme(Lexeme.RPAREN, token)) ;
                	} else if(token.equals(",")){
                		tokBuf.add(new Lexeme(Lexeme.COMMA , token)) ;
                	}else {
                		tokBuf.add(new Lexeme(Lexeme.OPERATOR, token)) ;
                	}
                	break;
                    
            }
        }
   /* 	} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			if(tokBuf.isEmpty()){
				throw new ExpressionParseException("Expression string cannot be tokenized", position);
			} else {
				Lexeme last = tokBuf.get(tokBuf.size()-1);
				
				throw new ExpressionParseException("Expression string cannot be tokenized after" +
				last.toString(), position);
			}
		}*/
        return tokBuf;
    }
}
