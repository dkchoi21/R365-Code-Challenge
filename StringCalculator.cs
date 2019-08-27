public class StringCalculator
{
	public static string add(string input)
	{
		return calculate(input, "+", (x, y) => x + y);
	}

	public static string subtract(string input)
	{
		return calculate(input, "-", (x, y) => x - y);
	}

	public static string divide(string input)
	{
		return calculate(input, "/", (x, y) => x / y);
	}

	public static string multiply(string input)
	{
		return calculate(input, "*", (x, y) => x * y);
	}

	private static string calculate(string input, string optr, Func<double, double, double> lambda)
	{
		//separates delimiter(s) from number
		Tuple<string[], string> tuple = separateDelimiterFromNumber(input);
		string[] delimiter = tuple.Item1;
		string numbers = tuple.Item2;
		//separates string of numbers using given delimiter
		List<string> parsedNumbers = parseUsingDelimiter(numbers, delimiter);
		double result = -1;
		bool isFirstItem = true;
		foreach (string stringNumber in parsedNumbers)
		{
			double number = convertToNumber(stringNumber);
			//result is set is the first number; lambda function not used yet
			if (isFirstItem)
			{
				result = number;
				isFirstItem = false;
				continue;
			}

			result = lambda(result, number);
		}

		return displayFormula(optr, parsedNumbers, result);
	}


	private static Tuple<string[], string> separateDelimiterFromNumber(string input)
	{
		string[] defaultDelimiter = new string[]{"\n", ","};
		string numbers = input;
		//check if the input contains a custom delimiter
		if (input.Length > 1 && input.Substring(0, 2) == "//")
		{
			//split on "\n" to separate delimiters & number --> [[{delimiter1}]...[{delimiterN}]], {numbers}]
			List<string> inputArray = new List<string>(input.Substring(2).Split(new[]{'\n'}, 2));
			//split on on brackets to separate delimiters --> [{delimiter1}, {delimiter2},...{delimiterN}]
			string[] customDelimiter = inputArray[0].Split(new string[]{"[", "]"}, StringSplitOptions.RemoveEmptyEntries);
			//combine default delimiters & custom delimiters
			List<string> list = new List<string>();
			list.AddRange(customDelimiter);
			list.AddRange(defaultDelimiter);
			string[] delimiter = list.ToArray();
			
			if (inputArray.Count == 1) {
				numbers = "";
			}
			else
			{
				numbers = inputArray[1];
			}
			return new Tuple<string[], string>(delimiter, numbers);
		}

		return new Tuple<string[], string>(defaultDelimiter, numbers);
	}


	private static List<string> parseUsingDelimiter(string input, string[] delimiters)
	{
		List<string> parsedString = new List<string>(input.Split(delimiters, StringSplitOptions.None));
		//traverses through the string of items separated by delimiter and replaces all invalid ones
		for (int i = 0; i < parsedString.Count; i++)
		{
			string item = parsedString[i];
			//if item is not a number or if it is a number greater than 1000, then replace with "0"
			if (!isNumber(item) || convertToNumber(item) > 1000)
			{
				parsedString[i] = "0";
			}
		}

		return parsedString;
	}

	private static double convertToNumber(string item)
	{
		double convertedNumber = Convert.ToDouble(item);
		//exception thrown for negative integers
		if (convertedNumber < 0)
		{
			throw new Exception("Must be a non-negative integer");
		}

		return convertedNumber;
	}

	private static bool isNumber(string item)
	{
		int n; double m;
		return int.TryParse(item, out n) || double.TryParse(item, out m);
	}

	private static string displayFormula(string optr, List<String> stringNumbers, double result)
	{
		string formula = "";
		for (int i = 0; i < stringNumbers.Count; i++)
		{
			string operand = stringNumbers[i];
			if (i == stringNumbers.Count - 1)
			{
				formula += operand;
			}
			else
			{
				formula += String.Format("{0} {1} ", operand, optr);;
			}
		}
		formula += String.Format(" = {0}", result);
		return formula;
	}

	//Used for unit-testing
	private static void AssertEqual(string actual_formula, string expected_formula)
	{
		if (expected_formula != actual_formula)
		{
			string msg = String.Format("The actual formula: [{0}] is not equivalent to  expected formula: [{1}]", actual_formula, expected_formula);
			throw new Exception(msg);
		}
	}

	//Unit Tests
	private static void testBasicAddition()
	{
		AssertEqual(add(""), "0 = 0");
		AssertEqual(add("//"), "0 = 0");
		AssertEqual(add("2"), "2 = 2");
		AssertEqual(add("2,2"), "2 + 2 = 4");
		AssertEqual(add("1\n2,2"), "1 + 2 + 2 = 5");
		AssertEqual(add("5,tytyt"), "5 + 0 = 5");
		AssertEqual(add("5,1001"), "5 + 0 = 5");
		AssertEqual(add("2,4,rrrr,1001,6"), "2 + 4 + 0 + 0 + 6 = 12");
	}
	
	private static void testAdvanceAddition()
	{
		//Support 1 custom delimiter of one character length
		AssertEqual(add("//;\n2;5"), "2 + 5 = 7");
		
		//Support 1 custom delimiter of any length
		AssertEqual(add("//[***]\n11***22***33"), "11 + 22 + 33 = 66");
		
		//Support multiple delimiters of any length
		AssertEqual(add("//[*][!!][rrr]\n11rrr22*33!!44"), "11 + 22 + 33 + 44 = 110");
		
		//Using "//" as a custom delimiter
		AssertEqual(add("//[//]\n11//22//33"), "11 + 22 + 33 = 66");

		//Using "]" as a custom delimiter
		AssertEqual(add("//[]]\n11]22]33"), "11 + 22 + 33 = 66");

		//Using "-" as a custom delimiter
		AssertEqual(add("//[-]\n11-22-33"), "11 + 22 + 33 = 66");
	}


	public static void Main()
	{
		testBasicAddition();
		testAdvanceAddition();

	}

}