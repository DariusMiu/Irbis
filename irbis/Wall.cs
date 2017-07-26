public struct Wall
{
    public int Top
    {
        get
        { return _top; }
        set
        { _top = value; }
    }
    public int Bottom
    {
        get
        { return _bottom; }
        set
        { _bottom = value; }
    }
    public int Left
    {
        get
        { return _left; }
        set
        { _left = value; }
    }
    public int Right
    {
        get
        { return _right; }
        set
        { _right = value; }
    }
    public int Total
    {
        get
        { return _top + _bottom + _left + _right; }
    }
    /// <summary> Returns true if Left or Right is greater than zero </summary>
    public bool Horizontal
    {
        get
        { return (_left > 0 || _right > 0); }
    }
    /// <summary> Returns true if Top or Bottom is greater than zero </summary>
    public bool Vertical
    {
        get
        { return (_bottom > 0 || _top > 0); }
    }
    /// <summary> Static constant equalling Wall(0, 0, 0, 0) </summary>
    public static Wall Zero
    {
        get
        { return zero; }
    }
    private static Wall zero = new Wall(0, 0, 0, 0);
    private int _top;
    private int _bottom;
    private int _left;
    private int _right;
    public Wall(int top, int bottom, int left, int right)
    {
        _top = top;
        _bottom = bottom;
        _left = left;
        _right = right;
    }
    public static bool operator ==(Wall value1, Wall value2)
    {
        return ((value1._top == value2._top) && (value1._bottom == value2._bottom) && (value1._left == value2._left) && (value1._right == value2._right));
    }
    public static bool operator !=(Wall value1, Wall value2)
    {
        return ((value1._top != value2._top) || (value1._bottom != value2._bottom) || (value1._left != value2._left) || (value1._right != value2._right));
    }
    public bool Equals(Wall value)
    {
        return ((_top == value._top) && (_bottom == value._bottom) && (_left == value._left) && (_right == value._right));
    }
    public override bool Equals(object obj)
    {
        if (obj is Wall) { return Equals((Wall)obj); }
        else { return false; }
    }
    public override int GetHashCode()
    {
        return (_top.GetHashCode() + _bottom.GetHashCode() + _left.GetHashCode() + _right.GetHashCode());
    }
    public override string ToString()
    {
        return "{Top:" + _top + " Bottom:" + _bottom + " Left:" + _left + " Right:" + _right + "}";
    }
}