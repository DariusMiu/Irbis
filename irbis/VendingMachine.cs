using Irbis;
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
    float drawDepth;
    public Texture2D vendingTex;
    public Vector2 displayLocation;
    public Rectangle collider;
    public VendingType type;
    Rectangle sourceRect;
    Tooltip tooltip;
    public bool drawMenu;
    public int selection;
    VendingMenu menu;
    long[] cost;
    string[] itemDescriptions;

    public VendingMachine(int startingCost, VendingType vendingType, Rectangle displayRectangle, Texture2D texture, float depth)
    {
        sourceRect = texture.Bounds;
        displayLocation = displayRectangle.Location.ToVector2();
        collider = displayRectangle;
        vendingTex = texture;
        drawDepth = depth;

        type = vendingType;

        Texture2D[] icons;
        string[] itemNames;


        switch (vendingType)
        {
            case VendingType.Enchant:
                icons = Irbis.Irbis.LoadEnchantIcons();
                itemNames = new string[icons.Length];
                itemNames[0] = "Bleed";
                itemNames[1] = "Fire";
                itemNames[2] = "Frost";
                itemNames[3] = "Knockback";
                itemNames[4] = "Poison";
                itemNames[5] = "Sharpness";
                itemNames[6] = "Stun";
                itemDescriptions = Irbis.Irbis.LoadEnchantDescriptions();
                break;
            default:
                icons = Irbis.Irbis.LoadEnchantIcons();
                itemNames = new string[icons.Length];
                itemDescriptions = Irbis.Irbis.LoadEnchantDescriptions();
                for (int i = 0; i < icons.Length; i++)
                {
                    itemNames[i] = "butts";
                }
                break;
        }


        cost = new long[icons.Length];
        for (int i = 0; i < icons.Length; i++)
        {
            cost[i] = 200;
        }


        menu = new VendingMenu(icons, itemNames, cost);

        string popuptext = Irbis.Irbis.useKey + " to use";
        tooltip = Irbis.Irbis.tooltipGenerator.CreateTooltip(popuptext, new Point((int)((displayLocation.X + (sourceRect.Width / 2)) * Irbis.Irbis.screenScale), (int)((displayLocation.Y - (10 / Irbis.Irbis.screenScale)) * Irbis.Irbis.screenScale)), drawDepth);
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
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[0])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Bleed);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[0];
                            cost[0] *= 2;
                            menu.Update(0, cost[0], "butts", "Bleed");
                        }
                        break;
                    case 1: //fire
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[1])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Fire);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[1];
                            cost[1] *= 2;
                            menu.Update(1, cost[1], "butts", "Fire");
                        }
                        break;
                    case 2: //frost
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[2])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Frost);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[2];
                            cost[2] *= 2;
                            menu.Update(2, cost[2], "butts", "Frost");
                        }
                        break;
                    case 3: //knockback
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[3])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Knockback);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[3];
                            cost[3] *= 2;
                            menu.Update(3, cost[3], "butts", "Knockback");
                        }
                        break;
                    case 4: //poison
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[4])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Poison);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[4];
                            cost[4] *= 2;
                            menu.Update(4, cost[4], "butts", "Poison");
                        }
                        break;
                    case 5: //sharpness
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[5])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Sharpness);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[5];
                            cost[5] *= 2;
                            menu.Update(5, cost[5], "butts", "Sharpness");
                        }
                        break;
                    case 6: //stun
                        if (Irbis.Irbis.onslaughtSpawner.Points >= cost[6])
                        {
                            Irbis.Irbis.AddPlayerEnchant(EnchantType.Stun);
                            Irbis.Irbis.onslaughtSpawner.Points -= cost[6];
                            cost[6] *= 2;
                            menu.Update(6, cost[6], "butts", "Stun");
                        }
                        break;
                }
                break;
        }
        Irbis.Irbis.vendingMachineMenu = menu;
    }

    public void LoadMenu()
    {
        if (!drawMenu)
        {
            drawMenu = true;
            Irbis.Irbis.vendingMachineMenu = menu;
        }
        else
        {
            drawMenu = false;
            Irbis.Irbis.vendingMachineMenu = null;
        }
    }

    private void UpdateMenu()
    {
        menu.Update(selection, cost[selection], itemDescriptions[selection], ((EnchantType)selection).ToString());
        Irbis.Irbis.vendingMachineMenu = menu;
    }

    public void MoveSelectionDown()
    {
        selection = menu.MoveSelectionDown();
        UpdateMenu();
    }

    public void MoveSelectionUp()
    {
        selection = menu.MoveSelectionUp();
        UpdateMenu();
    }

    public void Update(Point MouseLocation)
    {
        int tempint = menu.IconConatains(MouseLocation);
        if (tempint >= 0)
        {
            selection = tempint;
            UpdateMenu();
        }
    }

    public void OnClick(Point MouseLocation)
    {
        if (menu.IconConatains(MouseLocation, selection))
        {
            Purchase(selection);
            UpdateMenu();
        }
    }

    public void Update(int selectedthingy)
    {
        if (selectedthingy >= 0 && selectedthingy < menu.cost.Length)
        {
            selection = selectedthingy;
            UpdateMenu();
        }
    }

    public override string ToString()
    {
        string debugstring = string.Empty;

        debugstring += "{location:" + displayLocation;
        debugstring += " depth:" + drawDepth;

        //for (int i = 0; i < cost.Length; i++)
        //{
        //    debugstring += " cost[" + i + "]:" + cost[i];
        //}

        //for (int i = 0; i < icons.Length; i++)
        //{
        //    debugstring += " icon[" + i + "]:" + icons[i];
        //}

        debugstring += "}";

        return debugstring;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(vendingTex, displayLocation * Irbis.Irbis.screenScale, sourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, drawDepth);
        //if (Irbis.Irbis.geralt != null)
        //{
        //    if (Irbis.Irbis.DistanceSquared(collider.Center, Irbis.Irbis.geralt.Collider.Center) <= Irbis.Irbis.vendingMachineUseDistanceSqr)
        //    {
        //        if (!drawMenu)
        //        {
        //            tooltip.Draw(sb);
        //        }
        //    }
        //    else if (drawMenu)
        //    {
        //        drawMenu = false;
        //        Irbis.Irbis.vendingMachineMenu = null;
        //    }
        //}
        if (!drawMenu && Irbis.Irbis.geralt != null && Irbis.Irbis.DistanceSquared(collider.Center, Irbis.Irbis.geralt.Collider.Center) <= Irbis.Irbis.vendingMachineUseDistanceSqr)
        {
            tooltip.Draw(sb);
        }
    }
}
