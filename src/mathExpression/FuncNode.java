package mathExpression;

public class FuncNode extends Expression {

	protected Expression[] children = null;
	protected Operator operator; 
	protected String name; 
	
	public FuncNode(String name, Expression[] children, Operator oper){
		checkBeforeAccept(children);
		this.children = children;
		this.operator = oper;
		this.name = name;
		
	}

	@Override
	public double eval(VarMap v, FuncMap f) {
		double[] values = new double[children.length];
		//System.out.print("func children "+children.length);
		for(int i=0; i<values.length;i++){
			values[i] = children[i].eval(v, f);
		}
		
		return operator.eval(values);
	}
	
	public String toString(){
		String myName=name + "(";
		String sep = "";
		for(Expression child : children){
			myName +=sep;
			sep = ",";
			
			myName +=child.toString();
		}
		myName +=")";
		return myName;
	}

	@Override
	public Expression[] getChildren() {
		return children;
	}

	@Override
	public boolean isLeaf() {
		// TODO Auto-generated method stub
		for(int i=0; i<children.length; i++){
			if(!children[i].isLeaf()){
				return false;
			}
		}
		return true;
	}

	@Override
	public int getPrecedence() {
		// TODO Auto-generated method stub
		return Integer.MAX_VALUE;
	}

	@Override
	public Expression deepCopy() {
		// TODO Auto-generated method stub
		Expression[] dcChildren = new Expression[children.length];
		for(int i =0; i<dcChildren.length; i++){
			dcChildren[i] = children[i].deepCopy();
		}
		return new FuncNode(name, dcChildren, operator);
	}
}
