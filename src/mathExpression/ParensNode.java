package mathExpression;

public class ParensNode extends Expression {

	protected Expression[] children = null;
	
	public ParensNode(Expression[] children){
		checkBeforeAccept(children);
		this.children = children;
		
	}

	@Override
	public double eval(VarMap v, FuncMap f) {
		double value = children[0].eval(v, f);
		//System.out.print("("+value+")");
		return value; //children[0].eval(v, f);
	}

	@Override
	public Expression[] getChildren() {
		// TODO Auto-generated method stub
		return children;
	}
	public String toString(){
		return "("+children[0].toString()+")";
	}

	@Override
	public boolean isLeaf() {
		// TODO Auto-generated method stub
		if(children!= null && children.length==1 && children[0] instanceof NumberNode){
			return true;
		}
		return false;
	}

	@Override
	public int getPrecedence() {
		// TODO Auto-generated method stub
		return Integer.MAX_VALUE;
	}

	@Override
	public Expression deepCopy() {
		Expression[] dcChildren = new Expression[children.length];
		for(int i =0; i<dcChildren.length; i++){
			dcChildren[i] = children[i].deepCopy();
		}
		return new ParensNode( dcChildren);
	}
}
