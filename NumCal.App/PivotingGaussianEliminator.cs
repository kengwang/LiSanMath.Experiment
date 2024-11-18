using MathNet.Numerics.LinearAlgebra;
using Spectre.Console;

namespace NumCal.App;

public static class PivotingGaussianEliminator
{
    
    public static double[] SolveEquation(Matrix<double> matrix)
    {
        if (matrix.RowCount != matrix.ColumnCount - 1)
        {
            throw new ArgumentException("Matrix is not in the correct form.");
        }
        
        // 解为上三角矩阵
        for (var i = 0; i < matrix.RowCount - 1; i++)
        {
            matrix = matrix.SwapMajorRowBelowWithColumn(i);
            for (var j = i + 1; j < matrix.RowCount; j++)
            {
                AnsiConsole.MarkupLine("消元第 [blue]{0}[/] 行", j);
                var factor = matrix[j, i] / matrix[i, i];
                AnsiConsole.MarkupLine("第 [blue]{0}[/] 行减去第 [blue]{1}[/] 行的 [green]{2}[/] 倍", j, i, factor);
                matrix.SetRow(j, matrix.Row(j) - factor * matrix.Row(i));
                Console.WriteLine(matrix.ToMatrixString());
            }
        }
        
        // 回代求解
        var result = new double[matrix.RowCount];
        for (var i = matrix.RowCount - 1; i >= 0; i--)
        {
            var sum = 0.0;
            for (var j = i + 1; j < matrix.ColumnCount - 1; j++)
            {
                sum += matrix[i, j] * result[j];
            }
            result[i] = (matrix[i, matrix.ColumnCount - 1] - sum) / matrix[i, i];
        }
        
        return result;
    }

    public static Matrix<double> SwapMajorRowBelowWithColumn(this Matrix<double> matrix, int column)
    {
        var maxRow = column;
        for (var i = column + 1; i < matrix.RowCount; i++)
        {
            if (Math.Abs(matrix[i, column]) > Math.Abs(matrix[maxRow, column]))
            {
                maxRow = i;
            }
        }
        
        if (maxRow != column)
        {
            AnsiConsole.MarkupLine("交换第 [blue]{0}[/] 行和第 [blue]{1}[/] 行", column, maxRow);
            matrix = SwapRows(matrix, column, maxRow);
            AnsiConsole.WriteLine(matrix.ToMatrixString());
        }
        
        return matrix;
    }
    
    public static Matrix<double> SwapRows(Matrix<double> matrix, int row1, int row2)
    {
        var temp = matrix.Row(row1);
        matrix.SetRow(row1, matrix.Row(row2));
        matrix.SetRow(row2, temp);
        return matrix;
    }
}