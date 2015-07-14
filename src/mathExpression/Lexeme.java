package mathExpression;

public class Lexeme {
    public final static int WORD = 1;
    public final static int NUMBER = 2;
    public final static int OPERATOR = 3;
    public final static int LPAREN = 4;
    public final static int RPAREN = 5;
    public final static int COMMA = 6;
    int type;
    String value;
    public Lexeme(int type, String value){
        this.type=type;
        this.value=value;
    }
    
    public String getValue(){
    	return value;
    }
    
    public int getType(){
    	return type;
    }
    
    @Override
    public String toString(){
    	String typeName;
    	switch(type){
    	case WORD:
    		typeName = "WORD";
    		return "<"+typeName+": "+ value+">";
    	case NUMBER:
    		typeName = "NUMBER";
    		return "<"+typeName+": "+ value+">";
    	case OPERATOR:
    		typeName = "OPERATOR";
    		return "<"+typeName+": "+ value+">";
    	case LPAREN:
    		typeName = "LPAREN";
    		return "<"+typeName+">";
    	case RPAREN:
    		typeName = "RPAREN";
    		return "<"+typeName+">";
    	case COMMA:
    		typeName = "COMMA";
    		return "<"+typeName+">";
    	default:
    		typeName = "ERROR";
    		return "<"+typeName+": "+ value+">";
    	}
    }

}
