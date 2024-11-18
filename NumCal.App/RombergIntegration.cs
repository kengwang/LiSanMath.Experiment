using System.Globalization;

namespace NumCal.App;

using Spectre.Console;
using Function = Func<double, double>;

public class RombergIntegration(Function f, double lower, double upper, double epsilon = double.Epsilon)
{
    
    private readonly Dictionary<int, double> _trapezoid = new();
    private readonly Dictionary<int, double> _simpson = new();
    private readonly Dictionary<int, double> _cotes = new();
    private readonly Dictionary<int, double> _romberg = new();

    public double Integrate()
    {
        int n = 1;
        while (true)
        {
            var result = Romberg(n);
            var result2 = Romberg(n * 2);
            if (Math.Abs(result - result2) / 255 < epsilon)
            {
                return result2;
            }
            n *= 2;
        }
    }
    
    public double Romberg(int n)
    {
        if (_romberg.TryGetValue(n, out var romberg))
        {
            return romberg;
        }
        var ans = (64 * Cotes(n * 2) - Cotes(n)) / 63;
        _romberg[n] = ans;
        return ans;
    }
    
    public double Cotes(int n)
    {
        if (_cotes.TryGetValue(n, out var cotes))
        {
            return cotes;
        }
        
        var ans = (16 * Simpson(n * 2) - Simpson(n)) / 15;
        _cotes[n] = ans;
        return ans;
    }
    
    
    public double Simpson(int n)
    {
        if (_simpson.TryGetValue(n, out var simpson))
        {
            return simpson;
        }
        var ans = (4 * Trapezoid(n * 2) - Trapezoid(n))/3;
        _simpson[n] = ans;
        return ans;
    }
    
    public double Trapezoid(int n)
    {
        if (_trapezoid.TryGetValue(n, out var trapezoid))
        {
            return trapezoid;
        }
        var h = (upper - lower) / n;
        var sigma = 0.0;
        for (var i = 1; i < n; i++)
        {
            sigma += f(lower + i * h);
        }
        
        var result =  h * (f(lower) + f(upper) + 2 * sigma) / 2;
        _trapezoid[n] =result;
        return result;
    }

    public void PrintTable()
    {
        var table = new Table();
        table.AddColumn("k");
        table.AddColumn("Trapezoid");
        table.AddColumn("Simpson");
        table.AddColumn("Cotes");
        table.AddColumn("Romberg");
        for (int i = 0; i < _trapezoid.Count; i++)
        {
            var k = (int)Math.Floor(Math.Pow(2, i));
            _trapezoid.TryGetValue(k, out var t);
            _simpson.TryGetValue(k, out var s);
            _cotes.TryGetValue(k, out var c);
            _romberg.TryGetValue(k, out var r);
            table.AddRow(k.ToString(), 
                t.ToString(CultureInfo.InvariantCulture),
                s.ToString(CultureInfo.InvariantCulture),
                c.ToString(CultureInfo.InvariantCulture),
                r.ToString(CultureInfo.InvariantCulture)
                );
        }
        AnsiConsole.Write(table);
    }
}