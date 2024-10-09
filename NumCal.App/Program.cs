using NumCal.App;
using Spectre.Console;

var result = NewtonMethod.Solve(
    (x) => x*x - Math.Exp(x),
    (x) => 2*x - Math.Exp(x),
    0.0,
    double.Epsilon
    );
    AnsiConsole.MarkupLine("得到的解为: [bold green]{0}[/]", result);