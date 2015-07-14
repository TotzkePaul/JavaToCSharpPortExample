

import mathExpression.*;


public class MathDriver {

	public static void main(String[] args) {
		String s = "3+3+-4+4*pow(-3,3)*pow(pi,3)+4+7";
		Expression x = ExpressionTree.parse(s);
		VarMap vm = new VarMap(false );
		vm.setValue("pi", Math.PI);
		vm.setValue("r", 5);

		FuncMap fm = null; // no functions in expression
		//x.print("", true);
		
		System.out.println(x); // (pi*(r^2))
		System.out.println(x.eval(vm, fm)); // 78.53981633974483
		System.out.println("---");
		x.print();
		System.out.println("---");
		
		
		Expression y = x.deepCopy();
		System.out.print(y.eval(vm, fm) +": ");
		System.out.println(y);
		while( (y = y.reduceExpression(vm, fm)) != null){
			System.out.print(y.eval(vm, fm) +": ");
			System.out.println(y);
		}
		//y = x.reduceExpression(vm, fm);
		//System.out.println(y); // (pi*(r^2))
		//ystem.out.println(y.eval(vm, fm)); // 78.53981633974483
		
	}
	
	public static void visualExpression(Expression x){
		
	}

}
