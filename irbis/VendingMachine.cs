﻿using Irbis;
using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

[Serializable]
public class VendingMachine
{
    long[] cost;
    float drawDepth;
    public Texture2D vendingTex;
    public Rectangle displayRect;
    public VendingType type;
    Rectangle sourceRect;
    Texture2D[] icons;
    Rectangle[] iconRects;
    Print[] prints;
    public bool drawMenu;
    public int selection;
    RectangleBorder selectionborder;

    public VendingMachine(int startingCost, VendingType vendingType, Rectangle displayRectangle, Texture2D texture, float depth)
    {
        sourceRect = texture.Bounds;
        displayRect = displayRectangle;
        vendingTex = texture;
        icons = new Texture2D[0];
        drawDepth = depth;

        type = vendingType;

        switch (vendingType)
        {
            case VendingType.Enchant:
                icons = Irbis.Irbis.LoadEnchantIcons();
                break;
            default:
                icons = Irbis.Irbis.LoadEnchantIcons();
                break;
        }

        cost = new long[icons.Length];
        prints = new Print[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            cost[i] = 200;
        }
    }

    public void Purchase(int item)
    {
        //buy your thing
        switch (type)
        {
            case VendingType.Enchant:
                switch (item)
                {
                    case 0: //bleed
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.bleed);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[0];
                        cost[0] *= 2;
                        prints[0].Update("" + cost[0], true);
                        break;
                    case 1: //fire
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.fire);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[1];
                        cost[1] *= 2;
                        prints[1].Update("" + cost[1], true);
                        break;
                    case 2: //frost
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.frost);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[2];
                        cost[2] *= 2;
                        prints[2].Update("" + cost[2], true);
                        break;
                    case 3: //knockback
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.knockback);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[3];
                        cost[3] *= 2;
                        prints[3].Update("" + cost[3], true);
                        break;
                    case 4: //poison
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.poison);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[4];
                        cost[4] *= 2;
                        prints[4].Update("" + cost[4], true);
                        break;
                    case 5: //sharpness
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.sharpness);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[5];
                        cost[5] *= 2;
                        prints[5].Update("" + cost[5], true);
                        break;
                    case 6: //stun
                        Irbis.Irbis.AddPlayerEnchant(EnchantType.stun);
                        Irbis.Irbis.onslaughtSpawner.Points -= cost[6];
                        cost[6] *= 2;
                        prints[6].Update("" + cost[6], true);
                        break;
                }
                break;
        }

    }

    public void LoadMenu()
    {
        if (!drawMenu)
        {
            switch (type)
            {
                case VendingType.Enchant:
                    iconRects = new Rectangle[icons.Length];
                    for (int i = 0; i < icons.Length; i++)
                    {
                        iconRects[i] = new Rectangle(Irbis.Irbis.screenspace.Center.X + (icons[i].Width * (i - 3)), (Irbis.Irbis.screenspace.Center.Y), icons[i].Width, icons[i].Height);
                        prints[i] = new Print(32, Irbis.Irbis.font, Color.White, false, new Point(iconRects[i].Left, iconRects[i].Bottom - (Irbis.Irbis.font.charHeight/2)), Direction.left, 0.91f);
                        prints[i].Update("" + cost[i]);
                    }
                    selection = 0;
                    Update(selection);
                    break;
            }
            drawMenu = true;
        }
        else
        {
            drawMenu = false;
        }
    }

    public void Update(int selectedthingy)
    {
        if (selectedthingy >= 0 && selectedthingy < icons.Length)
        {
            selection = selectedthingy;
        }
        selectionborder = new RectangleBorder(iconRects[selection], Color.White, 0.905f);
    }

    public override string ToString()
    {
        string debugstring = string.Empty;

        debugstring += "{location:" + displayRect.Location;
        debugstring += " depth:" + drawDepth;

        for (int i = 0; i < cost.Length; i++)
        {
            debugstring += " cost[" + i + "]:" + cost[i];
        }

        for (int i = 0; i < icons.Length; i++)
        {
            debugstring += " icon[" + i + "]:" + icons[i];
        }

        debugstring += "}";

        return debugstring;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(vendingTex, displayRect, sourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, drawDepth);
        if (drawMenu)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                sb.Draw(icons[i], iconRects[i], icons[i].Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
                prints[i].Draw(sb);
                selectionborder.Draw(sb, Irbis.Irbis.nullTex);
            }
        }
    }
}