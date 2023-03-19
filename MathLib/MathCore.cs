using System.Globalization;
using System.Text.RegularExpressions;
using org.mariuszgromada.math.mxparser;

namespace MathLib;

public class MathCore
{
    public string MathString { get; set; }

    public MathCore()
    {
        MathString = "0";
    }

    public static string MakeExpression(string mathString)
    {
        while (!mathString.IsNumber())
        {
            if (mathString == string.Empty) return "0";
            mathString = mathString.Remove(mathString.Length - 1);
        }

        return PlaceBrackets(mathString);
    }

    private static string ReplaceActionsToEvaluate(string mathString) => 
        mathString.Replace(MathHelper.Mult, "*").Replace(MathHelper.Div, "/")
                  .Replace(MathHelper.Root, "sqrt(");

    public string EvaluateExpression()
    {
        double resultDouble;
        var mathString = ReplaceActionsToEvaluate(MakeExpression(MathString));
        try
        {
            resultDouble = new Expression(mathString).calculate();
        }
        catch (DivideByZeroException)
        {
            resultDouble = double.NaN;
        }
        catch (OverflowException)
        {
            resultDouble = double.PositiveInfinity;
        }

        string resultString;
        if (!double.IsNaN(resultDouble))
        {
            resultString = resultDouble.ToString(CultureInfo.CurrentCulture)
                .Replace(MathHelper.Comma, MathHelper.Dot);
        }
        else resultString = "Error";

        return resultString;
    }

    private static int GetBracketsQuantity(string mathString)
    {
        var mathLength = mathString.Length;
        var openBracketsQuantity = mathLength - mathString.Replace("(", string.Empty).Length; 
        var closeBracketsQuantity = mathLength - mathString.Replace(")", string.Empty).Length; 

        return openBracketsQuantity - closeBracketsQuantity;
    }

    private static string PlaceBrackets(string mathString)
    {
        var brAmt = GetBracketsQuantity(mathString);
        return mathString + string.Concat(Enumerable.Repeat(")", brAmt));
    }

    private void AddString(string s) => MathString += s;

    private void RemoveString(int amt) => MathString = MathString.Remove(MathString.Length - amt);

    public void NumberClick(string num)
    {
        if (MathString == "0") RemoveString(1);
        if (MathString.IsMultNeed())
            AddString(MathHelper.Mult);
        AddString(num);
    }

    public void ActionClick(string act)
    {
        if (MathString.IsMatchToRe(new Regex(@"[\(÷×\^]$")) && act == MathHelper.Minus)
        {
            AddString(act);
            return;
        }

        if (MathString.IsMatchToRe(new Regex(@"(\(\-|\^\-|×\-|÷\-)$")) && act == MathHelper.Plus)
        {
            RemoveString(1);
            return;
        }

        if (MathString.IsMatchToRe(new Regex(@"(\(\-|\^\-|×\-|÷\-)$"))) return;

        switch (MathString)
        {
            case "0" when act == MathHelper.Minus:
                MathString = MathHelper.Minus;
                break;
            case MathHelper.Minus when act == MathHelper.Plus || act == MathHelper.Mult || act == MathHelper.Div:
                return;
        }

        if (MathString.EndsWith(MathHelper.Comma) || MathString.EndsWith("(")) return;
        if (MathString.CheckForAction() || MathString.EndsWith(MathHelper.Dot)) RemoveString(1);
        AddString(act);
    }

    public void DotClick()
    {
        if (MathString.EndsWith(MathHelper.Dot)) return;
        if (MathString.IsMatchToRe(new Regex(@"([^0-9])$")))
        {
            NumberClick("0");
        }
        else if (MathString.IsMatchToRe(new Regex(@"\.[0-9]*$"))) return;
        AddString(MathHelper.Dot);
    }

    public void EqualClick()
    {
        MathString = EvaluateExpression();
    }

    public void PowClick()
    {
        if (MathString.CheckForAction() ||
            MathString.EndsWith("(") ||
            MathString.EndsWith(MathHelper.Pow)) return;
        if (MathString.CheckForAction() ||
            MathString.EndsWith(MathHelper.Dot)) RemoveString(1);
        AddString(MathHelper.Pow);
    }

    public void SqrtClick()
    {
        if (MathString == "0" || MathString.EndsWith(MathHelper.Dot)) RemoveString(1);
        if (MathString.IsNumber() && MathString.Length != 0 || MathString.IsMultNeed()) AddString(MathHelper.Mult);
        AddString(MathHelper.Root);
    }

    public void SpecClick(string spec)
    {
        if ((MathString.IsNumber() || MathString.EndsWith(",")) &&
            !MathString.EndsWith(spec)) AddString(spec);
    }

    public void RemoveClick()
    {
        RemoveString(MathString.RemoveQuantityCheck());
        MathString = MathString == string.Empty ? "0" : MathString;
    }

    public void Clear() => MathString = "0";
}