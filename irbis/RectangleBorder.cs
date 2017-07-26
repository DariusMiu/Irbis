using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class RectangleBorder
{
    private static Vector2 lBorder = Vector2.One;
    private static Vector2 rBorder = Vector2.One;
    private static Vector2 tBorder = Vector2.One;
    private static Vector2 bBorder = Vector2.One;
    private static Vector2 hScale = Vector2.One;
    private static Vector2 vScale = Vector2.One;

    public static void Draw(SpriteBatch sb, Rectangle referenceRectangle, Color borderColor, float depth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        lBorder.X = referenceRectangle.Left;
        lBorder.Y = referenceRectangle.Top;
        rBorder.X = referenceRectangle.Right;
        rBorder.Y = referenceRectangle.Top;
        tBorder.X = referenceRectangle.Left;
        tBorder.Y = referenceRectangle.Top;
        bBorder.X = referenceRectangle.Left;
        bBorder.Y = referenceRectangle.Bottom;
        hScale.X = referenceRectangle.Width;
        vScale.Y = referenceRectangle.Height;
        hScale.Y = vScale.X = 1;
        hScale.X = referenceRectangle.Width;
        vScale.Y = referenceRectangle.Height;
        rBorder.X = referenceRectangle.Right - vScale.X;
        bBorder.Y = referenceRectangle.Bottom - vScale.X;
        sb.Draw(Irbis.Irbis.nullTex, lBorder, null, borderColor, 0f, Vector2.Zero, vScale, SpriteEffects.None, 0.9f);
        sb.Draw(Irbis.Irbis.nullTex, rBorder, null, borderColor, 0f, Vector2.Zero, vScale, SpriteEffects.None, 0.9f);
        sb.Draw(Irbis.Irbis.nullTex, tBorder, null, borderColor, 0f, Vector2.Zero, hScale, SpriteEffects.None, 0.9f);
        sb.Draw(Irbis.Irbis.nullTex, bBorder, null, borderColor, 0f, Vector2.Zero, hScale, SpriteEffects.None, 0.9f);
    }

    public static void Draw(SpriteBatch sb, Rectangle referenceRectangle, Color borderColor, bool screenScale)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        lBorder.X = referenceRectangle.Left;
        lBorder.Y = referenceRectangle.Top;
        rBorder.Y = referenceRectangle.Top;
        tBorder.X = referenceRectangle.Left;
        tBorder.Y = referenceRectangle.Top;
        bBorder.X = referenceRectangle.Left;
        hScale.X = referenceRectangle.Width;
        vScale.Y = referenceRectangle.Height;
        
        if (screenScale)
        {
            hScale.Y = vScale.X = 1 / Irbis.Irbis.screenScale;
            rBorder.X = referenceRectangle.Right - vScale.X;
            bBorder.Y = referenceRectangle.Bottom - vScale.X;
            sb.Draw(Irbis.Irbis.nullTex, lBorder * Irbis.Irbis.screenScale, null, borderColor, 0f, Vector2.Zero, vScale * Irbis.Irbis.screenScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, rBorder * Irbis.Irbis.screenScale, null, borderColor, 0f, Vector2.Zero, vScale * Irbis.Irbis.screenScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, tBorder * Irbis.Irbis.screenScale, null, borderColor, 0f, Vector2.Zero, hScale * Irbis.Irbis.screenScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, bBorder * Irbis.Irbis.screenScale, null, borderColor, 0f, Vector2.Zero, hScale * Irbis.Irbis.screenScale, SpriteEffects.None, 0.9f);
        }
        else
        {
            hScale.Y = vScale.X = 1;
            rBorder.X = referenceRectangle.Right - vScale.X;
            bBorder.Y = referenceRectangle.Bottom - vScale.X;
            sb.Draw(Irbis.Irbis.nullTex, lBorder, null, borderColor, 0f, Vector2.Zero, vScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, rBorder, null, borderColor, 0f, Vector2.Zero, vScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, tBorder, null, borderColor, 0f, Vector2.Zero, hScale, SpriteEffects.None, 0.9f);
            sb.Draw(Irbis.Irbis.nullTex, bBorder, null, borderColor, 0f, Vector2.Zero, hScale, SpriteEffects.None, 0.9f);
        }
    }
}
