using Irbis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Bars
{
    public static UIElementSlider healthBar;
    public static UIElementSlider shieldBar;
    public static UIElementSlider energyBar;
    public static UIElementDiscreteSlider potionBar;
    public static UIElementSlider enemyHealthBar;
    public static Texture2D backgroundTexture;
    public static Texture2D enemyBackgroundTexture;
    public static Vector2 backgroundTextureLocation;
    public static Vector2 enemyBackgroundTextureLocation;
    public static Print name;

    public Bars(Texture2D HealthTexture, Texture2D ShieldTexture, Texture2D EnergyTexture, Texture2D PotionTexture, Texture2D EnemyHealthTexture,
        Texture2D ShieldBarOverlay,Texture2D BackgroundTexture, Texture2D EnemyBackgroundTexture, Texture2D[] PotionBackgroundTextures)
    {
        int scale = (int)Irbis.Irbis.screenScale;
        float resizefactor = scale / 16f;

        backgroundTexture = Irbis.Irbis.ResizeTexture(BackgroundTexture, resizefactor, false);
        enemyBackgroundTexture = Irbis.Irbis.ResizeTexture(EnemyBackgroundTexture, resizefactor, false);
        HealthTexture = Irbis.Irbis.ResizeTexture(HealthTexture, resizefactor, false);
        ShieldTexture = Irbis.Irbis.ResizeTexture(ShieldTexture, resizefactor, false);
        EnergyTexture = Irbis.Irbis.ResizeTexture(EnergyTexture, resizefactor, false);
        EnemyHealthTexture = Irbis.Irbis.ResizeTexture(EnemyHealthTexture, resizefactor, false);
        backgroundTextureLocation = new Vector2((int)(32), (int)(32));
        enemyBackgroundTextureLocation = new Vector2((int)((Irbis.Irbis.resolution.X) - (enemyBackgroundTexture.Width + 32)), (int)(32));

        for (int i = 0; i < PotionBackgroundTextures.Length; i++)
        {
            PotionBackgroundTextures[i] = Irbis.Irbis.ResizeTexture(PotionBackgroundTextures[i], resizefactor, false);
        }
        PotionTexture = Irbis.Irbis.ResizeTexture(PotionTexture, resizefactor, false);

        healthBar = new UIElementSlider(Direction.Left, new Rectangle(                    (int)(32 + scale),                     (int)(32 + scale), HealthTexture.Width, HealthTexture.Height),
            new Point((int)(scale), 0), Direction.Left, Irbis.Irbis.jamie.maxHealth, Color.White, null, null, null, HealthTexture, null,             null, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        shieldBar = new UIElementSlider(Direction.Left, new Rectangle((int)(32 + scale) + (int)(15 * scale), (int)(32 + scale) + (int)(10 * scale), ShieldTexture.Width, ShieldTexture.Height),
            new Point((int)(scale), 0), Direction.Left, Irbis.Irbis.jamie.maxShield, Color.White, null, null, null, ShieldTexture, null, ShieldBarOverlay, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        energyBar = new UIElementSlider(Direction.Left, new Rectangle(                    (int)(32 + scale), (int)(32 + scale) + (int)(16 * scale), EnergyTexture.Width, EnergyTexture.Height),
            new Point((int)(scale), 0), Direction.Left, Irbis.Irbis.jamie.maxEnergy, Color.White, null, null, null, EnergyTexture, null,             null, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        energyBar.drawOverlay = false;

        int maxpots = 3;
        if (Irbis.Irbis.jamie.maxNumberOfPotions / 2 > 3)
        { maxpots = Irbis.Irbis.jamie.maxNumberOfPotions / 2; }

        potionBar = new UIElementDiscreteSlider(Direction.Left, (backgroundTextureLocation + (new Vector2(1040, 176) * resizefactor)).ToPoint(),
            (backgroundTextureLocation + (new Vector2(800, 336) * resizefactor)).ToPoint(), new[] { PotionTexture }, PotionBackgroundTextures, null, Color.White, Color.White,
            null, Irbis.Irbis.jamie.maxNumberOfPotions, maxpots, (new Vector2(222, 96) * resizefactor).ToPoint(), (new Vector2(272,128) * resizefactor).ToPoint(), scale,  0.5f);

        enemyHealthBar = new UIElementSlider(Direction.Right, new Rectangle((int)((Irbis.Irbis.resolution.X) - (32 + scale)), (int)(32 + scale), EnemyHealthTexture.Width, EnemyHealthTexture.Height),
            Point.Zero, Direction.Right, 100, new Color(108, 003, 003), null, null, null, EnemyHealthTexture, null, null, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        name = new Print(EnemyHealthTexture.Width, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (int)(32 + (18 * scale)),
            (int)(40 + (10 * scale))), Direction.Right, 0.6f);
        name.Update("default", true);

        //For crappy computers?
        //healthBar = new UIElementSlider(Direction.Left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale),     125, 10), jamie.maxHealth, new Color(166, 030, 030), Color.White, Color.White, Color.Red, nullTex, null,         null, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
        //shieldBar = new UIElementSlider(Direction.Left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale) + 10, 75, 10), jamie.maxShield, new Color(255, 170, 000), Color.White, Color.White, Color.Red, nullTex, null, shieldBarTex, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
        //energyBar = new UIElementSlider(Direction.Left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale) + 20, 50, 10), jamie.maxEnergy, new Color(000, 234, 255), Color.White, Color.White, Color.Red, nullTex, null,         null, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(backgroundTexture, backgroundTextureLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.499f);
        if (healthBar != null) { healthBar.Draw(sb); }
        if (shieldBar != null) { shieldBar.Draw(sb); }
        if (energyBar != null) { energyBar.Draw(sb); }
        if (potionBar != null) { potionBar.Draw(sb); }
        if (Irbis.Irbis.displayEnemyHealth)
        {
            sb.Draw(enemyBackgroundTexture, enemyBackgroundTextureLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.499f);
            enemyHealthBar.Draw(sb);
            name.Draw(sb);
        }
    }
}
