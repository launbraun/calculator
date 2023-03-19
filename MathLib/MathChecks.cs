using System.Text.RegularExpressions;

namespace MathLib;

public static class MathChecks
{
    public static bool CheckForAction(this string s) =>
        s.EndsWith(MathHelper.Plus) ||
        s.EndsWith(MathHelper.Minus) ||
        s.EndsWith(MathHelper.Mult) ||
        s.EndsWith(MathHelper.Div) ||
        s.EndsWith(MathHelper.Pow);

    public static int RemoveQuantityCheck(this string s)
    {
        if (s.EndsWith(MathHelper.Root) ||
            s.Length == 2 && s[0] == '-')
            return 2;

        return 1;
    }

    public static bool IsMultNeed(this string s) => s.EndsWith(")");

    public static bool IsNumber(this string s)
    {
        return s != string.Empty && int.TryParse(s[s.Length - 1].ToString(), out _);
    }

    public static bool IsMatchToRe(this string expression, Regex re)
    {
        return re.IsMatch(expression);
    }
}