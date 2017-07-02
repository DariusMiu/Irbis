using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public struct RectangleBorder
{

    public Rectangle leftBorder;
    public Rectangle rightBorder;
    public Rectangle topBorder;
    public Rectangle bottomBorder;
    public Color color;
    public float depth;

    public RectangleBorder(Rectangle refrect, Color bColor, float borderDepth)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.RectangleBorder"); }
        color = bColor;
        leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        bottomBorder = new Rectangle(refrect.Left, refrect.Bottom -1, refrect.Width, 1);
        depth = borderDepth;
    }

    public RectangleBorder(Rectangle refrect, Color bColor)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.RectangleBorder"); }
        color = bColor;
        leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        bottomBorder = new Rectangle(refrect.Left, refrect.Bottom - 1, refrect.Width, 1);
        depth = 1f;
    }

    public void Update(Rectangle refrect, Color bColor)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Update"); }
        color = bColor;
        //depth = borderDepth;
        //leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        leftBorder.X = refrect.Left;
        leftBorder.Y = refrect.Top;
        leftBorder.Height = refrect.Height;
        //rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        rightBorder.X = refrect.Right - 1;
        rightBorder.Y = refrect.Top;
        rightBorder.Height = refrect.Height;
        //topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        topBorder.X = refrect.Left;
        topBorder.Y = refrect.Top;
        topBorder.Width = refrect.Width;
        //bottomBorder = new Rectangle(refrect.Left, refrect.Bottom - 1, refrect.Width, 1);
        bottomBorder.X = refrect.Left;
        bottomBorder.Y = refrect.Bottom - 1;
        bottomBorder.Width = refrect.Width;
    }

    public void Update(Rectangle refrect, Color bColor, float borderDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Update"); }
        color = bColor;
        depth = borderDepth;
        //leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        leftBorder.X = refrect.Left;
        leftBorder.Y = refrect.Top;
        leftBorder.Height = refrect.Height;
        //rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        rightBorder.X = refrect.Right - 1;
        rightBorder.Y = refrect.Top;
        rightBorder.Height = refrect.Height;
        //topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        topBorder.X = refrect.Left;
        topBorder.Y = refrect.Top;
        topBorder.Width = refrect.Width;
        //bottomBorder = new Rectangle(refrect.Left, refrect.Bottom - 1, refrect.Width, 1);
        bottomBorder.X = refrect.Left;
        bottomBorder.Y = refrect.Bottom - 1;
        bottomBorder.Width = refrect.Width;
    }

    public void Draw(SpriteBatch sb, Texture2D tex)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        sb.Draw(tex, leftBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, rightBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, topBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, bottomBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
    }

    public void Draw(SpriteBatch sb, Texture2D tex, Rectangle refrect)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        //leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        leftBorder.X = refrect.Left;
        leftBorder.Y = refrect.Top;
        leftBorder.Height = refrect.Height;
        //rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        rightBorder.X = refrect.Right - 1;
        rightBorder.Y = refrect.Top;
        rightBorder.Height = refrect.Height;
        //topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        topBorder.X = refrect.Left;
        topBorder.Y = refrect.Top;
        topBorder.Width = refrect.Width;
        //bottomBorder = new Rectangle(refrect.Left, refrect.Bottom - 1, refrect.Width, 1);
        bottomBorder.X = refrect.Left;
        bottomBorder.Y = refrect.Bottom - 1;
        bottomBorder.Width = refrect.Width;
        sb.Draw(tex, leftBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, rightBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, topBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, bottomBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
    }

    public void Draw(SpriteBatch sb, Texture2D tex, float borderDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        //depth = borderDepth;
        sb.Draw(tex, leftBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, borderDepth);
        sb.Draw(tex, rightBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, borderDepth);
        sb.Draw(tex, topBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, borderDepth);
        sb.Draw(tex, bottomBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, borderDepth);
    }

    public void Draw(SpriteBatch sb, Texture2D tex, Rectangle refrect, float borderDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("RectangleBorder.Draw"); }
        //leftBorder = new Rectangle(refrect.Left, refrect.Top, 1, refrect.Height);
        leftBorder.X = refrect.Left;
        leftBorder.Y = refrect.Top;
        leftBorder.Height = refrect.Height;
        //rightBorder = new Rectangle(refrect.Right - 1, refrect.Top, 1, refrect.Height);
        rightBorder.X = refrect.Right - 1;
        rightBorder.Y = refrect.Top;
        rightBorder.Height = refrect.Height;
        //topBorder = new Rectangle(refrect.Left, refrect.Top, refrect.Width, 1);
        topBorder.X = refrect.Left;
        topBorder.Y = refrect.Top;
        topBorder.Width = refrect.Width;
        //bottomBorder = new Rectangle(refrect.Left, refrect.Bottom - 1, refrect.Width, 1);
        bottomBorder.X = refrect.Left;
        bottomBorder.Y = refrect.Bottom - 1;
        bottomBorder.Width = refrect.Width;
        depth = borderDepth;
        sb.Draw(tex, leftBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, rightBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, topBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        sb.Draw(tex, bottomBorder, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
    }
}
