using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public struct Line
{
    public Point Center
    {
        get
        {
            if (vertical)
            {
                return new Point(positionX, positionY + (length / 2));
            }
            else
            {
                return new Point(positionX + (length / 2), positionY);
            }
        }
    }

    public int Left
    {
        get
        {
            return positionX;
        }
    }

    public int Right
    {
        get
        {
            if (vertical)
            {
                return positionX;
            }
            else
            {
                return positionX + length;
            }
        }
    }

    public int Top
    {
        get
        {
            return positionY;
        }
    }

    public int Bottom
    {
        get
        {
            if (vertical)
            {
                return positionY + length;
            }
            else
            {
                return positionY;
            }
        }
    }

    public bool Horizontal
    {
        get
        {
            if (vertical)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool Vertical
    {
        get
        {
            if (vertical)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public int X
    {
        get
        {
            return positionX;
        }
        set
        {
            positionX = value;
        }
    }

    public int Y
    {
        get
        {
            return positionY;
        }
        set
        {
            positionY = value;
        }
    }

    private int positionX;
    private int positionY;
    private int length;

    //vertical is true horizontal is false
    private bool vertical;

    public Line(int X, int Y, int Length, bool Vertical)
    {
        positionX = X;
        positionY = Y;
        length = Length;
        vertical = Vertical;
    }

    public Line(Point Location, int Length, bool Vertical)
    {
        positionX = Location.X;
        positionY = Location.Y;
        length = Length;
        vertical = Vertical;
    }
}
