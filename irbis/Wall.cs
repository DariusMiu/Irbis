﻿public struct Wall
{
    public int Top
    {
        get
        {
            return t;
        }
        set
        {
            t = value;
        }
    }
    public int Bottom
    {
        get
        {
            return b;
        }
        set
        {
            b = value;
        }
    }
    public int Left
    {
        get
        {
            return l;
        }
        set
        {
            l = value;
        }
    }
    public int Right
    {
        get
        {
            return r;
        }
        set
        {
            r = value;
        }
    }

    public Wall Zero
    {
        get
        {
            return zero;
        }
    }
    private static Wall zero = new Wall(0,0,0,0);

    private int t;
    private int b;
    private int l;
    private int r;

    public Wall(int top, int bottom, int left, int right)
    {
        t = top;
        b = bottom;
        l = left;
        r = right;
    }

    public static bool operator ==(Wall value1, Wall value2)
    {
        return ((value1.t == value2.t) && (value1.b == value2.b) && (value1.l == value2.l) && (value1.r == value2.r));
    }

    public static bool operator !=(Wall value1, Wall value2)
    {
        return ((value1.t != value2.t) || (value1.b != value2.b) || (value1.l != value2.l) || (value1.r != value2.r));
    }

    public bool Equals(Wall value)
    {
        return ((this.t == value.t) && (this.b == value.b) && (this.l == value.l) && (this.r == value.r));
    }

    public override bool Equals(object obj)
    {
        if (obj is Wall) return this.Equals((Wall)obj);
        else return false;
    }

    public override int GetHashCode()
    {
        return (this.t.GetHashCode() + this.b.GetHashCode() + this.l.GetHashCode() + this.r.GetHashCode());
    }

    public override string ToString()
    {
        return "{Top:" + this.t + " Bottom:" + this.b + " Left:" + this.l + " Right:" + this.r + "}";
    }

}
