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
    int cost;
    Rectangle displayRect;
    Rectangle sourceRect;
    Texture2D[] icons;

    public VendingMachine(int startingCost, VendingType vendingType, Rectangle rectangle, Texture2D texture)
    {
        cost = startingCost;
        sourceRect = Rectangle.Empty;

        switch (vendingType)
        {
            case VendingType.Enchants:

                break;
            default:
                displayRect = Rectangle.Empty;
                break;
        }
    }

    public void Purchase(int item)
    {
        if (Irbis.Irbis.onslaughtMode)
        {
            //buy your thing





            cost = cost * 2;
        }
    }
    public void Draw()
    {

    }
}
