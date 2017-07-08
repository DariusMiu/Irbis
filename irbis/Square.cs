using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

[Serializable]
public class Square : ICollisionObject
{
    public Rectangle Collider
    {
        get
        {
            return collider;
        }
        set
        {
            collider = value;
        }
    }
    private Rectangle collider;


    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position.X = value.X - 16;
            position.Y = value.Y - 16;
        }
    }

    public bool alwaysDraw;
    public Texture2D tex;
    private Vector2 position;
    public bool drawTex;
    public float depth;
    public Color color;
    private float scale;
    //public string texture;

    //bool pressed;
    public Square(Texture2D t, Point initialPos, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = drawDepth;
        tex = t;
        position = initialPos.ToVector2();
        position.X = position.X * 32;
        position.Y = position.Y * 32;
        drawTex = true;
        color = Color.White;
        scale = 1;
        collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
    }
    public Square(Texture2D t, Point initialPos, bool exactCoords, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = drawDepth;
        tex = t;
        position = initialPos.ToVector2();
        if (exactCoords)
        {
            //position.X = position.X;
            //position.Y = position.Y;
        }
        else
        {
            position.X = position.X * 32;
            position.Y = position.Y * 32;
        }
        drawTex = true;
        color = Color.White;
        scale = 1;
        collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
    }
    public Square(Texture2D t, Point initialPos, int scale, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = drawDepth;
        tex = t;
        position = initialPos.ToVector2();
        drawTex = true;
        color = Color.White;
        scale = 1;
        collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
    }
    public Square(Texture2D t, Point initialPos, int squareScale, bool hascollider, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = drawDepth;
        tex = t;
        position = initialPos.ToVector2();
        scale = squareScale;
        if (hascollider) { collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale)); }
        else { collider = Rectangle.Empty; }
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Color drawColor, Point initialPos, int width, int height, bool hascollider, bool useExactPixels, bool AlwaysDraw, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        scale = 1;
        depth = drawDepth;
        tex = t;
        position = initialPos.ToVector2();
        color = drawColor;

        if (hascollider) { collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale)); }
        else { collider = Rectangle.Empty; }
        alwaysDraw = AlwaysDraw;
    }
    public Square(Texture2D t, Point drawPos, Point colliderPos, int drawWidth, int drawHeight, int colliderWidth, int colliderHeight, bool useExactPixels, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        scale = 1;
        depth = drawDepth;
        tex = t;
        position = drawPos.ToVector2();
        if (useExactPixels)
        {
            collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
        }
        else
        {
            collider = new Rectangle((int)position.X, (int)position.Y, (int)(32 * scale), (int)(32 * scale));
        }
        drawTex = true;
        color = Color.White;
    }

    public void Draw(SpriteBatch sb)
    {
        ////if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Draw"); }
        //if (alwaysDraw || Irbis.Irbis.IsTouching(collider, Irbis.Irbis.screenspace))
        {
            sb.Draw(tex, position * Irbis.Irbis.screenScale, null, color, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
        }
    }
}
