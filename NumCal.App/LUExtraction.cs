using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace NumCal.App;

public static class LUExtraction
{
    public static (Matrix<double> L, Matrix<double> U) Extract(Matrix<double> A)
    {
        var L = Matrix<double>.Build.DenseIdentity(A.RowCount, A.ColumnCount);
        var U = Matrix<double>.Build.Dense(A.RowCount, A.ColumnCount);
        var mat = A.Clone();
        for (var i = 0; i < A.RowCount; i++)
        {
            // Calculate the row
            for (var j = i; j < A.ColumnCount; j++)
            {
                for (var k = 0; k < i; k++)
                {
                    mat[i, j] -= mat[i, k] * mat[k, j];
                }
            }
            // Calculate the column
            for (var j = i + 1; j < A.RowCount; j++)
            {
                for (var k = 0; k < i; k++)
                {
                    mat[j, i] -= mat[j, k] * mat[k, i];
                }
                mat[j, i] /= mat[i, i];
            }
        }
        for (var i = 0; i < A.RowCount; i++)
        {
            for (var j = 0; j < A.ColumnCount; j++)
            {
                if (i > j)
                {
                    L[i, j] = mat[i, j];
                }
                else
                {
                    U[i, j] = mat[i, j];
                }
            }
        }
        return (L, U);
    }
    
    public static (Matrix<double> L, Matrix<double> U) Extract(Matrix<double> A, Vector<double> B)
    {
        // combine A and B
        var AB = A.Append(B.ToColumnMatrix());
        return Extract(AB);
    }
}