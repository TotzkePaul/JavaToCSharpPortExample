package mathExpression;

public class FuncMap {

	private String[] name = new String[50];
	private Function[] func = new Function[50];
	private int numFunc = 0;
	private boolean caseSensitive = false;

	public FuncMap() {}

	public FuncMap(boolean caseSensitive) {
		this.caseSensitive = caseSensitive;
	}

	/**
	Adds the mappings for many common functions.  The names are specified in all lowercase letters.
	*/
	public void loadDefaultFunctions() {

	}

	/**
	Returns a function based on the name and the specified number of parameters.

	@throws RuntimeException If no supporting function can be found.
	*/
	public Function getFunction(String funcName, int numParam) {
		for (int i = 0; i < numFunc; i++) {
			if (func[i].acceptNumParam(numParam) && (caseSensitive && name[i].equals(funcName) || !caseSensitive && name[i].equalsIgnoreCase(funcName)))
				return func[i];
		}

		throw new RuntimeException("function not found: " + funcName + " " + numParam);
	}

	/**
	Assigns the name to map to the specified function.

	@throws IllegalArgumentException If any of the parameters are null.
	*/
	public void setFunction(String funcName, Function f) {
		if (funcName == null)
			throw new IllegalArgumentException("function name cannot be null");

		if (f == null)
			throw new IllegalArgumentException("function cannot be null");

		for (int i = 0; i < numFunc; i++) {
			if (caseSensitive && name[i].equals(funcName) || !caseSensitive && name[i].equalsIgnoreCase(funcName)) {
				func[i] = f;
				return;
			}
		}

		if (numFunc == name.length) {
			String[] tmp1 = new String[2 * numFunc];
			Function[] tmp2 = new Function[tmp1.length];

			for (int i = 0; i < numFunc; i++) {
				tmp1[i] = name[i];
				tmp2[i] = func[i];
			}

			name = tmp1;
			func = tmp2;
		}

		name[numFunc] = funcName;
		func[numFunc] = f;
		numFunc++;
	}

	/**
	Returns true if the case of the function names is considered.
	*/
	public boolean isCaseSensitive() {
		return caseSensitive;
	}

	/**
	Returns an array of exact length of the function names stored in this map.
	*/
	public String[] getFunctionNames() {
		String[] arr = new String[numFunc];

		for (int i = 0; i < arr.length; i++)
			arr[i] = name[i];

		return arr;
	}

	/**
	Returns an array of exact length of the functions stored in this map.  The returned
	array corresponds to the order of the names returned by getFunctionNames.
	*/
	public Function[] getFunctions() {
		Function[] arr = new Function[numFunc];

		for (int i = 0; i < arr.length; i++)
			arr[i] = func[i];

		return arr;
	}

	/**
	Removes the function-name and the associated function from the map.  Does nothing if the function-name
	is not found.
	*/
	public void remove(String funcName) {
		for (int i = 0; i < numFunc; i++) {
			if (caseSensitive && name[i].equals(funcName) || !caseSensitive && name[i].equalsIgnoreCase(funcName)) {
				for (int j = i + 1; j < numFunc; j++) {
					name[j - 1] = name[j];
					func[j - 1] = func[j];
				}
				numFunc--;
				name[numFunc] = null;
				func[numFunc] = null;
				break;
			}
		}
	}
}