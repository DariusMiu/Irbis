using Irbis;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

[Serializable]
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
    private Rectangle? initialCollider;

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

    public Texture2D texture;
    public float depth;
    public Color color;
    public float scale;
    public bool draw = true;
    public Point InitialPosition
    {
        get
        { return initialPosition; }
    }
    private Point initialPosition;

    public Vector2 Origin
    {
        get
        { return origin; }
    }
    private Vector2 origin;

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

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        { RectangleBorder.Draw(sb, Collider, Color.Green, true); }
        if (draw)
        { sb.Draw(texture, position * scale, null, color, rotation, origin, scale, SpriteEffects.None, depth); }
    }
}
