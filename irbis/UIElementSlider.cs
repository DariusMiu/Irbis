using Irbis;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class UIElementSlider
{
    public Rectangle bounds;
    Rectangle valueRect;
    Vector2 valueLocation;
    Vector2 origin;

    public Texture2D backgroundTexture;
    public Texture2D overlayTexture;
    public Texture2D fillTexture;

    public float overlayAnimationSpeed;
    Rectangle overlaySourceRect;
    int currentOverlayFrame;
    //float Irbis.Irbis.DeltaTime;
    float timeSinceLastFrame;


    public float value;
    public float maxValue;
    float halfWidth;
    float halfMaxValue;
    Direction align;

    float fillDepth;
    float backgroundDepth;
    float overlayDepth;
    float textDepth;

    Color fillColor;
    Color backgroundColor;
    Color overlayColor;
    Color borderColor;

    public bool drawOverlay;
    public bool drawBorder;
    public bool drawBackground;
    public bool screenScale;
    bool drawText;

    Print printValue;
    public Vector2 printLocation;

    public UIElementSlider(Direction AlignSide, Rectangle Bounds, Point TextOffset, float MaxValue, Color FillColor, Nullable<Color> BackgroundColor, Nullable<Color> OverlayColor, Nullable<Color> BorderColor, Texture2D FillTexture,
       Texture2D BackgroundTexture, Texture2D OverlayTexture, bool DrawBorder, Nullable<Font> PrintFont, bool ScreenScale, float FillDepth, float BackgroundDepth, float OverlayDepth, float TextDepth)
    {
        overlayDepth = OverlayDepth;
        textDepth = TextDepth;
        fillDepth = FillDepth;
        backgroundDepth = BackgroundDepth;
        value = maxValue = MaxValue;
        halfMaxValue = maxValue / 2;
        align = AlignSide;
        screenScale = ScreenScale;

        fillColor = FillColor;
        if (BackgroundColor != null)
        { backgroundColor = (Color)BackgroundColor; }
        if (OverlayColor != null)
        { overlayColor = (Color)OverlayColor; }
        if (BorderColor != null)
        { borderColor = (Color)BorderColor; }

        fillTexture = FillTexture;
        if (BackgroundTexture != null)
        {
            backgroundTexture = BackgroundTexture;
            drawBackground = true;
        }
        else
        { drawBackground = false; }
        if (OverlayTexture != null)
        {
            overlayTexture = OverlayTexture;
            drawOverlay = true;
        }
        else
        { drawOverlay = false; }
        if (PrintFont != null)
        {
            drawText = true;
        }
        else
        { drawText = false; }
        drawBorder = DrawBorder;


        if (ScreenScale)
        {
            if (align == Direction.right)
            {
                bounds = new Rectangle(Bounds.X - Bounds.Width, Bounds.Y, Bounds.Width, Bounds.Height);
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, new Point(bounds.Right - (((Font)PrintFont).charHeight / 2), bounds.Center.Y), Direction.right, textDepth);
                    printLocation = new Vector2(bounds.Right - (((Font)PrintFont).charHeight / 2), bounds.Center.Y) + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            else if (align == Direction.forward)
            {
                bounds = new Rectangle(Bounds.X - (Bounds.Width / 2), Bounds.Y, Bounds.Width, Bounds.Height);
                halfWidth = bounds.Width / 2;
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, bounds.Center, Direction.forward, textDepth);
                    printLocation = bounds.Center.ToVector2() + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            else
            {
                bounds = Bounds;
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale), (int)(bounds.Height * Irbis.Irbis.screenScale));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, new Point(bounds.Left + (((Font)PrintFont).charHeight / 2), bounds.Center.Y), Direction.left, textDepth);
                    printLocation = new Vector2(bounds.Left + (((Font)PrintFont).charHeight / 2), bounds.Center.Y) + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            valueLocation = origin = bounds.Location.ToVector2() * Irbis.Irbis.screenScale;
        }
        else
        {
            if (align == Direction.right)
            {
                bounds = new Rectangle(Bounds.X - Bounds.Width, Bounds.Y, Bounds.Width, Bounds.Height);
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width), (int)(bounds.Height));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, new Point(bounds.Right - (((Font)PrintFont).charHeight / 2), bounds.Center.Y), Direction.right, textDepth);
                    printLocation = new Vector2(bounds.Right - (((Font)PrintFont).charHeight / 2), bounds.Center.Y) + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            else if (align == Direction.forward)
            {
                bounds = new Rectangle(Bounds.X - (Bounds.Width / 2), Bounds.Y, Bounds.Width, Bounds.Height);
                halfWidth = bounds.Width / 2;
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width), (int)(bounds.Height));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, bounds.Center, Direction.forward, textDepth);
                    printLocation = bounds.Center.ToVector2() + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            else
            {
                bounds = Bounds;
                valueRect = new Rectangle(0, 0, (int)((value / maxValue) * bounds.Width), (int)(bounds.Height));
                if (drawText)
                {
                    printValue = new Print(bounds.Width, ((Font)PrintFont), Color.White, false, new Point(bounds.Left + (((Font)PrintFont).charHeight / 2), bounds.Center.Y), Direction.left, textDepth);
                    printLocation = new Vector2(bounds.Left + (((Font)PrintFont).charHeight / 2), bounds.Center.Y) + TextOffset.ToVector2();
                    printValue.Update(value.ToString());
                }
            }
            valueLocation = origin = bounds.Location.ToVector2();
        }

        overlayAnimationSpeed = 0.05f;
        currentOverlayFrame = 0;
        timeSinceLastFrame = 0;
        overlaySourceRect = new Rectangle(currentOverlayFrame * bounds.Width, 0, bounds.Width, bounds.Height);
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
        if (currentOverlayFrame * 150 >= overlayTexture.Width)
        {
            currentOverlayFrame = 0;
        }
        overlaySourceRect.X = currentOverlayFrame * 150;

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
        if (currentOverlayFrame * 150 >= overlayTexture.Width)
        {
            currentOverlayFrame = 0;
        }
        overlaySourceRect.X = currentOverlayFrame * 150;

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

    public void UpdateValue(float Value)
    {
        value = Value;
        if (screenScale)
        {
            if (align == Direction.right)
            {
                valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width) * Irbis.Irbis.screenScale;
            }
            else if (align == Direction.forward)
            {
                valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth)) * Irbis.Irbis.screenScale;
            }
            valueRect.Width = (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale);
        }
        else
        {
            if (align == Direction.right)
            {
                valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width);
            }
            else if (align == Direction.forward)
            {
                valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth));
            }
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }

        if (drawText)
        { printValue.Update(value.ToString("0"), true); }
    }

    public void UpdateValue(float Value, bool DrawOverlay)
    {
        drawOverlay = DrawOverlay;
        if (drawOverlay)
        {
            timeSinceLastFrame += Irbis.Irbis.DeltaTime;
            if (timeSinceLastFrame >= overlayAnimationSpeed)
            {
                timeSinceLastFrame -= overlayAnimationSpeed;
                currentOverlayFrame++;
            }
            if (currentOverlayFrame * 150 >= overlayTexture.Width)
            {
                currentOverlayFrame = 0;
            }
            overlaySourceRect.X = currentOverlayFrame * 150;
        }

        value = Value;

        if (screenScale)
        {
            if (align == Direction.right)
            {
                valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width) * Irbis.Irbis.screenScale;
            }
            else if (align == Direction.forward)
            {
                valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth)) * Irbis.Irbis.screenScale;
            }
            valueRect.Width = (int)((value / maxValue) * bounds.Width * Irbis.Irbis.screenScale);
        }
        else
        {
            if (align == Direction.right)
            {
                valueLocation.X = (bounds.Right - (value / maxValue) * bounds.Width);
            }
            else if (align == Direction.forward)
            {
                valueLocation.X = (bounds.Left + halfWidth - ((value / maxValue) * halfWidth));
            }
            valueRect.Width = (int)((value / maxValue) * bounds.Width);
        }

        if (drawText)
        { printValue.Update(value.ToString("0"), true); }
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
        if (screenScale)
        {
            if (drawOverlay) { sb.Draw(overlayTexture, origin, overlaySourceRect, overlayColor, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, overlayDepth); }
            sb.Draw(fillTexture, valueLocation, valueRect, fillColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, fillDepth);
            if (drawText) { printValue.Draw(sb, (printLocation * Irbis.Irbis.screenScale).ToPoint()); }
            if (drawBorder) { RectangleBorder.Draw(sb, bounds, borderColor, true); }
            if (drawBackground) { /* draw background */ }
        }
        else
        {
            if (drawOverlay) { sb.Draw(overlayTexture, origin, overlaySourceRect, overlayColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, overlayDepth); }
            sb.Draw(fillTexture, valueLocation, valueRect, fillColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, fillDepth);
            if (drawText) { printValue.Draw(sb, (printLocation).ToPoint()); }
            if (drawBorder) { RectangleBorder.Draw(sb, bounds, borderColor, false); }
            if (drawBackground) { /* draw background */ }
        }
    }
}
