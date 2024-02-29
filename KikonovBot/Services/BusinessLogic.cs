using System.Text.RegularExpressions;

namespace KikonovBot.Services;

internal class BusinessLogic : ILogic
{
    int ILogic.Counting(string input)
    {
        return input.Length;
    }

    int ILogic.Sum(string input)
    {
        int result = 0;

        string pattern = @"\D";
        string target = " ";
        char[] separators = new char[] { ' ' };
        Regex regex = new Regex(pattern);
        var splitedInput = regex.Replace(input, target).Split(separators, StringSplitOptions.RemoveEmptyEntries);

        foreach(string s in splitedInput)
        {
            result += int.Parse(s);
        }

        return result;
    }
}
