public class StringCalculator
{
	public static string add(string input)
	{
		return calculate(input, "+", (x, y) => x + y);
	}

	private static double calculate(string input, string optr, Func<double, double, double> lambda)
	{

		string[] delimiter = new string[]{","};

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
			//if item is not a number, then replace with "0"
			if (!isNumber(item))
			{
				parsedString[i] = "0";
			}
		}
		return parsedString;
	}

	private static double convertToNumber(string item)
	{
		double convertedNumber = Convert.ToDouble(item);
		return convertedNumber;
	}

	private static bool isNumber(string item)
	{
		int n; double m;
		return int.TryParse(item, out n) || double.TryParse(item, out m);
	}


	public static void Main()
	{
		Console.WriteLine(add("5000"));
		Console.WriteLine(add("1,20"));
		Console.WriteLine(add(""));
		Console.WriteLine(add("5,tytyt"));

	}

}