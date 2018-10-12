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
public class Tooltip
{
    public Print Text
    {
        get
        { return text; }
    }
    Texture2D texture;
    [DataMember]
    private string texname;

    [DataMember]
    Rectangle displayRectangle;
    [DataMember]
    float textureDepth;
    [DataMember]
    private Print text;

    public Tooltip(Print Text, Texture2D Texture, Point Location)
    {
        text = Text;
        texture = Texture;
        textureDepth = text.depth - 0.001f;
        displayRectangle = new Rectangle(Location.X - (texture.Width / 2), (int)(Location.Y - (text.characterHeight + Irbis.Irbis.screenScale)), texture.Width, texture.Height);
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
    }

    public override string ToString()
    { return "text:" + text.ToString() + " texture:{Width:" + texture.Width + " Height:" + texture.Height + "}" + " displayRectangle:" + displayRectangle; }

    public void Draw(SpriteBatch sb)
    {
        text.Draw(sb);
        sb.Draw(texture, displayRectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, textureDepth);
    }
}
