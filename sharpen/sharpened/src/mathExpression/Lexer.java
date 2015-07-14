package mathExpression;

import java.io.IOException;
import java.io.StreamTokenizer;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Lexer {
	
    public static ArrayList<Lexeme> tokenize(String s) throws IOException {
    	
        StreamTokenizer tokenizer = new StreamTokenizer(new StringReader(s));
        tokenizer.ordinaryChar('-');  // Don't parse minus as part of numbers.
        ArrayList<Lexeme> tokBuf = new ArrayList<>();
        while (tokenizer.nextToken() != StreamTokenizer.TT_EOF) {
            switch(tokenizer.ttype) {
                case StreamTokenizer.TT_NUMBER:
                    tokBuf.add(new Lexeme(Lexeme.NUMBER,String.valueOf(tokenizer.sval)));
                    break;
                case StreamTokenizer.TT_WORD:
                    tokBuf.add(new Lexeme(Lexeme.WORD,tokenizer.sval));
                    break;
                default:  // operator
                	if(String.valueOf((char) tokenizer.ttype).equals("(")){
                		tokBuf.add(new Lexeme(Lexeme.LPAREN , String.valueOf((char) tokenizer.ttype))) ;
                	} else if(String.valueOf((char) tokenizer.ttype).equals(")")){
                		tokBuf.add(new Lexeme(Lexeme.RPAREN, String.valueOf((char) tokenizer.ttype))) ;
                	} else if(String.valueOf((char) tokenizer.ttype).equals(",")){
                		tokBuf.add(new Lexeme(Lexeme.COMMA , String.valueOf((char) tokenizer.ttype))) ;
                	}else {
                		tokBuf.add(new Lexeme(Lexeme.OPERATOR, String.valueOf((char) tokenizer.ttype))) ;
                	}
                	break;
                    
            }
        }
        return tokBuf;
    }
    
    enum TokenType {
        WORD, INT, DECIMAL, OPERATOR, UNSET
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
    	//Pattern pat = Pattern.compile("0-9]*\\.[0-9]+|[0-9]+|[A-z]+|[\\W\\S]");
        //Matcher m = pat.matcher(s);
    	@SuppressWarnings("unused")
		int position =0;
    	String[] tokens = getAllMatches(s);
    	//String[] tokens = s.split("([0-9]*\\.[0-9]+|[0-9]+|[A-z]+|[\\W\\S])");
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
