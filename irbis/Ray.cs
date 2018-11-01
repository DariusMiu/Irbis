using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public struct Ray
{
    public Vector2 Origin
    {
        get
        { return origin; }
        set
        {
            origin = new Vector2(value.X - (Irbis.Irbis.halfResolution.X ), -(value.Y - (Irbis.Irbis.halfResolution.Y )));
        }
    }
    public Vector2 Direction
    {
        get
        { return direction; }
        //set
        //{
        //    direction = new Vector2(value.X, -value.Y);
        //    direction.Normalize();
        //}
    }
    public float Angle
    {
        get
        { return angle; }
    }
    public static Line Zero
    {
        get
        { return zero; }
    }
    private static Line zero = new Line(Vector2.Zero, Vector2.Zero);
    private Vector2 origin;
    private Vector2 direction;
    private float angle;
    public Ray(Vector2 Origin, float Angle)
    {
        angle = Angle;
        origin = new Vector2(Origin.X - (Irbis.Irbis.halfResolution.X ), -(Origin.Y - (Irbis.Irbis.halfResolution.Y )));
        direction = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));

        //direction = Direction;
        direction.Normalize();
    }
    //public Line Intersects
    public override string ToString()
    {
        return "{origin:" + origin + " angle:" + angle + " direction:" + direction + "}";
    }

    public void Draw()
    {
        VertexPositionColor[] vert = new VertexPositionColor[4];
        Vector2 perp = new Vector2(-direction.Y, direction.X);
        perp.Normalize();
        perp /= 2f;
        Vector2 displayEnd = new Vector2(origin.X + (direction.X * 500f), origin.Y + (direction.Y * 500f));
        vert[0].Position = new Vector3((origin - perp), 000f);
        vert[1].Position = new Vector3((origin + perp), 000f);
        vert[2].Position = new Vector3((displayEnd + perp), 000f);
        vert[3].Position = new Vector3((displayEnd - perp), 000f);
        short[] ind = new short[6];
        ind[0] = 0;
        ind[1] = 1;
        ind[2] = 2;
        ind[3] = 1;
        ind[4] = 2;
        ind[5] = 3;
        Irbis.Irbis.graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vert, 0, vert.Length, ind, 0, ind.Length / 3);
    }
    public Vector2 Intersect(Line[] lines)
    {
        float closestsqrdistance = float.MaxValue;
        Vector2 closest = Vector2.Zero;
        closestsqrdistance = float.MaxValue;
        //closest = new Vector2(float.MaxValue, float.MaxValue);
        foreach (Line l in lines)
        {
            Vector2 tempV2 = l.Intersect(this);
            float tempfloat = Vector2.DistanceSquared(origin, tempV2);
            if (closestsqrdistance >= tempfloat && tempV2 != Vector2.Zero)
            {
                closestsqrdistance = tempfloat;
                closest = tempV2;
            }
        }
        return closest;
    }
    public Vector2 Intersect(Shape[] Shapes)
    {
        Line[] lineArray = new Line[Shape.TotalLines(Shapes)];
        int currentIndex = 0;
        foreach (Shape s in Shapes)
        {
            s.Lines.CopyTo(lineArray, currentIndex);
            currentIndex += s.NumberOfLines;
        }

        return Intersect(lineArray);
    }
    public void Draw(Vector2 end)
    {
        Vector2 displayEnd;
        //end = new Vector2(end.X, end.Y);
        //float Tx = (end.X - origin.X) / direction.X;
        //float Ty = (end.Y - origin.Y) / direction.Y;
        //displayEnd = new Vector2(origin.X + direction.X * Tx, origin.Y + direction.Y * Ty);
        if (end != Vector2.Zero && end.X <= Irbis.Irbis.resolution.X && end.Y <= Irbis.Irbis.resolution.Y /*&& Tx <= Ty + 0.0001f && Tx >= Ty - 0.0001f && displayEnd == end*/)
        { displayEnd = end; }
        else
        { displayEnd = new Vector2(origin.X + (direction.X * Irbis.Irbis.resolution.X), origin.Y + (direction.Y * Irbis.Irbis.resolution.X)); }

        VertexPositionColor[] vert = new VertexPositionColor[4];
        Vector2 perp = new Vector2(-direction.Y, direction.X);
        perp.Normalize();
        perp /= 2f;
        //Vector2 displayOrigin = new Vector2(origin.X - (Irbis.Irbis.halfResolution.X / Irbis.Irbis.screenScale), -(origin.Y - (Irbis.Irbis.halfResolution.Y / Irbis.Irbis.screenScale)));
        //displayEnd /= 2;
        vert[0].Position = new Vector3((origin - perp), 000f);
        vert[0].Color = Color.Red;
        vert[1].Position = new Vector3((origin + perp), 000f);
        vert[1].Color = Color.Red;
        vert[2].Position = new Vector3((displayEnd + perp), 000f);
        vert[2].Color = Color.Red;
        vert[3].Position = new Vector3((displayEnd - perp), 000f);
        vert[3].Color = Color.Red;
        short[] ind = new short[6];
        ind[0] = 0;
        ind[1] = 1;
        ind[2] = 2;
        ind[3] = 1;
        ind[4] = 2;
        ind[5] = 3;
        Irbis.Irbis.graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vert, 0, vert.Length, ind, 0, ind.Length / 3);
    }
}
