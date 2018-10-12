using Irbis;
using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

[DataContract]
public class Doodad
{
    private Texture2D circle;
    public Texture2D texture;
    [DataMember]
    private string texname;

    [DataMember]
    public float depth;
    //[DataMember]
    public Tooltip tooltip;
    [DataMember]
    public Vector2 position;
    [DataMember]
    private bool drawTooltip;
    [DataMember]
    private string text;
    [DataMember]
    private int toolTipDistance;

    private int toolTipDistanceSquared;
    private Vector2 origin;

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

    public Doodad(Texture2D Texture, Point Position, string Text, int Distance, float Depth)
    {
        texture = Texture;
        depth = Depth;
        text = Text;
        ToolTipDistance = Distance;
        position = Position.ToVector2();
        origin = texture.Bounds.Size.ToVector2() / 2;
        if (!string.IsNullOrWhiteSpace(text))
        { tooltip = Irbis.Irbis.tooltipGenerator.CreateTooltip(text, new Point((int)(position.X * Irbis.Irbis.screenScale), (int)((position.Y - texture.Height / 2 - Irbis.Irbis.textScale * 2) * Irbis.Irbis.screenScale)), 0.7f); }
    }

    [OnSerializing]
    void OnSerializing(StreamingContext c)
    { texname = texture.Name; }

    [OnSerialized]
    void OnSerialized(StreamingContext c)
    { texname = null; }

    [OnDeserializing]
    void OnDeserializing(StreamingContext c)
    { }

    [OnDeserialized]
    void OnDeserialized(StreamingContext c)
    {
        texture = Irbis.Irbis.LoadTexture(texname);
        texname = null;

        if (!string.IsNullOrWhiteSpace(text))
        { tooltip = Irbis.Irbis.tooltipGenerator.CreateTooltip(text, new Point((int)(position.X * Irbis.Irbis.screenScale), (int)((position.Y - texture.Height / 2 - Irbis.Irbis.textScale * 2) * Irbis.Irbis.screenScale)), 0.7f); }

        //setting circle and squared and origin variables
        ToolTipDistance = toolTipDistance;
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
        sb.Draw(texture, position * Irbis.Irbis.screenScale, null, Color.White, 0f, origin, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
        if (drawTooltip && tooltip != null)
        { tooltip.Draw(sb); }
    }
}
