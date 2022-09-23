using System;
using System.Linq;
using GlobalConstants;

namespace MainLib;

public class Matrix : IEquatable<Matrix>
{
    public Matrix(double[,] mtx)
    {
        Mtx = mtx;
    }

    public double[,] Mtx { get; }

    public double this[int row, int column]
    {
        get => Mtx[row, column];
        set => Mtx[row, column] = value;
    }

    public bool IsInvertible => GetDeterminant() != 0;

    public Matrix Transpose()
    {
        var res = new double[Mtx.GetLength(1), Mtx.GetLength(0)];
        for (var i = 0; i < res.GetLength(0); i++)
        {
            for (var j = 0; j < res.GetLength(1); j++)
            {
                res[i, j] = Mtx[j, i];
            }
        }

        return new Matrix(res);
    }

    public Matrix GetSubMatrix(int row, int col)
    {
        var res = new Matrix(new double[Mtx.GetLength(0) - 1, Mtx.GetLength(1) - 1]);

        for (int i = 0, resI = 0; i < Mtx.GetLength(0); i++)
        {
            if (i == row)
            {
                continue;
            }

            for (int j = 0, resJ = 0; j < Mtx.GetLength(1); j++)
            {
                if (j == col)
                {
                    continue;
                }

                res[resI, resJ++] = Mtx[i, j];
            }

            resI++;
        }

        return res;
    }

    public double GetMinor(int row, int col)
    {
        var sub = GetSubMatrix(row, col);
        return sub.GetDeterminant();
    }

    public double GetDeterminant()
    {
        if (Mtx.GetLength(0) == 2)
        {
            return Mtx[0, 0] * Mtx[1, 1] - Mtx[0, 1] * Mtx[1, 0];
        }
        else
        {
            var row = GetRow(0);

            return row.Select((t, i) => t * GetCofactor(0, i)).Sum();
        }

        return -1;
    }

    public Matrix GetInverse()
    {
        var res = new Matrix(new double[Mtx.GetLength(0), Mtx.GetLength(1)]);
        var determinant = GetDeterminant();
        for (var i = 0; i < Mtx.GetLength(0); i++)
        {
            for (var j = 0; j < Mtx.GetLength(1); j++)
            {
                res[j, i] = GetCofactor(i, j) / determinant;
            }
        }

        return res;
    }

    public double GetCofactor(int row, int col)
    {
        return (col + row) % 2 == 0 ? GetMinor(row, col) : -GetMinor(row, col);
    }

    public static Matrix operator *(Matrix left, Matrix right)
    {
        var res = new Matrix(new double[left.Mtx.GetLength(0), left.Mtx.GetLength(1)]);
        for (var i = 0; i < res.Mtx.GetLength(0); i++)
        {
            for (var j = 0; j < res.Mtx.GetLength(1); j++)
            {
                var col = left.GetRow(i);
                var row = right.GetColumn(j);
                res[i, j] = col.Zip(row, (col, row) => col * row).Sum();
            }
        }

        return res;
    }

    public static MathTuple operator *(Matrix left, MathTuple right)
    {
        var temp = new[] { right.X, right.Y, right.Z, right.W };

        var res = new MathTuple(temp.Zip(left.GetRow(0), (col, row) => col * row).Sum(),
            temp.Zip(left.GetRow(1), (col, row) => col * row).Sum(),
            temp.Zip(left.GetRow(2), (col, row) => col * row).Sum(),
            temp.Zip(left.GetRow(3), (col, row) => col * row).Sum());
        return res;
    }


    private double[] GetRow(int row)
    {
        var res = new double[Mtx.GetLength(0)];
        for (int i = 0; i < Mtx.GetLength(0); i++)
        {
            res[i] = Mtx[row, i];
        }

        return res;
    }

    private double[] GetColumn(int col)
    {
        var res = new double[Mtx.GetLength(1)];
        for (int i = 0; i < Mtx.GetLength(1); i++)
        {
            res[i] = Mtx[i, col];
        }

        return res;
    }

    #region Equality

    public bool Equals(Matrix other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.Mtx.Rank != Mtx.Rank
            || Mtx.GetLength(0) != other.Mtx.GetLength(0)
            || Mtx.GetLength(1) != other.Mtx.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < Mtx.GetLength(0); i++)
        {
            for (int j = 0; j < Mtx.GetLength(1); j++)
            {
                if (Math.Abs(Mtx[i, j] - other.Mtx[i, j]) > NumberConstants.Epsilon)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Matrix)obj);
    }

    public override int GetHashCode()
    {
        return Mtx.GetHashCode();
    }

    public static bool operator ==(Matrix left, Matrix right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Matrix left, Matrix right)
    {
        return !Equals(left, right);
    }

    #endregion
}