using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public struct Line
{
    public Vector2 Origin
    {
        get
        { return origin; }
        set
        {
            origin = value;
            direction = (end - origin);
            direction.Normalize();
        }
    }
    public Vector2 Direction
    {
        get
        { return direction; }
    }
    public Vector2 End
    {
        get
        { return end; }
        set
        {
            end = value;
            direction = (end - origin);
            direction.Normalize();
        }
    }
    public static Line Zero
    {
        get
        { return zero; }
    }
    private static Line zero = new Line(Vector2.Zero, Vector2.Zero);
    private Vector2 origin;
    private Vector2 direction;
    private Vector2 end;
    public Line(Vector2 Origin, Vector2 End)
    {
        origin = new Vector2(Origin.X - (Irbis.Irbis.halfResolution.X / Irbis.Irbis.screenScale), -(Origin.Y - (Irbis.Irbis.halfResolution.Y / Irbis.Irbis.screenScale)));
        end = new Vector2(End.X - (Irbis.Irbis.halfResolution.X / Irbis.Irbis.screenScale), -(End.Y - (Irbis.Irbis.halfResolution.Y / Irbis.Irbis.screenScale)));
        direction = (end - origin);
        direction.Normalize();
    }
    public Line(Vector2 Origin, Vector2 End, bool CorrectWorldCoordinates)
    {
        if (CorrectWorldCoordinates)
        {
            origin = new Vector2(Origin.X - (Irbis.Irbis.halfResolution.X / Irbis.Irbis.screenScale), -(Origin.Y - (Irbis.Irbis.halfResolution.Y / Irbis.Irbis.screenScale)));
            end = new Vector2(End.X - (Irbis.Irbis.halfResolution.X / Irbis.Irbis.screenScale), -(End.Y - (Irbis.Irbis.halfResolution.Y / Irbis.Irbis.screenScale)));
            direction = (end - origin);
            direction.Normalize();
        }
        else
        {
            origin = Origin;
            end = End;
            direction = (end - origin);
            direction.Normalize();
        }
    }
    public Vector2/*[]*/ Intersect(Ray ray)
    {
        // RAY in parametric: Point + Direction*T1
        float r_px = ray.Origin.X;
        float r_py = ray.Origin.Y;
        float r_dx = ray.Direction.X;
        float r_dy = ray.Direction.Y;
        // SEGMENT in parametric: Point + Direction*T2
        float s_px = origin.X;
        float s_py = origin.Y;
        float s_dx = end.X - origin.X;
        float s_dy = end.Y - origin.Y;
        //// Are they parallel? If so, no intersect
        //var r_mag = Math.Sqrt(r_dx * r_dx + r_dy * r_dy);
        //var s_mag = Math.Sqrt(s_dx * s_dx + s_dy * s_dy);
        //if (r_dx / r_mag == s_dx / s_mag && r_dy / r_mag == s_dy / s_mag)
        //{ // Directions are the same.
        //    return Vector2.Zero;
        //}
        // SOLVE FOR T1 & T2
        // r_px+r_dx*T1 = s_px+s_dx*T2 && r_py+r_dy*T1 = s_py+s_dy*T2
        // ==> T1 = (s_px+s_dx*T2-r_px)/r_dx = (s_py+s_dy*T2-r_py)/r_dy
        // ==> s_px*r_dy + s_dx*T2*r_dy - r_px*r_dy = s_py*r_dx + s_dy*T2*r_dx - r_py*r_dx
        // ==> T2 = (r_dx*(s_py-r_py) + r_dy*(r_px-s_px))/(s_dx*r_dy - s_dy*r_dx)
        float T2 = (r_dx * (s_py - r_py) + r_dy * (r_px - s_px)) / (s_dx * r_dy - s_dy * r_dx);
        float T1 = (s_px + s_dx * T2 - r_px) / r_dx;
        // Must be within parametic whatevers for RAY/SEGMENT
        if (T1 < 0) { return Vector2.Zero; }
        if (T2 < 0 || T2 > 1) { return Vector2.Zero; }
        // Return the POINT OF INTERSECTION
        return new Vector2(r_px + r_dx * T1, r_py + r_dy * T1);


        //float T2 = (ray.Direction.X * (origin.Y - ray.Origin.Y) + ray.Direction.Y * (ray.Origin.X - origin.X)) / (end.X * ray.Direction.Y - end.Y * ray.Direction.X);
        ////float T1 =  origin.X + (end.X * T2) - ray.Origin.X;
        //float T1 = (origin.X + (end.X * T2) - ray.Origin.X) / ray.Direction.X;
        //if (T1 > 0 && T2 < 0 && T2 >= -(1 / Irbis.Irbis.screenScale))
        //{
        //    return ray.Origin + (ray.Direction * T1);
        //    //return new Vector2(origin.X + (direction.X * T2), ray.Origin.Y + (ray.Direction.Y * T1));
        //}
        //return new Vector2(T1, T2);
    }
    //public void Draw(SpriteBatch sb, Color color, float depth)
    //{
    //    sb.Draw(Irbis.Irbis.nullTex, new Rectangle(origin.ToPoint(), new Point(1, (int)magnitude.Length())), null, color, (float)Math.Atan(magnitude.X / -magnitude.Y), Vector2.Zero, SpriteEffects.None, depth);
    //}
    public override string ToString()
    {
        return "{Origin:" + origin + " Direction:" + direction + " End:" + end + "}";
    }
    public void Draw()
    {
        VertexPositionColor[] vert = new VertexPositionColor[4];
        Vector2 perp = new Vector2(-direction.Y, direction.X);
        perp.Normalize();
        perp /= 2f;
        vert[0].Position = new Vector3((origin - perp), 000f);
        vert[0].Color = Color.Red;
        vert[1].Position = new Vector3((origin + perp), 000f);
        vert[1].Color = Color.Red;
        vert[2].Position = new Vector3(   (end + perp), 000f);
        vert[2].Color = Color.Red;
        vert[3].Position = new Vector3(   (end - perp), 000f);
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
