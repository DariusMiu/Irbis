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


    public Point Position
    {
        get
        {
            return pos;
        }
        set
        {
            pos.X = value.X - 16;
            pos.Y = value.Y - 16;
            drawRect = collider = new Rectangle(pos.X, pos.Y, 32, 32);
        }
    }

    public bool alwaysDraw;
    public Texture2D tex;
    public Rectangle drawRect;
    private Point pos;
    public bool drawTex;
    public float depth;
    public Color color;
    //public string texture;

    //bool pressed;
    public Square(Texture2D t, Point initialPos, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        pos.X = pos.X * 32;
        pos.Y = pos.Y * 32;
        drawRect = collider = new Rectangle(pos.X, pos.Y, 32, 32);
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Point initialPos, bool exactCoords, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        if (exactCoords)
        {
            //pos.X = pos.X;
            //pos.Y = pos.Y;
        }
        else
        {
            pos.X = pos.X * 32;
            pos.Y = pos.Y * 32;
        }
        drawRect = collider = new Rectangle(pos.X, pos.Y, 32, 32);
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Point initialPos, int scale, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        drawRect = collider = new Rectangle(pos.X, pos.Y, 32 * scale, 32 * scale);
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Point initialPos, int scale, bool hascollider, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        drawRect = new Rectangle(pos.X, pos.Y, 32 * scale, 32 * scale);
        if (hascollider) { collider = drawRect; }
        else { collider = Rectangle.Empty; }
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Color drawColor, Point initialPos, int width, int height, bool hascollider, bool useExactPixels, bool AlwaysDraw, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        color = drawColor;

        if (useExactPixels)
        {
            drawRect = new Rectangle(pos.X, pos.Y, width, height);
        }
        else
        {
            drawRect = new Rectangle(pos.X * 32, pos.Y * 32, width * 32, height * 32);
        }
        if (hascollider) { collider = drawRect; }
        else { collider = Rectangle.Empty; }
        alwaysDraw = AlwaysDraw;
    }
    public Square(Texture2D t, Point drawPos, Point colliderPos, int drawWidth, int drawHeight, int colliderWidth, int colliderHeight, bool useExactPixels, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = drawPos;
        if (useExactPixels)
        {
            drawRect = new Rectangle(drawPos.X, drawPos.Y, drawWidth, drawHeight);
            collider = new Rectangle(colliderPos.X, colliderPos.Y, colliderWidth, colliderHeight);
        }
        else
        {
            drawRect = new Rectangle(drawPos.X * 32, drawPos.Y * 32, drawWidth * 32, drawHeight * 32);
            collider = new Rectangle(colliderPos.X * 32, colliderPos.Y * 32, colliderWidth * 32, colliderHeight * 32);
        }
        drawTex = true;
        color = Color.White;
    }

    public void Draw(SpriteBatch sb)
    {
        if (alwaysDraw || Irbis.Irbis.IsTouching(drawRect, Irbis.Irbis.screenspace))
        {
            sb.Draw(tex, drawRect, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }
    }
}
