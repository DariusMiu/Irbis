using Irbis;
using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public class Doodad
{
    public float depth;
    public Texture2D tex;
    public Tooltip tooltip;
    public Vector2 position;

    private Texture2D circle;
    private bool drawTooltip;
    private string text;

    public int ToolTipDistance
    {
        get
        { return toolTipDistance; }
        set
        {
            toolTipDistance = value;
            toolTipDistanceSquared = value * value;
            circle = Irbis.Irbis.GenerateCircle((int)(toolTipDistance * Irbis.Irbis.screenScale), Color.Indigo);
            origin = new Vector2(toolTipDistance);
        }
    }
    private int toolTipDistance;
    private int toolTipDistanceSquared;
    private Vector2 origin;

    public Doodad(Texture2D Texture, Point Position, string Text, int Distance, float Depth)
    {
        tex = Texture;
        depth = Depth;
        text = Text;
        ToolTipDistance = Distance;
        position = Position.ToVector2();
        origin = tex.Bounds.Size.ToVector2() / 2;
        if (!string.IsNullOrWhiteSpace(text))
        { tooltip = Irbis.Irbis.tooltipGenerator.CreateTooltip(text, new Point((int)(position.X * Irbis.Irbis.screenScale), (int)((position.Y - tex.Height / 2 - Irbis.Irbis.textScale * 2) * Irbis.Irbis.screenScale)), 0.7f); }
    }

    public void Update(Player player)
    {
        drawTooltip = (Irbis.Irbis.DistanceSquared(player.Collider, position) <= toolTipDistanceSquared);
    }

    public void Light(SpriteBatch sb, bool UseColor)
    { }

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 2)
        { sb.Draw(circle, position * Irbis.Irbis.screenScale, null, Color.White, 0f, new Vector2(ToolTipDistance * Irbis.Irbis.screenScale), 1, SpriteEffects.None, depth + 0.001f); }
        sb.Draw(tex, position * Irbis.Irbis.screenScale, null, Color.White, 0f, origin, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
        if (drawTooltip && tooltip != null)
        { tooltip.Draw(sb); }
    }
}
