using Irbis;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TooltipGenerator
{
    SpriteBatch spriteBatch;
    uint[][] bg;
    uint[][] top;
    uint[][] bottom;
    uint[][] left;
    uint[][] right;
    uint[][] topleft;
    uint[][] topright;
    uint[][] bottomleft;
    uint[][] bottomright;
    uint[][] downarrow;


    public TooltipGenerator (Game game)
    {
        spriteBatch = new SpriteBatch(Irbis.Irbis.game.GraphicsDevice);
        bg = Irbis.Irbis.LoadPNGasArray("menu background");
        top = Irbis.Irbis.LoadPNGasArray("menu border top");
        bottom = Irbis.Irbis.LoadPNGasArray("menu border bottom");
        left = Irbis.Irbis.LoadPNGasArray("menu border left");
        right = Irbis.Irbis.LoadPNGasArray("menu border right");
        topleft = Irbis.Irbis.LoadPNGasArray("menu corner top left");
        topright = Irbis.Irbis.LoadPNGasArray("menu corner top right");
        bottomleft = Irbis.Irbis.LoadPNGasArray("menu corner bottom left");
        bottomright = Irbis.Irbis.LoadPNGasArray("menu corner bottom right");
        downarrow = Irbis.Irbis.LoadPNGasArray("popup arrow");
    }

    public Tooltip CreateTooltip(string Text, Point Location, float depth)
    {
        Print popup = new Print(Irbis.Irbis.halfResolution.X, Irbis.Irbis.font, Color.White, false, Location, Direction.Forward, depth);
        popup.Update(Text, true);
        Point size = popup.PrintSize(Text);
        size.X += (int)(topleft[1][0] + bottomright[1][0]) * Irbis.Irbis.textScale;
        size.Y += (int)(topleft[1][1] + bottomright[1][1]);

        return new Tooltip(popup, CreateTooltipTexture(size, false), Location);
    }

    public Texture2D CreateTooltipTexture(Point Size, bool screenScale)
    {
        /*RenderTarget2D renderTarget = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, Size.X, Size.Y);
        Vector2 drawLocation = Vector2.Zero;
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

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
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(null);*/

        uint[] pixels = new uint[Size.X * Size.Y];
        for (int i = 0; i < pixels.Length; i++)
        { pixels[i] = 0; }

        /*if (screenScale)
        {
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
        else*/
        {
            //int[] tempPixels = new int[topleft.Width * topleft.Height];
            //tempPixels = convert

            PlacePixels(pixels, Size, topleft, Point.Zero);

            for (int i = (int)topleft[1][0]; i < Size.X - topright[1][0] - topleft[1][0]; i += (int)top[1][0])
            { PlacePixels(pixels, Size, top, new Point(i, 0)); }

            PlacePixels(pixels, Size, topright, new Point(Size.X - (int)topright[1][0], 0));

            for (int i = (int)topleft[1][1]; i < Size.Y - bottomleft[1][1] - topleft[1][1]; i += (int)left[1][1])
            { PlacePixels(pixels, Size, left, new Point(0, i)); }

            for (int j = (int)topleft[1][1]; j < Size.Y - topleft[1][1]; j += (int)bg[1][1])
            {
                for (int i = (int)topleft[1][0]; i < Size.X - topleft[1][0]; i += (int)bg[1][0])
                { PlacePixels(pixels, Size, bg, new Point(i, j)); }
            }

            PlacePixels(pixels, Size, bottomleft, new Point(0, Size.Y - (int)bottomleft[1][1]));

            for (int i = (int)topleft[1][0]; i < Size.X - bottomright[1][0] - bottomleft[1][0]; i += (int)bottom[1][0])
            { PlacePixels(pixels, Size, bottom, new Point(i, Size.Y - (int)bottomleft[1][1])); }

            for (int i = (int)topleft[1][1]; i < Size.Y - bottomright[1][1] - topright[1][1]; i += (int)right[1][1])
            { PlacePixels(pixels, Size, right, new Point(Size.X - (int)right[1][0], i)); }

            PlacePixels(pixels, Size, bottomright, new Point(Size.X - (int)bottomright[1][0], Size.Y - (int)bottomright[1][1]));

            /*
            // topleft
            for (uint i = 0; i < topleft[1][1]; i++)
            {
                for (uint j = 0; j < topleft[1][0]; j++)
                { pixels[i * Size.X + j] = topleft[0][i * topleft[1][0] + j]; }
            }
            
            // top
            for (uint k = 0; k < Size.X; k += top[1][0])
            {
                for (uint i = 0; i < top[1][1]; i++)
                {
                    for (uint j = 0; j < top[1][0] && j+k < Size.X - topleft[1][0]; j++)
                    { pixels[i * Size.X + j + topleft[1][0] + k] = top[0][i * top[1][0] + j]; }
                }
            }

            // topright
            for (uint i = 0; i < topright[1][1]; i++)
            {
                for (uint j = 0; j < topright[1][0]; j++)
                { pixels[i * Size.X + j + (uint)Size.X - topright[1][0]] = topright[0][i * topright[1][0] + j]; }
            }

            // left
            for (uint i = 0; i < left[1][1] && (i + topleft[1][1]) * Size.X < pixels.Length; i++)
            {
                for (uint j = 0; j < left[1][0]; j++)
                { pixels[(i + topleft[1][1]) * Size.X + j] = left[0][i * left[1][0] + j]; }
            }

            // bg
            for (uint l = 0; l < pixels.Length; l += (uint)(bg[1][1] * Size.X))
            {
                for (uint k = 0; k < Size.X - bg[1][0]; k += bg[1][0])
                {
                    for (uint i = 0; i < bg[1][1]; i++)
                    {
                        for (uint j = 0; j < bg[1][0]; j++)
                        {
                            uint pixelloc = (uint)((i + topleft[1][1] + l) * Size.X + j + topleft[1][0] + k);
                            if (pixelloc < pixels.Length)
                            { pixels[pixelloc] = bg[0][i * bg[1][0] + j]; }
                        }
                    }
                }
            }
            */


            /*spriteBatch.Draw(topleft, drawLocation, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
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
            spriteBatch.Draw(bottom, new Vector2(bottomright.Width, Size.Y - bottom.Height), new Rectangle(0, 0, Size.X - (bottomleft.Width + bottomright.Width), bottom.Height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);*/
        }

        Texture2D renderTarget = new Texture2D(Irbis.Irbis.game.GraphicsDevice, Size.X, Size.Y);
        renderTarget.SetData(pixels);
        return renderTarget;
    }

    public static uint[] PlacePixels(uint[] placein, Point Size, uint[][] toplace, Point placeat)
    {
        for (uint i = 0; i < toplace[1][1]; i++)
        {
            if (placeat.Y + i >= Size.Y)
            { i = int.MaxValue; }
            else
            {
                for (uint j = 0; j < toplace[1][0]; j++)
                {
                    if (placeat.X + j >= Size.X)
                    { j = int.MaxValue; }
                    else
                    { placein[(i + placeat.Y) * Size.X + j + placeat.X] = toplace[0][i * toplace[1][0] + j]; }
                }
            }
        }



        return placein;
    }
}
