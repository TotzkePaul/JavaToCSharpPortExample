package mathExpression;

public abstract class Expression {
	
	/* types:
	 * 		Atomic
	 * 		|	Number
	 * 		|	Parens
	 * 		| 	Function
	 * 		|	Variables
	 * 		Operator {assoc, precdence} 
	 * 		|	Uniary {+,-,!}
	 * 		|	Binary
	 * 
	 */

	protected Expression parent = null;

	/**
	Returns the result of evaluating the expression tree rooted at this node.
	*/
	public abstract double eval(VarMap v, FuncMap f);
	public abstract Expression[] getChildren();
	public abstract int getPrecedence();
	
	public abstract Expression deepCopy();
	//public abstract String getName();
	public abstract boolean isLeaf();
	
	
	public Expression reduceExpression(VarMap v, FuncMap f){
		if(this.isLeaf()){
			return null;
		}
		Expression newExpr = this.deepCopy();
		Expression reductase = deepestReducableNode(newExpr);
		Expression replacement = new NumberNode( reductase.eval(v, f));
		//System.out.println("copy: " +newExpr);
		//System.out.println("rem " +reductase);
		//System.out.println("add "+ replacement);
		if(newExpr==reductase){
			//System.out.println(" copy == high p");
			return replacement;
		} else {
			//System.out.println("replacement");
			replaceNodeInTree(newExpr, reductase, replacement);
			return newExpr;
		}
	}
	
	private void replaceNodeInTree(Expression tree, final Expression rem, final Expression add){
		
		Expression[] treeChildren = tree.getChildren();
		for(int i =0; i<treeChildren.length; i++){
			if(treeChildren[i] ==rem){
				treeChildren[i] = add;
			} else {
				replaceNodeInTree(treeChildren[i], rem, add);
			}
		}		
	}
	
	
	private Expression deepestReducableNode(Expression node){
		if(node.isLeaf()){
			return node;
		} else {
			Expression[] myChildren = node.getChildren();
			Expression firstHighestPrec = null;
			boolean allLeafNode = true;
			for(int i =0; i<myChildren.length; i++){
				if(!myChildren[i].isLeaf()){
					allLeafNode = false;
				}
				Expression current = deepestReducableNode(myChildren[i]);
				if(firstHighestPrec == null || firstHighestPrec.getPrecedence() < myChildren[i].getPrecedence()){
					firstHighestPrec = current;
				}
			}
			if(firstHighestPrec==null || allLeafNode){
				return node;
			} else {
				return firstHighestPrec;
			}
		}
		
		//return null;
	}

	/**
	Returns true if this node is a descendent of the specified node, false otherwise.  By this
	methods definition, a node is a descendent of itself.
	*/
	public boolean isDescendent(Expression x) {
		Expression y = this;

		while (y != null) {
			if (y == x)
				return true;
			y = y.parent;
		}

		return false;
	}

	/**
	Returns the parent of this node.
	*/
	public Expression getParent() {
		return parent;
	}
	
	public void print() {
		print("", false);
	}

   public void print(String prefix, boolean isTail) {
	   Expression[] children = getChildren();
        System.out.println(prefix + (isTail ? "└── " : "├── ") + toString());
        for (int i = 0; i < children.length - 1; i++) {
            children[i].print(prefix + (isTail ? "    " : "│   "), false);
        }
        if (children.length > 0) {
            children[(children.length - 1)].print(prefix + (isTail ?"    " : "│   "), true);
        }
    }

	/**
	Protected method used to verify that the specified expression can be included as a child
	expression of this node.

	@throws IllegalArgumentException If the specified expression is not accepted.
	*/
	protected void checkBeforeAccept(Expression[] x) {
		if (x == null)
			throw new IllegalArgumentException("expression cannot be null");
		for(Expression child : x){
			if (child.parent != null)
				throw new IllegalArgumentException("expression must be removed parent");
	
			if (isDescendent(child))
				throw new IllegalArgumentException("cyclic reference");
		}
	}

	

	/**
	Returns a string that represents the expression tree rooted at this node.
	*/
	public String toString() {
		StringBuffer sb = new StringBuffer();
		toString(this, sb);
		return sb.toString();
	}

	private static void toString(Expression x, StringBuffer sb) {
		return;
	}
}
