using Spectre.Console;
using Function = System.Func<double, double>;

namespace NumCal.App;

public static class NewtonMethod
{
    public static double Solve(Function f, Function df, double x0, double tol = 1e-6, int maxIter = 100, bool debug = false)
    {

        var x = x0;
        AnsiConsole.MarkupLine("选取的初值: [bold blue]{0}[/]", x);
        for (var i = 0; i < maxIter; i++)
        {
            var dx = -f(x) / df(x);
            x += dx;
            AnsiConsole.MarkupLine("迭代的 x{0} = [bold blue]{1}[/] , delta = [yellow]{2}[/]", i,x, Math.Abs(dx));
            if (Math.Abs(dx) < tol)
            {
                return x;
            }
        }
        throw new Exception("Newton's method did not converge");
    }
}