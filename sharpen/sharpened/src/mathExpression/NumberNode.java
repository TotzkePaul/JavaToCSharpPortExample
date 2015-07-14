package mathExpression;

public class NumberNode extends Expression {


		protected double val = 0.0;

		public NumberNode(double d) {
			val = d;
		}
		
		public NumberNode(String s) {
			val = Double.parseDouble(s);
		}

		/**
		Returns the value.
		*/
		public double eval(VarMap v, FuncMap f) {
			return val;
		}

		public double getValue() {
			return val;
		}

		public void setValue(double d) {
			val = d;
		}
		
		

		@Override
		public Expression[] getChildren() {
			// TODO Auto-generated method stub
			return new Expression[0];
		}
		public String toString(){
			
			String s = String.format("%.3f", val);
			String temp = s.indexOf(".") < 0 ? s : s.replaceAll("0*$", "").replaceAll("\\.$", "");
			temp = temp.indexOf(".") < 0 ? temp +".0" : temp;
			return temp; //"" +val;
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
			return new NumberNode(val);
		}
	}
