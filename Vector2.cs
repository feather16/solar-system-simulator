using System;
using System.Drawing;

public struct Vector2
{
    public double x, y;

    public static readonly Vector2 EX = new Vector2(1, 0);
    public static readonly Vector2 EY = new Vector2(0, 1);
    public static readonly Vector2 ZERO = new Vector2();
    
    public Vector2(Point p)
    {
        x = p.X;
        y = p.Y;
    }

    public Vector2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2((double x, double y) v)
    {
        x = v.x;
        y = v.y;
    }

    public bool isZero => x == 0 && y == 0;

    public static Vector2 operator +(Vector2 v1, Vector2 v2) 
        => new Vector2(v1.x + v2.x, v1.y + v2.y);

    public static Vector2 operator +(Vector2 v1, (double x,double y) v2)
        => new Vector2(v1.x + v2.x, v1.y + v2.y);

    public static Vector2 operator +((double x, double y) v1, Vector2 v2)
        => new Vector2(v1.x + v2.x, v1.y + v2.y);

    public static Vector2 operator *(Vector2 v, double d)
        => new Vector2(v.x * d, v.y * d);

    public static Vector2 operator /(Vector2 v, double d)
        => new Vector2(v.x / d, v.y / d);

    public static Vector2 operator *(double d, Vector2 v)
        => v * d;

    public static Vector2 operator -(Vector2 v)
        => -1 * v;

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
        => v1 + -v2;

    public static Vector2 operator -(Vector2 v1, (double x, double y) v2)
        => v1 + -new Vector2(v2);

    public static Vector2 operator -((double x, double y) v1, Vector2 v2)
        => v1 + -v2;

    public Vector2 rotate(double theta)
    {
        double ct = Math.Cos(theta);
        double st = Math.Sin(theta);
        return new Vector2(x * ct - y * st, x * st + y * ct);
    }

    public PointF toPointF => new PointF((float)x, (float)y);

    public double abs2 => x * x + y * y;
    public double abs => Math.Sqrt(abs2);

    public override string ToString() => "(" + x + ", " + y + ")";
}