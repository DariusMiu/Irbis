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
    public static Vector2 backgroundTextureLocation;

    public Bars(Texture2D HealthTexture, Texture2D ShieldTexture, Texture2D EnergyTexture, Texture2D ShieldBarOverlay, Texture2D BackgroundTexture)
    {
        int scale = (int)Irbis.Irbis.screenScale;

        backgroundTexture = Irbis.Irbis.ResizeTexture(BackgroundTexture, 16f / scale, false);
        HealthTexture = Irbis.Irbis.ResizeTexture(HealthTexture, 16f / scale, false);
        ShieldTexture = Irbis.Irbis.ResizeTexture(ShieldTexture, 16f / scale, false);
        EnergyTexture = Irbis.Irbis.ResizeTexture(EnergyTexture, 16f / scale, false);
        backgroundTextureLocation = new Vector2((int)(32), (int)(32));
        healthBar = new UIElementSlider(Direction.left, new Rectangle(                    (int)(32 + scale),                     (int)(32 + scale), HealthTexture.Width, HealthTexture.Height), new Point((int)(scale), 0), Irbis.Irbis.geralt.maxHealth, Color.White, null, null, null, HealthTexture, null,             null, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        shieldBar = new UIElementSlider(Direction.left, new Rectangle((int)(32 + scale) + (int)(15 * scale), (int)(32 + scale) + (int)(10 * scale), ShieldTexture.Width, ShieldTexture.Height), new Point((int)(scale), 0), Irbis.Irbis.geralt.maxShield, Color.White, null, null, null, ShieldTexture, null, ShieldBarOverlay, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        energyBar = new UIElementSlider(Direction.left, new Rectangle(                    (int)(32 + scale), (int)(32 + scale) + (int)(16 * scale), EnergyTexture.Width, EnergyTexture.Height), new Point((int)(scale), 0), Irbis.Irbis.geralt.maxEnergy, Color.White, null, null, null, EnergyTexture, null,             null, false, Irbis.Irbis.font, false, 0.5f, 0.499f, 0.501f, 0.502f);
        energyBar.drawOverlay = false;
        potionBar = new UIElementDiscreteSlider(Direction.left, new Rectangle((int)(32 / scale) + 76, (int)(32 / scale) + 11, 48, 8), Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Color.DarkSlateGray, Color.DarkRed, Color.DarkSlateBlue, Irbis.Irbis.geralt.maxNumberOfPotions, 3, 0.5f);
        enemyHealthBar = new UIElementSlider(Direction.right, new Rectangle((int)((Irbis.Irbis.resolution.X / scale) - (32 / scale)), (int)(32 / scale), 250, 10), Point.Zero, 100, new Color(108, 003, 003), Color.White, Color.White, Color.Red, Irbis.Irbis.nullTex, null, null, true, Irbis.Irbis.font, true, 0.5f, 0.499f, 0.501f, 0.502f);

        //For crappy computers?
        //healthBar = new UIElementSlider(Direction.left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale),     125, 10), geralt.maxHealth, new Color(166, 030, 030), Color.White, Color.White, Color.Red, nullTex, null,         null, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
        //shieldBar = new UIElementSlider(Direction.left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale) + 10, 75, 10), geralt.maxShield, new Color(255, 170, 000), Color.White, Color.White, Color.Red, nullTex, null, shieldBarTex, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
        //energyBar = new UIElementSlider(Direction.left, new Rectangle((int)(32 / screenScale), (int)(32 / screenScale) + 20, 50, 10), geralt.maxEnergy, new Color(000, 234, 255), Color.White, Color.White, Color.Red, nullTex, null,         null, true, font, true, 0.5f, 0.499f, 0.501f, 0.502f);
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(backgroundTexture, backgroundTextureLocation, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.499f);
        if (healthBar != null) { healthBar.Draw(sb); }
        if (shieldBar != null) { shieldBar.Draw(sb); }
        if (energyBar != null) { energyBar.Draw(sb); }
        if (potionBar != null) { potionBar.Draw(sb); }
        if (Irbis.Irbis.displayEnemyHealth) { enemyHealthBar.Draw(sb); }
    }
}
