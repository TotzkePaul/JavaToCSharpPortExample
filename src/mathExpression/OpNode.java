package mathExpression;

public class OpNode extends Expression {

	protected Expression[] children = null;
	protected Operator operator; 

	public OpNode(Expression[] children, Operator oper) {
		this.operator = oper;
		this.children = children;
	}
	
	@Override
	public double eval(VarMap v, FuncMap f) {
		double[] values = new double[children.length];
		for(int i=0; i<values.length;i++){
			values[i] = children[i].eval(v, f);
		}
		return operator.eval(values);
	}

	@Override
	public Expression[] getChildren() {
		return children;
	}
	
	public String toString(){
		String mySymbol = operator.getSymbol();
		if(children.length==1){
			if(operator.isLeftAssociative()){
				return children[0].toString() + mySymbol;
			} else {
				return mySymbol + children[0].toString();
			}
		}if(children.length==2){
			return children[0].toString() + mySymbol + children[1].toString();
			
		} else {
			return "ERROR";
		}
	}

	@Override
	public boolean isLeaf() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public int getPrecedence() {
		// TODO Auto-generated method stub
		return operator.getPrecednce();
	}

	@Override
	public Expression deepCopy() {
		// TODO Auto-generated method stub
		Expression[] dcChildren = new Expression[children.length];
		for(int i =0; i<dcChildren.length; i++){
			dcChildren[i] = children[i].deepCopy();
		}
		return new OpNode(dcChildren, operator);
	}

}
