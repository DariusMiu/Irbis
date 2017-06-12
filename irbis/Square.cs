using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class Square : ICollisionObject
{
    public Rectangle collider
    {
        get
        {
            return Collider;
        }
        set
        {
            Collider = value;
        }
    }
    public Rectangle Collider;


    //public Rectangle collider
    //{
    //    get;
    //    set;
    //}

    public Texture2D tex;
    public Rectangle drawRect;
    public Point pos;
    public bool drawTex;
    float depth;
    public Color color;

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
    public Square(Texture2D t, Point initialPos, int scale, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        drawRect = collider = new Rectangle(pos.X, pos.Y, 32 * scale, 32 * scale);
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Point initialPos, int scale, bool hasCollider, float drawDepth)
    {
        depth = drawDepth;
        tex = t;
        pos = initialPos;
        drawRect = new Rectangle(pos.X, pos.Y, 32 * scale, 32 * scale);
        if (hasCollider) { collider = drawRect; }
        else { collider = Rectangle.Empty; }
        drawTex = true;
        color = Color.White;
    }
    public Square(Texture2D t, Color drawColor, Point initialPos, int width, int height, bool hasCollider, bool useExactPixels, bool draw, float drawDepth)
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
        if (hasCollider) { collider = drawRect; }
        else { collider = Rectangle.Empty; }
        drawTex = draw;
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

    public void Update(GameTime gameTime)
    {
        //MouseState mouseState = Mouse.GetState();
        //if (rect.Contains(mouseState.Position.X, mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed)
        //{
        //    pressed = true;
        //}
        //if (!(mouseState.LeftButton == ButtonState.Pressed))
        //{
        //    pressed = false;
        //}
        //if (pressed)
        //{
        //    rect.X = mouseState.Position.X - 5;
        //    rect.Y = mouseState.Position.Y - 5;
        //}
    }

    public void Draw(SpriteBatch sb)
    {
        if (drawTex) { sb.Draw(tex, drawRect, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth); }
    }
}
