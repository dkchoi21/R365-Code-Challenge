public class StringCalculator
{
	public static string add(string input)
	{
		return calculate(input, "+", (x, y) => x + y);
	}

	private static double calculate(string input, string optr, Func<double, double, double> lambda)
	{

		string[] delimiter = new string[]{"\n", ","};

		//separates string of numbers using given delimiter
		List<string> parsedNumbers = parseUsingDelimiter(input, delimiter);

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

		return result;
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


	public static void Main()
	{
		//step 1 unit tests
		Console.WriteLine(add("5000"));
		Console.WriteLine(add("1,20"));
		Console.WriteLine(add(""));
		Console.WriteLine(add("5,tytyt"));

		//step 2 unit tests
		Console.WriteLine(add("5,3,4"));
		Console.WriteLine(add("5,3,4,5"));

		//step 3 unit tests
		Console.WriteLine(add("1\n2,3"));
		Console.WriteLine(add("1\n2\n3"));

		//step 3 unit tests
		Console.WriteLine(add("-12,3"));
		Console.WriteLine(add("-1\n2\n3"));
	}

}