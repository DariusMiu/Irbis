using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LazorSpellEffect : SpellEffect
{
    ParticleSystem dottiboi;
    AngularParticleSystem glow;

    public LazorSpellEffect(Texture2D[] Textures)
    {
        dottiboi = new CircularParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.4f, 0.1f }, new float[] { 1f, 1f, 1f, 1f }, new float[] { 1f, 1f, 1f, 1f },
            0.05f, new float[] { 0.5f }, new float[] { 20f, 50f, 0.01f, 0f, 0f, 0.1f, 0.1f },
            new Rectangle(Point.Zero, new Point(2)), new Texture2D[] { Irbis.Irbis.dottex },
            new Color[] { Color.Transparent, Color.Cyan, Color.White, Color.Transparent }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
        glow = new AngularParticleSystem(15, -10, new float[] { 0.2f, 0.1f, 0.2f }, new float[] { 0.2f, 0.2f, 0.2f, 0.2f }, new float[] { 1f, 1f, 1f, 1f },
            0.003f, new float[] { 0.6f, 0.61f, 0.62f, 0.63f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0f, 0.1f },
            new Rectangle(Point.Zero, new Point(5)), Textures,
            new Color[] { Color.Transparent, Color.Cyan, Color.DarkCyan, Color.Transparent }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
    }

    public override void Update(Vector2 Position, bool Alive)
    {
        dottiboi.Update();
        dottiboi.Position = Position;
        glow.Update();
        glow.Position = Position;
        if (!Alive)
        {
            dottiboi.timeToLive = -1;
            glow.timeToLive = -1;
        }
    }

    public override void Draw(SpriteBatch sb)
    {
        dottiboi.Draw(sb);
        glow.Draw(sb);
    }

    public override void Light(SpriteBatch sb, bool UseColor)
    {
        dottiboi.Light(sb, UseColor);
        dottiboi.Light(sb, UseColor);
    }
}
