/*
 * Converted from Java
 */

using System;
using MathExpression;
using Sharpen;

public class MathDriver
{
	public static void Main(string[] args)
	{
		string s = "3+3+-4+4*pow(-3,3)*pow(pi,3)+4+7";
		Expression x = ExpressionTree.Parse(s);
		VarMap vm = new VarMap(false);
		vm.SetValue("pi", Math.PI);
		vm.SetValue("r", 5);
		FuncMap fm = null;
		// no functions in expression
		//x.print("", true);
		System.Console.Out.WriteLine(x);
		// (pi*(r^2))
		System.Console.Out.WriteLine(x.Eval(vm, fm));
		// 78.53981633974483
		System.Console.Out.WriteLine("---");
		x.Print();
		System.Console.Out.WriteLine("---");
		Expression y = x.DeepCopy();
		System.Console.Out.Write(y.Eval(vm, fm) + ": ");
		System.Console.Out.WriteLine(y);
		while ((y = y.ReduceExpression(vm, fm)) != null)
		{
			System.Console.Out.Write(y.Eval(vm, fm) + ": ");
			System.Console.Out.WriteLine(y);
		}
	}

	//y = x.reduceExpression(vm, fm);
	//System.out.println(y); // (pi*(r^2))
	//ystem.out.println(y.eval(vm, fm)); // 78.53981633974483
	public static void VisualExpression(Expression x)
	{
	}
}
