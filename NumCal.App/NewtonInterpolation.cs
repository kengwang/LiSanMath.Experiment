using System.Globalization;
using Spectre.Console;

namespace NumCal.App;

public class NewtonInterpolation
{
    public static List<double> Solve(Dictionary<double, double> points)
    {
        var quotient = new List<List<double>>();
        var x = points.Keys.ToList();
        quotient.Add(points.Values.ToList());
        for (var n = 1; n < points.Count; n++)
        {
            // fill zero
            var last = quotient[n-1];
            quotient.Add(new List<double>());
            for (var i = 0; i < n; i++)
            {
                quotient[n].Add(0);
            }
            for (var i = n; i < points.Count; i++)
            {
                quotient[n].Add((last[i] - last[i-1]) / (x[i] - x[i-n]));
            }
        }
        var table = new Table();
        table.AddColumn("x");
        for (var i = 0; i < points.Count; i++)
        {
            table.AddColumn($"f(x{i})");
        }
        for (var i = 0; i < points.Count; i++)
        {
            var row = new List<string> { x[i].ToString(CultureInfo.InvariantCulture) };
            for (var j = 0; j < points.Count; j++)
            {
                row.Add(quotient[j][i].ToString(CultureInfo.InvariantCulture));
            }
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
        return new();
    }
}