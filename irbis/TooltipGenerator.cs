using Irbis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TooltipGenerator
{
    SpriteBatch spriteBatch;
    Texture2D bg;
    Texture2D top;
    Texture2D bottom;
    Texture2D left;
    Texture2D right;
    Texture2D topleft;
    Texture2D topright;
    Texture2D bottomleft;
    Texture2D bottomright;
    Texture2D downarrow;


    public TooltipGenerator (Game game)
    {
        spriteBatch = new SpriteBatch(Irbis.Irbis.game.GraphicsDevice);
        bg = Irbis.Irbis.LoadTexture("menu background");
        top = Irbis.Irbis.LoadTexture("menu border top");
        bottom = Irbis.Irbis.LoadTexture("menu border bottom");
        left = Irbis.Irbis.LoadTexture("menu border left");
        right = Irbis.Irbis.LoadTexture("menu border right");
        topleft = Irbis.Irbis.LoadTexture("menu corner top left");
        topright = Irbis.Irbis.LoadTexture("menu corner top right");
        bottomleft = Irbis.Irbis.LoadTexture("menu corner bottom left");
        bottomright = Irbis.Irbis.LoadTexture("menu corner bottom right");
        downarrow = Irbis.Irbis.LoadTexture("popup arrow");
    }

    public Tooltip CreateTooltip(string Text, Point Location, float depth)
    {
        Print popup = new Print(Irbis.Irbis.halfResolution.X, Irbis.Irbis.font, Color.White, false, Location, Direction.Forward, depth);
        popup.Update(Text, true);
        Point size = popup.PrintSize(Text);
        size.X += (topleft.Width + bottomright.Width) * Irbis.Irbis.textScale;
        size.Y += (topleft.Height + bottomright.Height);

        return new Tooltip(popup, CreateTooltipTexture(size, false), Location);
    }

    public Texture2D CreateTooltipTexture(Point Size, bool screenScale)
    {
        RenderTarget2D renderTarget = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, Size.X, Size.Y);
        Vector2 drawLocation = Vector2.Zero;
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, Matrix.Identity);

        if (screenScale)
        {
            //Size.X += (int)((topleft.Width + bottomright.Width) * (Irbis.Irbis.screenScale - 1f));
            //Size.Y += (int)((topleft.Height + bottomright.Height) * (Irbis.Irbis.screenScale - 1f));
            spriteBatch.Draw(topleft, drawLocation, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            drawLocation.X = Size.X - (topright.Width * Irbis.Irbis.screenScale);
            spriteBatch.Draw(topright, drawLocation, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            drawLocation.X = 0;
            drawLocation.Y = Size.Y - (bottomleft.Width * Irbis.Irbis.screenScale);
            spriteBatch.Draw(bottomleft, drawLocation, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            drawLocation.X = Size.X - (bottomright.Width * Irbis.Irbis.screenScale);
            spriteBatch.Draw(bottomright, drawLocation, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(top, new Vector2((topleft.Width * Irbis.Irbis.screenScale), 0), new Rectangle(0, 0, (int)(((Size.X / Irbis.Irbis.screenScale) - (topleft.Width + topright.Width))), top.Height), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(left, new Vector2(0, (topleft.Height * Irbis.Irbis.screenScale)), new Rectangle(0, 0, left.Width, (int)(((Size.Y / Irbis.Irbis.screenScale) - (topleft.Height + bottomleft.Height)))), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(bg, new Vector2((topleft.Width * Irbis.Irbis.screenScale), (topleft.Height * Irbis.Irbis.screenScale)), new Rectangle(0, 0, (int)(((Size.X / Irbis.Irbis.screenScale) - (topleft.Width + bottomright.Width))), (int)(((Size.Y / Irbis.Irbis.screenScale) - (topleft.Height + bottomright.Height)))), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(right, new Vector2(Size.X - (right.Width * Irbis.Irbis.screenScale), (topright.Height * Irbis.Irbis.screenScale)), new Rectangle(0, 0, right.Width, (int)(((Size.Y / Irbis.Irbis.screenScale) - (topright.Height + bottomright.Height)))), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(bottom, new Vector2((bottomright.Width * Irbis.Irbis.screenScale), Size.Y - (bottom.Height * Irbis.Irbis.screenScale)), new Rectangle(0, 0, (int)(((Size.X / Irbis.Irbis.screenScale) - (topleft.Width + topright.Width))), bottom.Height), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.5f);
        }
        else
        {
            spriteBatch.Draw(topleft, drawLocation, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
            drawLocation.X = Size.X - topright.Width;
            spriteBatch.Draw(topright, drawLocation, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
            drawLocation.X = 0;
            drawLocation.Y = Size.Y - bottomleft.Width;
            spriteBatch.Draw(bottomleft, drawLocation, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
            drawLocation.X = Size.X - bottomright.Width;
            spriteBatch.Draw(bottomright, drawLocation, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(top, new Vector2(topleft.Width, 0), new Rectangle(0, 0, Size.X - (topleft.Width + topright.Width), top.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(left, new Vector2(0, topleft.Height), new Rectangle(0, 0, left.Width, Size.Y - (topleft.Height + bottomright.Height)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(bg, new Vector2(topleft.Width, topleft.Height), new Rectangle(0, 0, Size.X - (topleft.Width + bottomright.Width), Size.Y - (topleft.Height + bottomright.Height)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(right, new Vector2(Size.X - right.Width, topright.Height), new Rectangle(0, 0, right.Width, Size.Y - (topright.Height + bottomright.Height)), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.Draw(bottom, new Vector2(bottomright.Width, Size.Y - bottom.Height), new Rectangle(0, 0, Size.X - (bottomleft.Width + bottomright.Width), bottom.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
        }


        //old code that would (almost) draw a down arrow at the bottom of the tooptip
        // spriteBatch.Draw(bottom, new Rectangle(bottomright.Width, height - bottom.Height, (width / 2) - (bottomleft.Width + (downarrow.Width / 2)), bottom.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        // spriteBatch.Draw(downarrow, new Rectangle(((width / 2) + bottomright.Width) - (bottomleft.Width + (downarrow.Width / 2)), height - bottom.Height, downarrow.Width, downarrow.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        // spriteBatch.Draw(bottom, new Rectangle(((width / 2) + bottomright.Width) - (bottomleft.Width) + (downarrow.Width), height - bottom.Height, (width / 2) - (bottomright.Width + (downarrow.Width / 2)), bottom.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.5f);
        spriteBatch.End();
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(null);

        return (Texture2D)renderTarget;
    }
}
