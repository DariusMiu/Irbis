using Irbis;
using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public class VendingMenu
{
    Texture2D mainBackground;
    Vector2 mainBackgroundVector;
    Texture2D itemWindow;
    Vector2 itemWindowVector;
    Texture2D itemInfo;
    Vector2 itemInfoVector;
    Texture2D pointsWindow;
    Vector2 pointsWindowVector;
    Print pointsPrint;
    Print infoTitlePrint;
    Print infoPrint;
    Print pricePrint;
    Texture2D[] icons;
    Vector2[] iconPositions;
    string[] itemNames;
    public long[] cost;
    int[] iconrows;
    int iconsPerRow;
    int currentSelection;
    Rectangle currentSelectionRect;

    public VendingMenu(Texture2D[] Icons, string[] ItemNames, long[] Prices)
    {
        mainBackground = Irbis.Irbis.tooltipGenerator.CreateTooltipTexture(Irbis.Irbis.resolution - (new Vector2(32 * Irbis.Irbis.screenScale, 32 * Irbis.Irbis.screenScale)).ToPoint() , true);
        mainBackgroundVector = new Vector2((int)(16 * Irbis.Irbis.screenScale), (int)(16 * Irbis.Irbis.screenScale));
        itemWindow = Irbis.Irbis.tooltipGenerator.CreateTooltipTexture(new Point((int)(((Irbis.Irbis.resolution.X * 2f) / 3f) - (32 * Irbis.Irbis.screenScale)), (int)((Irbis.Irbis.resolution.Y * 3f) / 4f)), true);
        itemWindowVector = new Vector2((int)(32 * Irbis.Irbis.screenScale), (int)(32 * Irbis.Irbis.screenScale));
        pointsWindow = Irbis.Irbis.tooltipGenerator.CreateTooltipTexture(new Point((Irbis.Irbis.resolution.X * 1) / 4, Irbis.Irbis.font.charHeight + (int)(16 * Irbis.Irbis.screenScale) /*16 is the height of the top and bottom borders*/), true);
        itemInfo = Irbis.Irbis.tooltipGenerator.CreateTooltipTexture(new Point((Irbis.Irbis.resolution.X * 1) / 4, (int)(itemWindow.Height - (pointsWindow.Height + (24 * Irbis.Irbis.screenScale)))), true);
        itemInfoVector = new Vector2((int)(Irbis.Irbis.resolution.X - (itemInfo.Width + (32 * Irbis.Irbis.screenScale))), (int)(32 * Irbis.Irbis.screenScale));
        pointsWindowVector = new Vector2(Irbis.Irbis.resolution.X - (itemInfo.Width + (32 * Irbis.Irbis.screenScale)), itemInfoVector.Y + (24 * Irbis.Irbis.screenScale) + itemInfo.Height);

        pointsPrint = new Print(pointsWindow.Width - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(pointsWindowVector.X + (Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale)), (int)(pointsWindowVector.Y + (pointsWindow.Height / 2f) /*- ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale) / 2f)*/)), Direction.left, 0.9f);

        icons = Icons;
        //iconPositions = IconPositions;
        itemNames = ItemNames;
        cost = Prices;

        iconrows = new int[icons.Length];

        iconPositions = new Vector2[icons.Length];
        //iconPositions[0] = new Vector2((int)(48 / Irbis.Irbis.screenScale), (int)(48 / Irbis.Irbis.screenScale));

        int tempint = 0;
        Point temppoint = new Point((int)(48 * Irbis.Irbis.screenScale), (int)(48 * Irbis.Irbis.screenScale));
        for (int i = 0; i < icons.Length; i++)
        {
            if (temppoint.X + ((16 + icons[i].Width) * Irbis.Irbis.screenScale) >= (itemWindowVector.X + itemWindow.Width) - 32)
            {
                temppoint.Y += (int)((16 + icons[i].Height) * Irbis.Irbis.screenScale);
                temppoint.X  = (int)(48 * Irbis.Irbis.screenScale);
                tempint++;
            }

            iconPositions[i] = temppoint.ToVector2();
            iconrows[i] = tempint;
            temppoint.X += (int)((16 + icons[i].Width) * Irbis.Irbis.screenScale);
        }

        iconsPerRow = IconCols(icons.Length, (int)((16 + icons[0].Width) * Irbis.Irbis.screenScale), itemWindow.Width - 32);

        currentSelection = 0;
        currentSelectionRect = new Rectangle(iconPositions[currentSelection].ToPoint(), new Point((int)(icons[currentSelection].Width * Irbis.Irbis.screenScale), (int)(icons[currentSelection].Height * Irbis.Irbis.screenScale)));

        infoTitlePrint = new Print(pointsWindow.Width - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(pointsWindowVector.X + (Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale)), (int)(itemInfoVector.Y + (Irbis.Irbis.font.charHeight * (Irbis.Irbis.textScale + 1)))), Direction.left, 0.9f);
        infoTitlePrint.textScale = Irbis.Irbis.textScale + 1;
        //infoTitlePrint.statement = "Fire";
        infoPrint = new Print(itemInfo.Width - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(itemInfoVector.X + (Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale)), (int)(itemInfoVector.Y + (Irbis.Irbis.font.charHeight * 2.5f * (Irbis.Irbis.textScale + 1)))), Direction.left, 0.9f);
        //infoPrint.statement = "Some bullshit";
        pricePrint = new Print(itemInfo.Width - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(itemInfoVector.X + itemInfo.Width - (Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale)), (int)((itemInfoVector.Y + itemInfo.Height) - (Irbis.Irbis.font.charHeight * 2 * (Irbis.Irbis.textScale)))), Direction.right, 0.9f);
        //pricePrint.statement = "Cost: 200";

        mainBackground = MergeWindows();
    }

    public void Update(int selection, long price, string iteminfo, string itemname)
    {
        currentSelection = selection;
        currentSelectionRect = new Rectangle(iconPositions[currentSelection].ToPoint(), new Point((int)(icons[currentSelection].Width * Irbis.Irbis.screenScale), (int)(icons[currentSelection].Height * Irbis.Irbis.screenScale)));
        infoTitlePrint.statement = itemname;
        infoPrint.statement = iteminfo;
        cost[currentSelection] = price;
        pricePrint.statement = "Cost: " + price;
    }

    public int MoveSelectionDown()
    {
        if (currentSelection + iconsPerRow < icons.Length)
        {
            currentSelection = currentSelection + iconsPerRow;
        }
        return currentSelection;
    }

    public int MoveSelectionUp()
    {
        if (currentSelection - iconsPerRow >= 0)
        {
            currentSelection = currentSelection - iconsPerRow;
        }
        return currentSelection;
    }

    public int IconConatains(Point MouseLocation)
    {
        Rectangle temprect;

        for (int i = 0; i < icons.Length; i++)
        {
            temprect = new Rectangle(iconPositions[i].ToPoint(), new Point((int)(icons[i].Width * Irbis.Irbis.screenScale), (int)(icons[i].Height * Irbis.Irbis.screenScale)));
            if (temprect.Contains(MouseLocation))
            {
                return i;
            }
        }
        return -1;
    }

    public bool IconConatains(Point MouseLocation, int selection)
    {
        Rectangle temprect = new Rectangle(iconPositions[selection].ToPoint(), new Point((int)(icons[selection].Width * Irbis.Irbis.screenScale), (int)(icons[selection].Height * Irbis.Irbis.screenScale)));
        if (temprect.Contains(MouseLocation))
        {
            return true;
        }
        return false;
    }

    private int IconRows(int icons, int iconsize, int maxwidth)
    {
        int rows = 0;
        int width = 0;
        for (int i = 0; i < icons; i++)
        {
            if (width + iconsize > maxwidth)
            {
                width = 0;
                rows++;
            }
            else
            {
                width += iconsize;
            }
        }
        return rows;
    }

    private int IconCols(int icons, int iconsize, int maxwidth)
    {
        int cols = 0;
        int width = 0;
        for (int i = 0; i < icons; i++)
        {
            if (!(width + iconsize > maxwidth))
            {
                width += iconsize;
                cols++;
            }
            else
            {
                break;
            }
        }

        return cols;
    }

    public Texture2D MergeWindows()
    {
        SpriteBatch sb = new SpriteBatch(Irbis.Irbis.game.GraphicsDevice); ;
        RenderTarget2D renderTarget = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, mainBackground.Bounds.Size.X, mainBackground.Bounds.Size.Y);
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, Matrix.Identity);

        sb.Draw(mainBackground, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        sb.Draw(itemWindow, itemWindowVector - mainBackgroundVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        sb.Draw(itemInfo, itemInfoVector - mainBackgroundVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        sb.Draw(pointsWindow, pointsWindowVector - mainBackgroundVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        sb.End();
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(null);

        return (Texture2D)renderTarget;
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(mainBackground, mainBackgroundVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        //sb.Draw(itemWindow, itemWindowVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        //sb.Draw(itemInfo, itemInfoVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        //sb.Draw(pointsWindow, pointsWindowVector, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
        pointsPrint.statement = "Points: " + Irbis.Irbis.onslaughtSpawner.Points.ToString();
        pointsPrint.Draw(sb);
        infoTitlePrint.Draw(sb);
        infoPrint.Draw(sb);
        pricePrint.Draw(sb);
        RectangleBorder.Draw(sb, currentSelectionRect, Color.White, 0.5f);
        for (int i = 0; i < icons.Length; i++)
        {
            sb.Draw(icons[i], iconPositions[i], icons[i].Bounds, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.4f);
            //prints[i].Draw(sb);
        }
    }
}
