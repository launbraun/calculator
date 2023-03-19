using MathLib;
using System.Windows.Controls;

namespace wpf_calculator;

public static class ButtonClickHandler
{
    private static string ConvertSender(string btnKeyword)
    {
        return btnKeyword switch
        {
            "BtnOne" => "1",
            "BtnTwo" => "2",
            "BtnThree" => "3",
            "BtnFour" => "4",
            "BtnFive" => "5",
            "BtnSix" => "6",
            "BtnSeven" => "7",
            "BtnEight" => "8",
            "BtnNine" => "9",
            "BtnZero" => "0",

            "BtnEqual" => MathHelper.Equal,
            "BtnPlus" => MathHelper.Plus,
            "BtnMinus" => MathHelper.Minus,
            "BtnMult" => MathHelper.Mult,
            "BtnDivide" => MathHelper.Div,
            "BtnDot" => MathHelper.Dot,
            "BtnSqrt" => MathHelper.Root,
            "BtnLogComma" => MathHelper.Comma,
            "Pow" => MathHelper.Pow,

            "Remove" => "r",
            "Clear" => "c",
            _ => string.Empty
        };
    }

    public static string HandleSender(object sender)
    {
        var senderName = ((Button)sender).Name;
        return ConvertSender(senderName);
    }
}