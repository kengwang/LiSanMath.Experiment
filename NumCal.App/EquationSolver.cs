using MathNet.Numerics.LinearAlgebra;

namespace NumCal.App;

public static class EquationSolver
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
            Console.WriteLine(matrix.ToMatrixString());
            matrix = matrix.SwapMajorRowBelowWithColumn(i);
            Console.WriteLine(matrix.ToMatrixString());
            for (var j = i + 1; j < matrix.RowCount; j++)
            {
                var factor = matrix[j, i] / matrix[i, i];
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
        
        result.ToList().ForEach(Console.WriteLine);
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
            matrix = SwapRows(matrix, column, maxRow);
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