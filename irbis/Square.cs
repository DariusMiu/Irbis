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
        {
            return collider;
        }
        set
        {
            collider = value;
        }
    }
    private Rectangle collider;

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position.X = value.X - 16;
            position.Y = value.Y - 16;
        }
    }
    private Vector2 position;

    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }
    private Vector2 velocity;

    public Texture2D texture;
    public bool drawTex;
    public float depth;
    public Color color;
    private float scale;
    public bool useExactPixels;

    public Square(Texture2D Texture, Point InitialPosition, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = drawDepth;
        texture = Texture;
        position = InitialPosition.ToVector2();
        position.X = position.X * 16;
        position.Y = position.Y * 16;
        drawTex = true;
        color = Color.White;
        scale = Irbis.Irbis.screenScale;
        collider = new Rectangle(InitialPosition.X, InitialPosition.Y, 16, 16);
        useExactPixels = false;
    }

    public Square(Texture2D Texture, Point InitialPosition, float Scale, bool CenterTextureOnPosition, float DrawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        depth = DrawDepth;
        texture = Texture;
        drawTex = true;
        color = Color.White;
        scale = Scale;

        if (CenterTextureOnPosition)
        {
            position = ((InitialPosition.ToVector2() - ((texture.Bounds.Size.ToVector2() / 2f))).ToPoint()).ToVector2();
        }
        else
        {
            position = InitialPosition.ToVector2();
        }
        collider = new Rectangle(InitialPosition, texture.Bounds.Size);

        Irbis.Irbis.WriteLine("InitialPosition:" + InitialPosition + " Position:" + position + " texture.Bounds:" + texture.Bounds);
    }

    public Square(Texture2D t, Color drawColor, Point initialPos, Point Size, bool hascollider, bool UseExactPixels, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Square.Square"); }
        scale = Irbis.Irbis.screenScale;
        depth = drawDepth;
        position = initialPos.ToVector2();
        color = drawColor;

        if (hascollider) { collider = new Rectangle((int)position.X, (int)position.Y, (int)(16 * scale), (int)(16 * scale)); }
        else { collider = Rectangle.Empty; }

        useExactPixels = UseExactPixels;

        RenderTarget2D renderTarget = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, Size.X, Size.Y);
        SpriteBatch spriteBatch = new SpriteBatch(Irbis.Irbis.game.GraphicsDevice);
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, Matrix.Identity);
        spriteBatch.Draw(t, new Rectangle(0, 0, Size.X, Size.Y), Color.White);
        spriteBatch.End();
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(null);

        texture = (Texture2D)renderTarget;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(texture, position * scale, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, depth);
    }
}
