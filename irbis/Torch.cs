using Irbis;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Torch
{
    ParticleSystem flame;
    Texture2D stick;
    Point position;

    public Torch(Point Position)
    {
        position = Position;
        flame = new ParticleSystem(new Vector2(-10,-10),new Vector2(50,25), new float[]{0.2f,1f,0.5f}, new float[]{0.1f,0.1f,0.05f,0f},
            new float[]{0.25f,0.15f}, 0.01f, new float[] {0.6f}, new float[]{10,25,0,0,0,0.05f,0.1f}, new Rectangle(Position.X, Position.Y, 3, 5),
            new Texture2D[]{Irbis.Irbis.LoadTexture("torchflame")}, new Color[]{Color.Transparent,Color.White,Color.Black,Color.Transparent},
            new Color[]{Color.Transparent,new Color(1f,0f,0f,0.2f)}, new int[]{1,1,3,1}, 0.1f, 0f, 3);

        stick = Irbis.Irbis.LoadTexture("torchstick");
    }

    public void Update()
    {
        flame.Update();
        if (Irbis.Irbis.GetMouseState.LeftButton == ButtonState.Pressed)
        { flame.spawnArea.Location = position = Irbis.Irbis.WorldSpaceMouseLocation; }
    }

    public void Draw(SpriteBatch sb)
    {
        flame.Draw(sb);
        sb.Draw(stick, position.ToVector2() * Irbis.Irbis.screenScale, null, Color.White, 0f, new Vector2(9,0), Irbis.Irbis.screenScale, SpriteEffects.None, 0.55f);
    }

    public void Light(SpriteBatch sb, bool UseColor)
    { flame.Light(sb, UseColor); }
}
