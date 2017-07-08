using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class UIElementSlider
{
    public Rectangle bounds;
    Rectangle valueRect;
    Vector2 valueLocation;
    Vector2 origin;

    Texture2D borderTex;
    public Texture2D overlayTex;
    Texture2D fillTex;

    public float overlayAnimationSpeed;
    Rectangle overlaySourceRect;
    int currentOverlayFrame;
    //float Irbis.Irbis.DeltaTime;
    float timeSinceLastFrame;

    float depth;

    public float value;
    public float maxValue;
    float halfWidth;
    float halfMaxValue;
    Direction align;

    float overlayDepth;
    float borderDepth;
    float valueDepth;

    Color borderColor;
    Color fillColor;
    
    bool drawOverlay;

    Print printValue;
    public Vector2 printLocation;
    Font fernt;
    bool printText;

    public UIElementSlider(Direction alignSide, Point location, int maxWidth, int height, float maxVal, Color border_color, Color fill_color, Texture2D fillTexture,
        Texture2D borderTexture, Texture2D overlayTexture, Font font, float drawDepth)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.UIElementSlider"); }
        fernt = font;
        printText = true;
        drawOverlay = true;
        depth = drawDepth;

        overlayDepth = depth + 0.005f;
        borderDepth = valueDepth = depth + 0.05f;
        value = maxValue = maxVal;
        halfMaxValue = maxValue / 2;
        align = alignSide;
        overlayTex = overlayTexture;
        borderTex = borderTexture;
        fillTex = fillTexture;
        borderColor = border_color;
        fillColor = fill_color;

        if (align == Direction.right)
        {
            bounds = new Rectangle(location.X - maxWidth, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Right - (font.charHeight / 2), bounds.Center.Y), Direction.right, valueDepth);
            printLocation = new Vector2(bounds.Right - (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }
        else if (align == Direction.forward)
        {
            bounds = new Rectangle(location.X - (maxWidth / 2), location.Y, maxWidth, height);
            halfWidth = bounds.Width / 2;
            valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, bounds.Center, Direction.forward, valueDepth);
            printLocation = bounds.Center.ToVector2();
            printValue.Update(value.ToString());
        }
        else
        {
            bounds = new Rectangle(location.X, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Left + (font.charHeight / 2), bounds.Center.Y), Direction.left, valueDepth);
            printLocation = new Vector2(bounds.Left + (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }

        overlayAnimationSpeed = 0.05f;
        currentOverlayFrame = 0;
        timeSinceLastFrame = 0;
        overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        origin = bounds.Location.ToVector2();
        valueLocation = origin * Irbis.Irbis.screenScale;
    }

    public UIElementSlider(Direction alignSide, Point location, int maxWidth, int height, float maxVal, Color border_color, Color fill_color, Texture2D fillTexture,
        Texture2D borderTexture, Texture2D overlayTexture, Font font, bool overlayDraw, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.UIElementSlider"); }
        fernt = font;
        printText = true;
        drawOverlay = overlayDraw;
        depth = drawDepth;
        value = maxValue = maxVal;
        halfMaxValue = maxValue / 2;
        align = alignSide;
        overlayTex = overlayTexture;
        borderTex = borderTexture;
        fillTex = fillTexture;
        borderColor = border_color;
        fillColor = fill_color;

        overlayDepth = depth + 0.005f;
        borderDepth = valueDepth = depth + 0.05f;

        if (align == Direction.right)
        {
            bounds = new Rectangle(location.X - maxWidth, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Right - (font.charHeight / 2), bounds.Center.Y), Direction.right, valueDepth);
            printLocation = new Vector2(bounds.Right - (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }
        else if (align == Direction.forward)
        {
            bounds = new Rectangle(location.X - (maxWidth / 2), location.Y, maxWidth, height);
            halfWidth = bounds.Width / 2;
            valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, bounds.Center, Direction.forward, valueDepth);
            printLocation = bounds.Center.ToVector2();
            printValue.Update(value.ToString());
        }
        else
        {
            bounds = new Rectangle(location.X, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Left + (font.charHeight / 2), bounds.Center.Y), Direction.left, valueDepth);
            printLocation = new Vector2(bounds.Left + (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }

        overlayAnimationSpeed = 0.05f;
        currentOverlayFrame = 0;
        timeSinceLastFrame = 0;
        overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        origin = bounds.Location.ToVector2();
        valueLocation = origin * Irbis.Irbis.screenScale;
    }

    public UIElementSlider(Direction alignSide, Point location, int maxWidth, int height, float maxVal, Color border_color, Color fill_color, Texture2D fillTexture,
        Texture2D borderTexture, Texture2D overlayTexture, Font font, bool overlayDraw, float drawOverlayDepth, float drawBorderDepth, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.UIElementSlider"); }
        fernt = font;
        printText = true;
        drawOverlay = overlayDraw;
        depth = drawDepth;
        value = maxValue = maxVal;
        halfMaxValue = maxValue / 2;
        align = alignSide;
        overlayTex = overlayTexture;
        borderTex = borderTexture;
        fillTex = fillTexture;
        borderColor = border_color;
        fillColor = fill_color;

        overlayDepth = drawOverlayDepth;
        borderDepth = valueDepth = drawBorderDepth;

        if (align == Direction.right)
        {
            bounds = new Rectangle(location.X - maxWidth, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Right - (font.charHeight / 2), bounds.Center.Y), Direction.right, valueDepth);
            printLocation = new Vector2(bounds.Right - (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }
        else if (align == Direction.forward)
        {
            bounds = new Rectangle(location.X - (maxWidth / 2), location.Y, maxWidth, height);
            halfWidth = bounds.Width / 2;
            valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, bounds.Center, Direction.forward, valueDepth);
            printLocation = bounds.Center.ToVector2();
            printValue.Update(value.ToString());
        }
        else
        {
            bounds = new Rectangle(location.X, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Left + (font.charHeight / 2), bounds.Center.Y), Direction.left, valueDepth);
            printLocation = new Vector2(bounds.Left + (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }

        overlayAnimationSpeed = 0.05f;
        currentOverlayFrame = 0;
        timeSinceLastFrame = 0;
        overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        origin = bounds.Location.ToVector2();
        valueLocation = origin * Irbis.Irbis.screenScale;
    }

    public UIElementSlider(Direction alignSide, Point location, int maxWidth, int height, float maxVal, Color border_color, Color fill_color, Texture2D fillTexture,
       Texture2D borderTexture, Texture2D overlayTexture, Font font, bool overlayDraw, bool printtext, float drawOverlayDepth, float drawBorderDepth, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.UIElementSlider"); }
        printText = printtext;
        fernt = font;
        overlayDepth = drawOverlayDepth;
        borderDepth = valueDepth = drawBorderDepth;
        drawOverlay = overlayDraw;
        depth = drawDepth;
        value = maxValue = maxVal;
        halfMaxValue = maxValue / 2;
        align = alignSide;
        overlayTex = overlayTexture;
        borderTex = borderTexture;
        fillTex = fillTexture;
        borderColor = border_color;
        fillColor = fill_color;

        if (align == Direction.right)
        {
            bounds = new Rectangle(location.X - maxWidth, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Right - (font.charHeight / 2), bounds.Center.Y), Direction.right, valueDepth);
            printLocation = new Vector2(bounds.Right - (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }
        else if (align == Direction.forward)
        {
            bounds = new Rectangle(location.X - (maxWidth / 2), location.Y, maxWidth, height);
            halfWidth = bounds.Width / 2;
            valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, bounds.Center, Direction.forward, valueDepth);
            printLocation = bounds.Center.ToVector2();
            printValue.Update(value.ToString());
        }
        else
        {
            bounds = new Rectangle(location.X, location.Y, maxWidth, height);
            valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
            printValue = new Print(bounds.Width, font, Color.White, false, new Point(bounds.Left + (font.charHeight / 2), bounds.Center.Y), Direction.left, valueDepth);
            printLocation = new Vector2(bounds.Left + (font.charHeight / 2), bounds.Center.Y);
            printValue.Update(value.ToString());
        }

        overlayAnimationSpeed = 0.05f;
        currentOverlayFrame = 0;
        timeSinceLastFrame = 0;
        overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        origin = bounds.Location.ToVector2();
        valueLocation = origin * Irbis.Irbis.screenScale;
    }

    public void Update(float v)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Update"); }
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= overlayAnimationSpeed)
        {
            timeSinceLastFrame -= overlayAnimationSpeed;
            currentOverlayFrame++;
        }
        if (currentOverlayFrame * 32 >= overlayTex.Width)
        {
            currentOverlayFrame = 0;
        }
        //overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        overlaySourceRect.X = currentOverlayFrame * 32;

        //timeSinceLastFrame = 0;


        value = v;
        if (align == Direction.right)
        {
            //valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            valueRect.X = bounds.Right - (int)((value / maxValue) * bounds.Width);
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        else if (align == Direction.forward)
        {
            //valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            valueRect.X = bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth));
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        else
        {
            //valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            //valueRect.X = bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth));
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        printValue.Update(value.ToString("0"), true);
    }

    public void UpdateValue(float v)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.UpdateValue"); }
        value = v;
        if (align == Direction.right)
        {
            valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width) * Irbis.Irbis.screenScale;
        }
        else if (align == Direction.forward)
        {
            valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth)) * Irbis.Irbis.screenScale;
        }
        valueRect.Width = (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale);
        printValue.Update(value.ToString("0"), true);
    }

    public void UpdateValue(bool overlayDraw, float v)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Update"); }
        drawOverlay = overlayDraw;

        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= overlayAnimationSpeed)
        {
            timeSinceLastFrame -= overlayAnimationSpeed;
            currentOverlayFrame++;
        }
        if (currentOverlayFrame * 32 >= overlayTex.Width)
        {
            currentOverlayFrame = 0;
        }
        overlaySourceRect.X = currentOverlayFrame * 32;


        value = v;
        if (align == Direction.right)
        {
            valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width) * Irbis.Irbis.screenScale;
        }
        else if (align == Direction.forward)
        {
            valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth)) * Irbis.Irbis.screenScale;
        }
        valueRect.Width = (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale);
        printValue.Update(value.ToString("0"), true);
    }

    public void Update(Color border_color, Color fill_color, float v)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Update"); }
        borderColor = border_color;
        fillColor = fill_color;
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= overlayAnimationSpeed)
        {
            timeSinceLastFrame -= overlayAnimationSpeed;
            currentOverlayFrame++;
        }
        if (currentOverlayFrame * 32 >= overlayTex.Width)
        {
            currentOverlayFrame = 0;
        }
        //overlaySourceRect = new Rectangle(currentOverlayFrame * 32, 0, 32, 32);
        overlaySourceRect.X = currentOverlayFrame * 32;

        //timeSinceLastFrame = 0;


        value = v;
        if (align == Direction.right)
        {
            //valueRect = new Rectangle(bounds.Right - (int)((value / maxValue) * bounds.Width), bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            valueRect.X = bounds.Right - (int)((value / maxValue) * bounds.Width);
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        else if (align == Direction.forward)
        {
            //valueRect = new Rectangle(bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth)), bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            valueRect.X = bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth));
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        else
        {
            //valueRect = new Rectangle(bounds.Left, bounds.Top, (int)((value / maxValue) * bounds.Width), bounds.Height);
            //valueRect.X = bounds.Left + (int)(halfWidth - ((value / maxValue) * halfWidth));
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }
        printValue.Update(value.ToString("0"), true);
    }

    public bool Contains(MouseState mouseState)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Contains"); }
        return bounds.Contains(mouseState.Position.X, mouseState.Position.Y);
    }

    public bool Pressed(MouseState mouseState)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Pressed"); }
        if (bounds.Contains(mouseState.Position.X, mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed)
        {
            return true;
        }
        return false;
    }

    public bool Pressed(MouseState mouseState, MouseState previousMouseState)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Pressed"); }
        if (bounds.Contains(mouseState.Position.X, mouseState.Position.Y) && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
        {
            return true;
        }
        return false;
    }


    public void Draw(SpriteBatch sb)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementSlider.Draw"); }
        if (drawOverlay) { sb.Draw(overlayTex, origin * Irbis.Irbis.screenScale, overlaySourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, overlayDepth); }
        sb.Draw(fillTex, valueLocation, valueRect, fillColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, depth);
        if (printText) { printValue.Draw(sb, (printLocation * Irbis.Irbis.screenScale).ToPoint()); }
        RectangleBorder.Draw(sb, bounds, borderColor, true);
    }
}
