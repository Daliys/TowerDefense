using System;

/// <summary>
///  Class for points. Used to determine the position of objects. (Easier to use than Vector2)
/// </summary>
[Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is not Point point) return false;
        return this.x == point.x && this.y == point.y;
    }

    public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);

    /// <summary>
    ///  Returns the point of the side of the current point.
    /// </summary>
    /// <param name="sideNum"> The side number of the point.</param>
    /// <returns></returns>
    public Point GetPoint(int sideNum)
    {
        return sideNum switch
        {
            0 => Up(),
            1 => RightUp(),
            2 => Right(),
            3 => RightDown(),
            4 => Down(),
            5 => LeftDown(),
            6 => Left(),
            7 => LeftUp(),
            _ => this
        };
    }

    /// <summary>
    ///  Returns positive values of the point.
    /// </summary>
    public void Abs()
    {
        x = Math.Abs(x);
        y = Math.Abs(y);
    }
    
    public Point Right() => new Point(x + 1, y);
    public Point Left() => new Point(x - 1, y);
    public Point Up() => new Point(x, y - 1);
    public Point Down() => new Point(x, y + 1);
    public Point RightUp() => new Point(x + 1, y - 1);
    public Point RightDown() => new Point(x + 1, y + 1);
    public Point LeftUp() => new Point(x - 1, y - 1);
    public Point LeftDown() => new Point(x - 1, y + 1);
    
    public override string ToString()
    {
        return "x: " + x + " y: " + y;
    }
}