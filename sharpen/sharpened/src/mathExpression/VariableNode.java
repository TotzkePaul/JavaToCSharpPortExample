package mathExpression;

public class VariableNode extends Expression {

	protected double val = 0.0;
	protected String name;

	public VariableNode(String  name) {
		this.name = name;
	}

	/**
	Returns the value.
	*/
	public double eval(VarMap v, FuncMap f) {
		return v.getValue(name);
	}

	@Override
	public Expression[] getChildren() {
		// TODO Auto-generated method stub
		return new Expression[0];
	}
	public String toString(){
		return name;
	}

	@Override
	public boolean isLeaf() {
		// TODO Auto-generated method stub
		return true;
	}

	@Override
	public int getPrecedence() {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public Expression deepCopy() {
		// TODO Auto-generated method stub
		return new VariableNode( name) ;
	}
}
