using Irbis;
using System;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

[DataContract]
public class Square : ICollisionObject
{
    public Rectangle Collider
    {
        get
        { return collider; }
        set
        { collider = value; }
    }
    private Rectangle collider;
    public Rectangle? InitialCollider
    {
        get
        { return initialCollider; }
    }
    public Vector2 Position
    {
        get
        { return position; }
        set
        {
            position.X = value.X;
            position.Y = value.Y;
            if (initialCollider != null)
            { collider.Location = (position + ((Rectangle)initialCollider).Location.ToVector2()).ToPoint(); }
            else
            { collider.Location = position.ToPoint(); }
        }
    }
    private Vector2 position;
    public Vector2 Velocity
    {
        get
        { return velocity; }
        set
        { velocity = value; }
    }
    private Vector2 velocity;
    public float rotation;
    public Point InitialPosition
    {
        get
        { return initialPosition; }
    }

    public Texture2D texture;
    [DataMember]
    private string texname;

    [DataMember]
    private Point initialPosition;
    [DataMember]
    private Rectangle? initialCollider;
    [DataMember]
    public float depth;
    [DataMember]
    public Color color;
    [DataMember]
    public float scale;
    [DataMember]
    public bool draw = true;
    [DataMember]
    private Vector2 origin;

    public Vector2 Origin
    {
        get
        { return origin; }
    }
    public static Square Empty
    {
        get
        { return new Square(Irbis.Irbis.nullTex, Color.Transparent, Point.Zero, null, Vector2.Zero, 0, null); }
    }

    public Square(Texture2D Texture, Color drawColor, Point initialPos, Rectangle? Collider, Vector2 Origin, float Scale, float? DrawDepth)
    {
        origin = Origin;
        texture = Texture;
        scale = Scale;
        if (DrawDepth != null)
        { depth = (float)DrawDepth; }
        else
        { draw = false; }
        initialPosition = initialPos;
        position = initialPos.ToVector2();
        color = drawColor;

        if (Collider == null)
        {
            initialCollider = null;
            collider = new Rectangle(initialPos, texture.Bounds.Size);
        }
        else
        {
            initialCollider = Collider;
            collider = new Rectangle(initialPos + ((Rectangle)Collider).Location, ((Rectangle)Collider).Size);
        }
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

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        { RectangleBorder.Draw(sb, Collider, Color.SaddleBrown, true); }
        if (draw)
        { sb.Draw(texture, position * scale, null, color, rotation, origin, scale, SpriteEffects.None, depth); }
    }
}
