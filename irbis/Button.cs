using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Button
{
    public Rectangle bounds;
    RectangleBorder border;
    Direction alignSide;
    public string highlightStatement;
    public string originalStatement;
    public string buttonStatement;
    Color buttonBorderColor;
    Color buttonFillColor;
    bool buttonFill;
    Print text;

    bool drawBorder;

    Texture2D borderTex;

    //MouseState prevMouseState;

    public Point buttonLocation;

    float depth;

    public Button(Rectangle buttonBounds, Direction align, string buttonText, string highlightText, Color borderColor, Texture2D borderTexture, Font font, Color fillColor, bool fill, float drawDepth)
	{
        bounds = buttonBounds;
        alignSide = align;
        originalStatement = buttonStatement = buttonText;
        buttonBorderColor = borderColor;
        highlightStatement = highlightText;
        buttonFillColor = fillColor;
        buttonFill = fill;
        depth = drawDepth;

        borderTex = borderTexture;
        buttonLocation = bounds.Center;


        text = new Print(bounds.Width, font, Color.White, false, buttonLocation, Direction.forward, depth);
        text.Update(originalStatement);

        border = new RectangleBorder(bounds, buttonBorderColor);
        drawBorder = true;
    }

    public Button(Rectangle buttonBounds, Direction align, string buttonText, string highlightText, Color borderColor, Texture2D borderTexture, Font font, Color fillColor, bool fill, bool dBorder, float drawDepth)
    {
        bounds = buttonBounds;
        alignSide = align;
        originalStatement = buttonStatement = buttonText;
        highlightStatement = highlightText;

        buttonBorderColor = borderColor;
        buttonFillColor = fillColor;
        buttonFill = fill;
        depth = drawDepth;

        borderTex = borderTexture;
        buttonLocation = bounds.Center;


        text = new Print(bounds.Width, font, Color.White, false, buttonLocation, align, depth);
        text.Update(buttonStatement);
        drawBorder = dBorder;

        if (drawBorder)
        {
            border = new RectangleBorder(bounds, buttonBorderColor);
        }
    }

    public Button(Rectangle buttonBounds, Direction align, Side side, string buttonText, string highlightText, Color borderColor, Texture2D borderTexture, Font font, Color fillColor, bool fill, bool dBorder, float drawDepth)
    {
        bounds = buttonBounds;
        alignSide = align;
        originalStatement = buttonStatement = buttonText;
        highlightStatement = highlightText;

        buttonBorderColor = borderColor;
        buttonFillColor = fillColor;
        buttonFill = fill;
        depth = drawDepth;

        borderTex = borderTexture;
        buttonLocation = bounds.Center;

        if (side == Side.left)
        {
            text = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Left, bounds.Center.Y), align, depth);
        }
        else if (side == Side.right)
        {
            text = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Right, bounds.Center.Y), align, depth);
        }
        else if(side == Side.top)
        {
            text = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Center.X, bounds.Top), align, depth);
        }
        else if (side == Side.bottom)
        {
            text = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Center.X, bounds.Bottom), align, depth);
        }

        text.Update(buttonStatement);
        drawBorder = dBorder;

        if (drawBorder)
        {
            border = new RectangleBorder(bounds, buttonBorderColor);
        }
    }

    public bool Pressed(MouseState mouseState)
    {
        if (bounds.Contains(mouseState.Position.X, mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed)
        {
            return true;
        }
        return false;
    }

    public bool Pressed(MouseState mouseState, MouseState previousMouseState)
    {
        if (bounds.Contains(mouseState.Position.X, mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
        {
            return true;
        }
        return false;
    }

    public void Update(string statement)
    {
        buttonStatement = statement;
        text.Update(statement, true);
    }

    public void Update(string statement, bool clear)
    {
        if (clear)
        {
            buttonStatement = statement;
        }
        else
        {
            buttonStatement += statement;
        }
        text.Update(buttonStatement, true);
    }

    public bool Contains(MouseState mouseState)
    {
        return bounds.Contains(mouseState.Position.X, mouseState.Position.Y);
    }

    public void Draw(SpriteBatch sb)
    {
        text.Draw(sb);
        if (drawBorder) { border.Draw(sb, borderTex); }
    }
}
