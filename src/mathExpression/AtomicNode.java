package mathExpression;

public class AtomicNode  {
	protected Expression child = null;
	protected Operator operator; 
	protected String name; 
	
	public AtomicNode(String name, Expression child, Operator oper){
		//setChild(child);
		this.operator = oper;
		this.name = name;
		
	}

	
	public double eval(VarMap v, FuncMap f) {
		double a = child.eval(v, f);
		return operator.eval(new double[]{a});
	}

	
	public Expression[] getChildren() {
		// TODO Auto-generated method stub
		return null;
	}

	
	public boolean isLeaf() {
		// TODO Auto-generated method stub
		return false;
	}

}
