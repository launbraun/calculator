using MathLib;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using wpf_calculator.History.Db;
using wpf_calculator.Models.Entity;

namespace wpf_calculator;

public partial class MainWindow : Window
{
    private readonly ApplicationContext _db = new();
    private readonly MathCore _calc = new();

    private string ExpressionText
    {
        get => MathTb.Text;
        set => MathTb.Text = value;
    }

    private string ResultText
    {
        get => ResultTb.Text;
        set => ResultTb.Text = "=" + value;
    }

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            Loaded += InitializeDb;
            WriteExpression();
        }
        catch (Exception ex)
        {
            AddAppLog(ex.Message, "initialize_main_window");
        }
    }

    private class HistoryItem
    {
        public string Expression { get; set; }
        public string Result { get; set; }
    }

    private void InitializeDb(object sender, RoutedEventArgs e)
    {
        _db.Database.EnsureCreated();

        LoadHistoryLog();
    }

    private void WriteExpression()
    {
        ExpressionText = _calc.MathString;

        try
        {
            WriteToResult();
        }
        catch (Exception ex)
        {
            AddAppLog(ex.Message, $"write_result");
        }
    }

    private void WriteToHistory()
    {
        HistoryList.Items.Insert(0, new HistoryItem() { Expression = ExpressionText, Result = ResultText });

        AddHistoryLog(ExpressionText, ResultText);
    }

    private void WriteToResult()
    {
        var result = _calc.EvaluateExpression();

        TextBoxes.RowDefinitions[1].Height = result == MathCore.MakeExpression(ExpressionText) ||
                                             result == string.Empty
            ? new GridLength(0.0, GridUnitType.Star)
            : new GridLength(30.0, GridUnitType.Star);
        ResultText = result;
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
        var button = ButtonClickHandler.HandleSender(sender);

        try
        {            
            PrepareExpression(button);
        }
        catch (Exception ex)
        {
            AddAppLog(ex.Message, $"button_click_{button}");
        }
    }      

    private void ClearHistory(object sender, RoutedEventArgs e)
    {
        HistoryList.Items.Clear();
        ClearHistoryLog();
    }

    private void PrepareExpression(string sender)
    {
        switch (sender)
        {
            case ("1"):
            case ("2"):
            case ("3"):
            case ("4"):
            case ("5"):
            case ("6"):
            case ("7"):
            case ("8"):
            case ("9"):
            case ("0"):
                _calc.NumberClick(sender);
                break;

            case (MathHelper.Plus):
            case (MathHelper.Minus):
            case (MathHelper.Div):
            case (MathHelper.Mult):
                _calc.ActionClick(sender);
                break;

            case (MathHelper.Root):
                _calc.SqrtClick();
                break;

            case (MathHelper.Pow):
                _calc.PowClick();
                break;

            case (MathHelper.Comma):
                _calc.SpecClick(sender);
                break;

            case (MathHelper.Dot):
                _calc.DotClick();
                break;

            case (MathHelper.Equal):
                if (ResultText.Contains("Error") ||
                    ResultText.Contains(double.PositiveInfinity.ToString(CultureInfo.CurrentCulture))) break;
                WriteToHistory();
                _calc.EqualClick();
                break;

            case ("r"):
                _calc.RemoveClick();
                break;
            case ("c"):
                _calc.Clear();
                break;
        }

        WriteExpression();
    }

    #region HistoryLog

    private void AddHistoryLog(string expression, string result)
    {
        _db.HistoryLogs.Add(new HistoryLogEntity
        {
            Expression = expression,
            Result = result
        });

        _db.SaveChanges();
    }

    private void ClearHistoryLog()
    {
        _db.HistoryLogs.RemoveRange(_db.HistoryLogs.ToList());
        _db.SaveChanges();
    }

    private void LoadHistoryLog()
    {
        foreach (var history in _db.HistoryLogs.ToList())
            HistoryList.Items.Insert(0, history);
    }

    #endregion

    #region Application Log

    private void AddAppLog(string message, string module) 
    {
        _db.AppLogs.Add(new AppLogEntity
        {
            ExeptionMessage = message,
            DateTimeAdd = DateTime.Now,
            Module = module
        });

        _db.SaveChanges();
    }

    #endregion
}