using System;

namespace wpf_calculator.Models.Entity;

public class AppLogEntity
{
    public int Id { get; set; }
    public string ExeptionMessage { get; set; }
    public string Module { get; set; }
    public DateTime DateTimeAdd { get; set; }
}