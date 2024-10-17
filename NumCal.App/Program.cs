using MathNet.Numerics.LinearAlgebra;
using NumCal.App;
using Spectre.Console;

// var result = NewtonMethod.Solve(
//     (x) => x*x - Math.Exp(x),
//     (x) => 2*x - Math.Exp(x),
//     0.0,
//     double.Epsilon
//     );
//     AnsiConsole.MarkupLine("得到的解为: [bold green]{0}[/]", result.EscapeMarkup());

// var input = Matrix<double>.Build.DenseOfArray(new double[,]
// {
//     { 1, 1, 0, 3, 4 },
//     { 2, 1, -1, 1, 1 },
//     { 3, -1, -1, 3, -3 },
//     { -1, 2, 3, -1, 4 }
// });
//
// var res = EquationSolver.SolveEquation(input);

// var (L , U) = LUExtraction.Extract(Matrix<double>.Build.DenseOfArray(new double[,]
// {
//     { 48, -24, 0, 12, 4 },
//     { -24, 24, 12, 12, 4 },
//     { 0, 6, 20, 2, -2 },
//     { -6, 6, 2, 16, -2 }
// }));
//
// AnsiConsole.MarkupLine("{0}", L.ToMatrixString().EscapeMarkup());
// AnsiConsole.MarkupLine("{0}", U.ToMatrixString().EscapeMarkup());

NewtonInterpolation.Solve(new()
{
    { 20, 1.20103 },
    { 21, 1.32222 },
    { 22, 1.34242 },
    { 23, 1.36173 },
    { 24, 1.38021 }
});