package mathExpression;

public abstract class Operator {
	boolean leftassociative, binary,isFunc;
	boolean opLogs = false;
	int precedence, numOfOperands;
	String symbol;
	
	public Operator(String symbol, int precedence, boolean leftassociative){
		this(symbol,  precedence, leftassociative,true);
	/*	this.symbol = symbol;
		this.precedence = precedence;
		this.leftassociative = leftassociative;
		this.binary = true;*/
	}
	
	public Operator(String symbol, int precedence, boolean leftassociative, boolean binary){
		this.symbol = symbol;
		this.precedence = precedence;
		this.leftassociative = leftassociative;
		this.binary = binary;
		if(binary){
			numOfOperands =2;
		} else {
			numOfOperands =1;
		}
		this.isFunc=false;
	}
	public Operator(String symbol, int precedence, int numOfOperands, boolean isFunc){
		this.symbol = symbol;
		this.precedence = precedence;
		this.leftassociative = true;
		this.binary = false;
		this.numOfOperands = numOfOperands;
		this.isFunc = isFunc;
	}
	public abstract double eval(double[] inputs);
	
	public boolean isAtomicOperator(){
		return true;
	}
	
	public int getOperandsSize(){
		return numOfOperands;
	}
	
	public String getSymbol(){
		return symbol;
	}
	
	public int getPrecednce(){
		return precedence;
	}
	
	public boolean isLeftAssociative(){
		return leftassociative;
	}
	public boolean isFunc(){
		return isFunc;
	}

	public boolean isBinary(){
		return binary;
	}
	
	public String toString(){
		String assoc = (leftassociative) ? "Left" : "Right";  
		int  operands = (binary) ? 2 : 1; 
		
		return "[\""+symbol+"\": precedence:"+precedence+ " assoc:" +assoc + " operands:" +operands+"]";
	}
}
