package mathExpression;

public interface Function {
	
/*	public PowFunction() {}

	*//**
	Returns the value at index location 0 to the exponent of the value
	at index location 1.
	*//*
	public double of(double[] d, int numParam) {
		return java.lang.Math.pow(d[0], d[1]);
	}

	*//**
	Returns true only for 2 parameters, false otherwise.
	*//*
	public boolean acceptNumParam(int numParam) {
		return numParam == 2;
	}

	public String toString() {
		return "pow(x, y)";
	}*/

	/**
	Takes the specified double array as input and returns a double value.  Functions
	that accept a variable number of inputs can take numParam to be the number of inputs.
	*/
	public double of(double[] param, int numParam);

	/**
	Returns true if the numParam is an accurate representation of the number of inputs
	the function processes.
	*/
	public boolean acceptNumParam(int numParam);

}