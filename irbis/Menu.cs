using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Menu
{
    public bool Exists
    {
        get
        { return exists; }
    }
    private bool exists;
    public Menu ()
    { exists = true; }

    public void Create(int scene)
    {
        Point tempLP;
        Point tempDP;
        Irbis.Irbis.justLeftMenu = true;
        switch (scene)
        {
            case -1:
                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point(000, (int)((5 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.resolution.Y - (tempLP.Y - 000000000000000000000000000000000000), (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "New game", ">NEW GAME", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[0].bounds.Y + Irbis.Irbis.buttonList[0].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Continue", ">CONTINUE", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[1].bounds.Y + Irbis.Irbis.buttonList[1].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Options", ">OPTIONS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Credits", ">CREDITS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[3].bounds.Y + Irbis.Irbis.buttonList[3].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Exit();", ">EXIT();", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.lastMenuScene = Irbis.Irbis.scene = 0;
                break;
            case 0:     //main menu
                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point(000, (int)((5 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f/8f)), Irbis.Irbis.resolution.Y - (tempLP.Y - 000000000000000000000000000000000000), (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "New game", ">NEW GAME", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f/8f)), Irbis.Irbis.buttonList[0].bounds.Y + Irbis.Irbis.buttonList[0].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Continue", ">CONTINUE", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f/8f)), Irbis.Irbis.buttonList[1].bounds.Y + Irbis.Irbis.buttonList[1].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Options", ">OPTIONS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                if (!Irbis.Irbis.creditsActive)
                { Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Credits", ">CREDITS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f)); }
                else
                { Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Exit credits", ">EXIT CREDITS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f)); }
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f/8f)), Irbis.Irbis.buttonList[3].bounds.Y + Irbis.Irbis.buttonList[3].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Exit();", ">EXIT();", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 1:     //options menu
                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point(000, (int)((5 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                if (!Irbis.Irbis.resetRequired)
                { Irbis.Irbis.DisplayInfoText("For even more options and details, view the playerSettings.ini file", 0); }
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.resolution.Y - (tempLP.Y - 000000000000000000000000000000000000), (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Controls", ">Controls", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[0].bounds.Y + Irbis.Irbis.buttonList[0].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Camera", ">Camera", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[1].bounds.Y + Irbis.Irbis.buttonList[1].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Video", ">Video", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Audio", ">Audio", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.resolution.X * (5f / 8f)), Irbis.Irbis.buttonList[3].bounds.Y + Irbis.Irbis.buttonList[3].bounds.Height, (int)(Irbis.Irbis.resolution.X / 4f), (int)(25 * 2 * Irbis.Irbis.textScale)), Direction.Left, "Misc", ">Misc", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bBack", "<\u001bBack", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 2:     //options - controls
                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point(000, (int)((5 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                tempDP = new Point((int)(80 * Irbis.Irbis.textScale), (int)(24 * Irbis.Irbis.textScale));
                Irbis.Irbis.listenForNewKeybind = false;

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)((Irbis.Irbis.resolution.X * (6f / 8f)) - (40 * Irbis.Irbis.textScale)), Irbis.Irbis.resolution.Y - (tempLP.Y + 20), tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.attackKey.ToString(), ">" + Irbis.Irbis.attackKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[0].bounds.Y + Irbis.Irbis.buttonList[0].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.jumpKey.ToString(), ">" + Irbis.Irbis.jumpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[1].bounds.Y + Irbis.Irbis.buttonList[1].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.rollKey.ToString(), ">" + Irbis.Irbis.rollKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.potionKey.ToString(), ">" + Irbis.Irbis.potionKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[3].bounds.Y + Irbis.Irbis.buttonList[3].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.shieldKey.ToString(), ">" + Irbis.Irbis.shieldKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[4].bounds.Y + Irbis.Irbis.buttonList[4].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.shockwaveKey.ToString(), ">" + Irbis.Irbis.shockwaveKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[5].bounds.Y + Irbis.Irbis.buttonList[5].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.upKey.ToString(), ">" + Irbis.Irbis.upKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[6].bounds.Y + Irbis.Irbis.buttonList[6].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.downKey.ToString(), ">" + Irbis.Irbis.downKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[7].bounds.Y + Irbis.Irbis.buttonList[7].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.leftKey.ToString(), ">" + Irbis.Irbis.leftKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[8].bounds.Y + Irbis.Irbis.buttonList[8].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.rightKey.ToString(), ">" + Irbis.Irbis.rightKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X, Irbis.Irbis.buttonList[9].bounds.Y + Irbis.Irbis.buttonList[9].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.useKey.ToString(), ">" + Irbis.Irbis.useKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)((Irbis.Irbis.resolution.X * (6f / 8f)) + (40 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[0].bounds.Y, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altAttackKey.ToString(), ">" + Irbis.Irbis.altAttackKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[0].bounds.Y + Irbis.Irbis.buttonList[0].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altJumpKey.ToString(), ">" + Irbis.Irbis.altJumpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[1].bounds.Y + Irbis.Irbis.buttonList[1].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altRollKey.ToString(), ">" + Irbis.Irbis.altRollKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[2].bounds.Y + Irbis.Irbis.buttonList[2].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altPotionKey.ToString(), ">" + Irbis.Irbis.altPotionKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[3].bounds.Y + Irbis.Irbis.buttonList[3].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altShieldKey.ToString(), ">" + Irbis.Irbis.altShieldKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[4].bounds.Y + Irbis.Irbis.buttonList[4].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altShockwaveKey.ToString(), ">" + Irbis.Irbis.altShockwaveKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[5].bounds.Y + Irbis.Irbis.buttonList[5].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altUpKey.ToString(), ">" + Irbis.Irbis.altUpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[6].bounds.Y + Irbis.Irbis.buttonList[6].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altDownKey.ToString(), ">" + Irbis.Irbis.altDownKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[7].bounds.Y + Irbis.Irbis.buttonList[7].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altLeftKey.ToString(), ">" + Irbis.Irbis.altLeftKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[8].bounds.Y + Irbis.Irbis.buttonList[8].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altRightKey.ToString(), ">" + Irbis.Irbis.altRightKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[11].bounds.X, Irbis.Irbis.buttonList[9].bounds.Y + Irbis.Irbis.buttonList[9].bounds.Height, tempDP.X, tempDP.Y), Direction.Forward, Irbis.Irbis.altUseKey.ToString(), ">" + Irbis.Irbis.altUseKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                //Save and Cancel buttons
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[22].bounds.X + Irbis.Irbis.buttonList[22].bounds.Width, (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(100 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                for (int i = 1; i < 11; i++)
                {
                    if (i < 11)
                    {
                        Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point((int)(Irbis.Irbis.buttonList[i].bounds.X - (5 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[i].bounds.Y), new Point(200 * Irbis.Irbis.textScale, Irbis.Irbis.textScale), Irbis.Irbis.screenScale, false, true, 0.1f));
                        Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(27, 28, 32), new Point((int)(Irbis.Irbis.buttonList[i].bounds.X - (5 * Irbis.Irbis.textScale) + Irbis.Irbis.textScale), Irbis.Irbis.buttonList[i].bounds.Y + Irbis.Irbis.textScale), new Point(200 * Irbis.Irbis.textScale, Irbis.Irbis.textScale), Irbis.Irbis.screenScale, false, true, 0.09f));
                    }
                }

                Print op201t = new Print((int)(100 * Irbis.Irbis.textScale),Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.buttonList[0].bounds.Center.X, Irbis.Irbis.buttonList[0].bounds.Y - Irbis.Irbis.buttonList[0].bounds.Height), Direction.Forward, 0.5f);
                op201t.Update("Key");
                Irbis.Irbis.printList.Add(op201t);
                Print op202t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.buttonList[11].bounds.Center.X, Irbis.Irbis.buttonList[11].bounds.Y - Irbis.Irbis.buttonList[11].bounds.Height), Direction.Forward, 0.5f);
                op202t.Update("Alt.");
                Irbis.Irbis.printList.Add(op202t);

                Print op21t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[0].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[0].bounds.Y  + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op21t.Update("Attack");
                Irbis.Irbis.printList.Add(op21t);
                Print op22t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[1].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[1].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op22t.Update("Jump");
                Irbis.Irbis.printList.Add(op22t);
                Print op23t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[2].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[2].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op23t.Update("Roll");
                Irbis.Irbis.printList.Add(op23t);
                Print op24t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[3].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[3].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op24t.Update("Potion");
                Irbis.Irbis.printList.Add(op24t);
                Print op25t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[4].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[4].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op25t.Update("Shield");
                Irbis.Irbis.printList.Add(op25t);
                Print op26t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[5].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[5].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op26t.Update("Shockwave");
                Irbis.Irbis.printList.Add(op26t);
                Print op27t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[6].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[6].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op27t.Update("Up");
                Irbis.Irbis.printList.Add(op27t);
                Print op28t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[7].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[7].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op28t.Update("Down");
                Irbis.Irbis.printList.Add(op28t);
                Print op29t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[8].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[8].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op29t.Update("Left");
                Irbis.Irbis.printList.Add(op29t);
                Print op210t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[9].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[9].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op210t.Update("Right");
                Irbis.Irbis.printList.Add(op210t);
                Print op211t = new Print((int)(100 * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, false, new Point((int)(Irbis.Irbis.buttonList[10].bounds.X - (10 * Irbis.Irbis.textScale)), Irbis.Irbis.buttonList[10].bounds.Y + ((Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale))), Direction.Right, 0.5f);
                op211t.Update("Use");
                Irbis.Irbis.printList.Add(op211t);

                break;
            case 3:     //options - camera
                Print BB = new Print(Irbis.Irbis.boundingBox.Width, Irbis.Irbis.font, Color.Magenta, false, new Point(Irbis.Irbis.boundingBox.Center.X, Irbis.Irbis.boundingBox.Center.Y), Direction.Forward, 0.5f);
                BB.Update("Bounding Box");
                Irbis.Irbis.printList.Add(BB);
                Irbis.Irbis.listenForNewKeybind = false;

                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point((int)(Irbis.Irbis.resolution.X * (6f / 8f)), (int)((5 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen

                Print op31t = new Print(200 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y)), Direction.Forward, 0.5f);
                op31t.Update("Bounding Box (anchor)");
                Irbis.Irbis.printList.Add(op31t);
                Print op31at = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X - (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (22 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op31at.Update("X:");
                Irbis.Irbis.printList.Add(op31at);
                Print op31bt = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, tempLP.Y + (22 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op31bt.Update("Y:");
                Irbis.Irbis.printList.Add(op31bt);
                Print op32t = new Print(200 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (50 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op32t.Update("Bounding Box (Width/Height)");
                Irbis.Irbis.printList.Add(op32t);
                Print op32at = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X - (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (72 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op32at.Update("X:");
                Irbis.Irbis.printList.Add(op32at);
                Print op32bt = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (72 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op32bt.Update("Y:");
                Irbis.Irbis.printList.Add(op32bt);
                Print op33t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (100 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op33t.Update("Smart Camera");
                Irbis.Irbis.printList.Add(op33t);
                Print op34t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (150 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op34t.Update("Lerp Speed");
                Irbis.Irbis.printList.Add(op34t);
                Print op34at = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X - (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (172 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op34at.Update("X:");
                Irbis.Irbis.printList.Add(op34at);
                Print op34bt = new Print(12 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (172 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op34bt.Update("Y:");
                Irbis.Irbis.printList.Add(op34bt);
                Print op35t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (200 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op35t.Update("Camera Shake");
                Irbis.Irbis.printList.Add(op35t);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (15 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.boundingBox.Center.X.ToString(), ">" + Irbis.Irbis.boundingBox.Center.X.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X + (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (15 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.boundingBox.Center.Y.ToString(), ">" + Irbis.Irbis.boundingBox.Center.Y.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (65 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.boundingBox.Width.ToString(), ">" + Irbis.Irbis.boundingBox.Width.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X + (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (65 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.boundingBox.Height.ToString(), ">" + Irbis.Irbis.boundingBox.Height.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (115 * Irbis.Irbis.textScale), 50 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.smartCamera.ToString(), ">" + Irbis.Irbis.smartCamera.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (165 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.cameraLerpXSpeed.ToString(), ">" + Irbis.Irbis.cameraLerpXSpeed.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X + (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (165 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.cameraLerpYSpeed.ToString(), ">" + Irbis.Irbis.cameraLerpYSpeed.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (215 * Irbis.Irbis.textScale), 50 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.cameraShakeSetting.ToString(), ">" + Irbis.Irbis.cameraShakeSetting.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));
                
                //Save and Cancel buttons
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[8].bounds.X + Irbis.Irbis.buttonList[8].bounds.Width, (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(100 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                break;
            case 4:     //options - video
                Irbis.Irbis.listenForNewKeybind = false;
                tempLP = new Point((int)(Irbis.Irbis.resolution.X * (6f / 8f)), (int)((4 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen


                Print op41t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) - (50 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op41t.Update("Display");
                Irbis.Irbis.printList.Add(op41t);
                Print op42t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (00 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op42t.Update("Window Scale");
                Irbis.Irbis.printList.Add(op42t);
                Print op42at = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X - (25 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (22 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op42at.Update("x");
                Irbis.Irbis.printList.Add(op42at);
                Print op43t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (50 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op43t.Update("Resolution");
                Irbis.Irbis.printList.Add(op43t);
                Print op43at = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X - (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (72 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op43at.Update("X:");
                Irbis.Irbis.printList.Add(op43at);
                Print op43bt = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (72 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op43bt.Update("Y:");
                Irbis.Irbis.printList.Add(op43bt);
                Print op46t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (100 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op46t.Update("vSync");
                Irbis.Irbis.printList.Add(op46t);
                Print op47t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (150 * Irbis.Irbis.textScale)), Direction.Forward, 0.5f);
                op47t.Update("Lighting");
                Irbis.Irbis.printList.Add(op47t);

                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point(Irbis.Irbis.resolution.X - (tempLP.X + 83), Irbis.Irbis.resolution.Y - (tempLP.Y - 23)), new Point(65, 1), Irbis.Irbis.textScale, false, true, 0.5f));
                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(027, 028, 032), new Point(Irbis.Irbis.resolution.X - (tempLP.X + 82), Irbis.Irbis.resolution.Y - (tempLP.Y - 24)), new Point(65, 1), Irbis.Irbis.textScale, false, true, 0.5f));

                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 23)), new Point(69, 1), Irbis.Irbis.textScale, false, true, 0.5f));
                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(027, 028, 032), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 26), Irbis.Irbis.resolution.Y - (tempLP.Y - 24)), new Point(69, 1), Irbis.Irbis.textScale, false, true, 0.5f));

                if (Irbis.Irbis.graphics.IsFullScreen)
                {
                    Irbis.Irbis.sList[0].drawTex = Irbis.Irbis.sList[1].drawTex = false;
                    Irbis.Irbis.sList[2].drawTex = Irbis.Irbis.sList[3].drawTex = true;
                }
                else
                {
                    Irbis.Irbis.sList[0].drawTex = Irbis.Irbis.sList[1].drawTex = true;
                    Irbis.Irbis.sList[2].drawTex = Irbis.Irbis.sList[3].drawTex = false;
                }

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) - (35 * Irbis.Irbis.textScale), 80 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, "windowed", ">WINDOWED<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X + (60 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) - (35 * Irbis.Irbis.textScale), 80 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, "fullscreen", ">FULLSCREEN<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (15 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, "", ">  <", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (65 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.tempResolution.X.ToString(), ">" + Irbis.Irbis.tempResolution.X.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X + (30 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (65 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.tempResolution.Y.ToString(), ">" + Irbis.Irbis.tempResolution.Y.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, true, 0.5f));

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (115 * Irbis.Irbis.textScale), 50 * Irbis.Irbis.textScale, 20 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace.ToString(), ">" + Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X, (Irbis.Irbis.resolution.Y - tempLP.Y) + (165 * Irbis.Irbis.textScale), 50 * Irbis.Irbis.textScale, 20 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.lightingEnabled.ToString(), ">" + Irbis.Irbis.lightingEnabled.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, true, false, 0.5f));

                //Save and Cancel buttons
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[7].bounds.X + Irbis.Irbis.buttonList[7].bounds.Width, (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(100 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                break;
            case 5:     //options - audio
                Irbis.Irbis.sliderPressed = -1;
                Irbis.Irbis.listenForNewKeybind = false;
                // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                tempLP = new Point((int)(Irbis.Irbis.resolution.X * (6f / 8f)), (int)((3 + 1) * (25 * 2 * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen

                Print op52t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (int)((Irbis.Irbis.resolution.Y - tempLP.Y))), Direction.Forward, 0.5f);
                op52t.Update("Master");
                Irbis.Irbis.printList.Add(op52t);
                Print op53t = new Print(100 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (int)((Irbis.Irbis.resolution.Y - tempLP.Y) + (50 * Irbis.Irbis.textScale))), Direction.Forward, 0.5f);
                op53t.Update("Music");
                Irbis.Irbis.printList.Add(op53t);
                Print op54t = new Print(200 * Irbis.Irbis.textScale, Irbis.Irbis.font, Color.White, false, new Point(tempLP.X, (int)((Irbis.Irbis.resolution.Y - tempLP.Y) + (100 * Irbis.Irbis.textScale))), Direction.Forward, 0.5f);
                op54t.Update("Sound Effects");
                Irbis.Irbis.printList.Add(op54t);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (170 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (015 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.masterAudioLevel.ToString("0"), ">" + Irbis.Irbis.masterAudioLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.Left, new Rectangle(tempLP.X - (115 * Irbis.Irbis.textScale), (int)((Irbis.Irbis.resolution.Y - tempLP.Y) + (014 * Irbis.Irbis.textScale)), 250 * Irbis.Irbis.textScale, 20 * Irbis.Irbis.textScale), Point.Zero, Direction.Left, 100, new Color(166, 030, 030), Color.White, Color.White, Color.Red, Irbis.Irbis.nullTex, null, null, true, null, false, 0.9f, 0.899f, 0.901f, 0.902f));
                Irbis.Irbis.sliderList[0].UpdateValue(Irbis.Irbis.masterAudioLevel);
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (170 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (065 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.musicLevel.ToString("0"), ">" + Irbis.Irbis.musicLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.Left, new Rectangle(tempLP.X - (115 * Irbis.Irbis.textScale), (int)((Irbis.Irbis.resolution.Y - tempLP.Y) + (064 * Irbis.Irbis.textScale)), 250 * Irbis.Irbis.textScale, 20 * Irbis.Irbis.textScale), Point.Zero, Direction.Left, 100, new Color(255, 170, 000), Color.White, Color.White, Color.Red, Irbis.Irbis.nullTex, null, null, true, null, false, 0.9f, 0.899f, 0.901f, 0.902f));
                Irbis.Irbis.sliderList[1].UpdateValue(Irbis.Irbis.musicLevel);
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(tempLP.X - (170 * Irbis.Irbis.textScale), (Irbis.Irbis.resolution.Y - tempLP.Y) + (115 * Irbis.Irbis.textScale), 40 * Irbis.Irbis.textScale, 16 * Irbis.Irbis.textScale), Direction.Forward, Irbis.Irbis.soundEffectsLevel.ToString("0"), ">" + Irbis.Irbis.soundEffectsLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.Left, new Rectangle(tempLP.X - (115 * Irbis.Irbis.textScale), (int)((Irbis.Irbis.resolution.Y - tempLP.Y) + (114 * Irbis.Irbis.textScale)), 250 * Irbis.Irbis.textScale, 20 * Irbis.Irbis.textScale), Point.Zero, Direction.Left, 100, new Color(000, 234, 255), Color.White, Color.White, Color.Red, Irbis.Irbis.nullTex, null, null, true, null, false, 0.9f, 0.899f, 0.901f, 0.902f));
                Irbis.Irbis.sliderList[2].UpdateValue(Irbis.Irbis.soundEffectsLevel);

                //Save and Cancel buttons
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[3].bounds.X + Irbis.Irbis.buttonList[3].bounds.Width, (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(100 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 6:     //options - misc
                Irbis.Irbis.DisplayInfoText("Coming soon! Hit escape to go back", 0);


                //Save and Cancel buttons
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.buttonList[0].bounds.X + Irbis.Irbis.buttonList[0].bounds.Width, (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(100 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                break;
            case 7:     //new game level select
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle((int)(Irbis.Irbis.font.charHeight * Irbis.Irbis.textScale), (int)(Irbis.Irbis.resolution.Y - (Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), (int)(50 * Irbis.Irbis.textScale), (int)(Irbis.Irbis.font.charHeight * 2 * Irbis.Irbis.textScale)), Direction.Left, Side.Left, "\u001bBack", "<\u001bBack", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.levelList = Directory.GetFiles(".\\levels");
                Irbis.Irbis.levelListCounter = 0;
                int buttonHeight = 25;

                {
                    List<string> tempLevelList = new List<string>();

                    for (int i = 0; i < Irbis.Irbis.levelList.Length; i++)
                    {
                        if (Irbis.Irbis.levelList[i].Length > 13 && Irbis.Irbis.IsDefaultLevelFormat(Irbis.Irbis.levelList[i].Substring(9, Irbis.Irbis.levelList[i].Length - 13))
                            && Irbis.Irbis.levelList[i].StartsWith(".\\levels\\") && Irbis.Irbis.levelList[i].EndsWith(".lvl"))
                        { tempLevelList.Add(Irbis.Irbis.levelList[i].Substring(9, Irbis.Irbis.levelList[i].Length - 13)); }
                    }

                    for (int i = 0; i < Irbis.Irbis.levelList.Length; i++)
                    {
                        if (Irbis.Irbis.levelList[i].Length > 13 && !Irbis.Irbis.IsDefaultLevelFormat(Irbis.Irbis.levelList[i].Substring(9, Irbis.Irbis.levelList[i].Length - 13))
                            && Irbis.Irbis.levelList[i].StartsWith(".\\levels\\") && Irbis.Irbis.levelList[i].EndsWith(".lvl"))
                        { tempLevelList.Add(Irbis.Irbis.levelList[i].Substring(9, Irbis.Irbis.levelList[i].Length - 13)); }
                    }


                    Irbis.Irbis.levelList = tempLevelList.ToArray();
                }

                Irbis.Irbis.WriteLine("level files formatted properly:" + Irbis.Irbis.levelList.Length);

                if (Irbis.Irbis.levelList.Length > Irbis.Irbis.maxButtonsOnScreen)
                {
                    // tempLP.Y == (int)((number of buttons + 1) * (buttonheight * 2 * Irbis.Irbis.textScale))
                    tempLP = new Point((int)(Irbis.Irbis.resolution.X * (5f / 8f)), (int)((Irbis.Irbis.maxButtonsOnScreen + 1) * (buttonHeight * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                    Irbis.Irbis.isMenuScrollable = true;
                }
                else if (Irbis.Irbis.levelList.Length < 10)
                {
                    //tempLP = new Point(500, (Irbis.Irbis.levelList.Length * 25) + ((10 - Irbis.Irbis.levelList.Length) * 25));
                    tempLP = new Point((int)(Irbis.Irbis.resolution.X * (5f / 8f)), (int)((10 + 1) * (buttonHeight * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                    Irbis.Irbis.isMenuScrollable = false;
                }
                else
                {
                    //tempLP = new Point(500, Irbis.Irbis.levelList.Length * 25);
                    tempLP = new Point((int)(Irbis.Irbis.resolution.X * (5f / 8f)), (int)((Irbis.Irbis.levelList.Length + 1) * (buttonHeight * Irbis.Irbis.textScale)));       //distance from the bottom right corner of the screen
                    Irbis.Irbis.isMenuScrollable = false;
                }

                for (int i = 0; i < Irbis.Irbis.levelList.Length; i++)
                {
                    string nextLevel = Irbis.Irbis.levelList[i];
                    int nextLevelLocation = i;
                    for (int j = i; j < Irbis.Irbis.levelList.Length; j++)
                    {
                        if (!nextLevel.Equals(Irbis.Irbis.levelList[j]))
                        {
                            if (Irbis.Irbis.IsDefaultLevelFormat(Irbis.Irbis.levelList[j]))
                            {
                                Point tempPoint = Irbis.Irbis.GetLevelChapterAndMap(Irbis.Irbis.levelList[j]);
                                if (Irbis.Irbis.IsDefaultLevelFormat(nextLevel))
                                {
                                    if (Irbis.Irbis.GetLevelChapterAndMap(Irbis.Irbis.levelList[j]).X < Irbis.Irbis.GetLevelChapterAndMap(nextLevel).X && Irbis.Irbis.GetLevelChapterAndMap(Irbis.Irbis.levelList[j]).Y < Irbis.Irbis.GetLevelChapterAndMap(nextLevel).Y)
                                    {
                                        nextLevel = Irbis.Irbis.levelList[j];
                                        nextLevelLocation = j;
                                    }
                                    else if (Irbis.Irbis.GetLevelChapterAndMap(Irbis.Irbis.levelList[j]).X == Irbis.Irbis.GetLevelChapterAndMap(nextLevel).X && Irbis.Irbis.GetLevelChapterAndMap(Irbis.Irbis.levelList[j]).Y == Irbis.Irbis.GetLevelChapterAndMap(nextLevel).Y)
                                    {
                                        if (0 >= string.Compare(Irbis.Irbis.levelList[j], nextLevel))
                                        {
                                            nextLevel = Irbis.Irbis.levelList[j];
                                            nextLevelLocation = j;
                                        }
                                    }
                                }
                                else
                                {
                                    nextLevel = Irbis.Irbis.levelList[j];
                                    nextLevelLocation = j;
                                }
                            }
                            else if (!Irbis.Irbis.IsDefaultLevelFormat(nextLevel))
                            {
                                if (0 >= string.Compare(Irbis.Irbis.levelList[j], nextLevel))
                                {
                                    nextLevel = Irbis.Irbis.levelList[j];
                                    nextLevelLocation = j;
                                }
                            }
                        }
                    }
                    if (nextLevelLocation >= 0)
                    {
                        Irbis.Irbis.levelList[nextLevelLocation] = Irbis.Irbis.levelList[i];
                        Irbis.Irbis.levelList[i] = nextLevel;
                    }
                }

                foreach (string s in Irbis.Irbis.levelList)
                {
                    Console.WriteLine(s);
                }

                if (Irbis.Irbis.levelList.Length > Irbis.Irbis.maxButtonsOnScreen)
                {
                    //scroll
                    for (int i = 0; i < Irbis.Irbis.maxButtonsOnScreen; i++)
                    {
                        Button tempButton = new Button(new Rectangle(tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (buttonHeight * i * Irbis.Irbis.textScale)),
                        (int)(Irbis.Irbis.resolution.X / 4f), buttonHeight * Irbis.Irbis.textScale), Direction.Left, Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[i]),
                        ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[i]), Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                        tempButton.data = Irbis.Irbis.levelList[i];
                        Irbis.Irbis.buttonList.Add(tempButton);
                    }
                }
                else
                {
                    for (int i = 0; i < Irbis.Irbis.levelList.Length; i++)
                    {
                        Button tempButton = new Button(new Rectangle(tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (buttonHeight * i * Irbis.Irbis.textScale)),
                        (int)(Irbis.Irbis.resolution.X / 4f), buttonHeight * Irbis.Irbis.textScale), Direction.Left, Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[i]),
                        ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[i]), Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                        tempButton.data = Irbis.Irbis.levelList[i];
                        Irbis.Irbis.buttonList.Add(tempButton);
                    }
                }

                //menuSelection = 1;
                break;
            default:
                Console.WriteLine("Error. Scene ID " + scene + " is not in LoadMenu list");
                break;
        }

    }

    public bool Update(Irbis.Irbis game)
    {
        switch (Irbis.Irbis.lastMenuScene)
        {
            case 0:     //main menu
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                    {
                        Irbis.Irbis.menuSelection = i;
                        //Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].highlightStatement.ToUpper());
                    }
                }
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (i == Irbis.Irbis.menuSelection)
                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                    else
                    { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                }
                if ((Irbis.Irbis.GetDownKeyDown) || (Irbis.Irbis.GetRightKeyDown))
                { Irbis.Irbis.menuSelection++; }
                if ((Irbis.Irbis.GetUpKeyDown) || (Irbis.Irbis.GetLeftKeyDown))
                { Irbis.Irbis.menuSelection--; }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                { Irbis.Irbis.menuSelection = 0; }
                if (Irbis.Irbis.menuSelection < 0)
                { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                //game DECIDES WHAT EACH BUTTON DOES
                if (Irbis.Irbis.Use() || Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState))
                {
                    switch (Irbis.Irbis.menuSelection)
                    {
                        case 0:                         //0 == New Game
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    New Game");
                            game.LoadMenu(7, 1, false);
                            break;
                        case 1:                         //1 == Continue
                            if (Irbis.Irbis.creditsActive)
                            { game.PauseCredits(); }
                            else
                            {
                                Irbis.Irbis.buttonList.Clear();
                                Irbis.Irbis.WriteLine("    Continue");
                                Irbis.Irbis.sceneIsMenu = false;
                                if (Irbis.Irbis.jamie == null) { Irbis.Irbis.levelEditor = true; return true; }
                                if (Irbis.Irbis.levelLoaded > 0)        ///game MEANS A /TRUE/ LEVEL (one not loaded exclusively for the titlescreen) HAS ALREADY BEEN LOADED
                                { if (Irbis.Irbis.debug <= 0) { game.IsMouseVisible = false; } }
                                else
                                {
                                    game.LoadUI();
                                    Irbis.Irbis.levelLoaded = 11;
                                    Irbis.Irbis.displayUI = true;
                                    //Irbis.Irbis.WriteLine("    loading " + Irbis.Irbis.savefile.lastPlayedLevel);
                                    //game.LoadLevel(Irbis.Irbis.savefile.lastPlayedLevel, true);
                                }
                            }
                            break;
                        case 2:                         //2 == Options
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Options");
                            game.LoadMenu(1, 0, false);
                            break;
                        case 3:                         //3 == Credits
                            if (!Irbis.Irbis.creditsActive)
                            {
                                Irbis.Irbis.RollCredits();
                                Irbis.Irbis.WriteLine("    Credits");
                            }
                            else
                            {
                                Irbis.Irbis.creditsActive = Irbis.Irbis.rollCredits = false;
                                Irbis.Irbis.buttonList[3].originalStatement = "Credits";
                                Irbis.Irbis.buttonList[3].highlightStatement = ">CREDITS";
                                Irbis.Irbis.WriteLine("    Exit credits");
                            }
                            break;
                        case 4:                         //4 == Quit
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Quit();");
                            game.Quit();
                            break;
                        default:
                            Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                            break;
                    }
                }
                if (Irbis.Irbis.GetPauseKeyDown || Irbis.Irbis.GetBackKeyDown)
                {
                    if (Irbis.Irbis.creditsActive)
                    { game.PauseCredits(); }
                    else if (Irbis.Irbis.levelLoaded > 0)
                    {
                        Irbis.Irbis.buttonList.Clear();
                        Irbis.Irbis.WriteLine("    Continue");
                        Irbis.Irbis.sceneIsMenu = false;
                        if (Irbis.Irbis.debug <= 0) { game.IsMouseVisible = false; }
                    }
                    else
                    { game.Quit(); }
                }
                break;
            case 1:     //options
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                    { Irbis.Irbis.menuSelection = i; }
                }
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (i == Irbis.Irbis.menuSelection)
                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                    else
                    { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                }
                if ((Irbis.Irbis.GetDownKeyDown) || (Irbis.Irbis.GetRightKeyDown))
                { Irbis.Irbis.menuSelection++;                }
                if ((Irbis.Irbis.GetUpKeyDown) || (Irbis.Irbis.GetLeftKeyDown))
                { Irbis.Irbis.menuSelection--; }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                { Irbis.Irbis.menuSelection = 0; }
                if (Irbis.Irbis.menuSelection < 0)
                { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                //game DECIDES WHAT EACH BUTTON DOES
                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                {
                    switch (Irbis.Irbis.menuSelection)
                    {
                        case 0:                         //0 == Controls
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Controls");
                            game.LoadMenu(2, 0, false);
                            break;
                        case 1:                         //1 == Camera
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Camera");
                            game.LoadMenu(3, 0, false);
                            break;
                        case 2:                         //2 == Video
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Video");
                            game.LoadMenu(4, 0, false);
                            break;
                        case 3:                         //3 == Audio
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Audio");
                            game.LoadMenu(5, 0, false);
                            break;
                        case 4:                         //4 == Misc
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Misc");
                            game.LoadMenu(6, 0, false);
                            break;

                        case 5:                         //5 == Back
                            game.LoadMenu(0, 2, false);
                            break;

                        default:
                            Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                            break;
                    }
                }
                if (Irbis.Irbis.GetBackKeyDown)
                { game.LoadMenu(0, 2, false); }
                break;
            case 2:     //options - controls
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("_");
                    if (Irbis.Irbis.GetBackKeyDown)
                    { Irbis.Irbis.listenForNewKeybind = false; }
                    else if (Irbis.Irbis.GetKeyboardState.GetPressedKeys().Length > 0 && Irbis.Irbis.GetPreviousKeyboardState.GetPressedKeys().Length <= 0)
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == Attack
                                Irbis.Irbis.attackKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 1:                         //1 == Jump
                                Irbis.Irbis.jumpKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 2:                         //2 == Roll
                                Irbis.Irbis.rollKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 3:                         //3 == Potion
                                Irbis.Irbis.potionKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 4:                         //4 == Shield
                                Irbis.Irbis.shieldKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 5:                         //5 == Shockwave
                                Irbis.Irbis.shockwaveKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 6:                         //6 == Up
                                Irbis.Irbis.upKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 7:                         //7 == Down
                                Irbis.Irbis.downKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 8:                         //8 == Left
                                Irbis.Irbis.leftKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 9:                         //9 == Right
                                Irbis.Irbis.rightKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 10:                         //9 == Right
                                Irbis.Irbis.useKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            // ALTS FROM HERE DOWN
                            case 11:                         //0 == Attack
                                Irbis.Irbis.altAttackKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 12:                         //1 == Jump
                                Irbis.Irbis.altJumpKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 13:                         //2 == Roll
                                Irbis.Irbis.altRollKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 14:                         //3 == Potion
                                Irbis.Irbis.altPotionKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 15:                         //4 == Shield
                                Irbis.Irbis.altShieldKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 16:                         //5 == Shockwave
                                Irbis.Irbis.altShockwaveKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 17:                         //6 == Up
                                Irbis.Irbis.altUpKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 18:                         //7 == Down
                                Irbis.Irbis.altDownKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 19:                         //8 == Left
                                Irbis.Irbis.altLeftKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 20:                         //9 == Right
                                Irbis.Irbis.altRightKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            case 21:                         //9 == Use
                                Irbis.Irbis.altUseKey = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0];
                                break;
                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.GetKeyboardState.GetPressedKeys()[0].ToString();
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement + "<";

                        Irbis.Irbis.listenForNewKeybind = false;
                    }
                }
                else
                {
                    for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                    {
                        if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                        { Irbis.Irbis.menuSelection = i; }
                        if (i == Irbis.Irbis.menuSelection)
                        { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                        else
                        { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                    }
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection == 10)
                        { Irbis.Irbis.menuSelection += 11; }
                        Irbis.Irbis.menuSelection++;
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection < 11)
                        { Irbis.Irbis.menuSelection += 11; }
                        else if (Irbis.Irbis.menuSelection < 22)
                        { Irbis.Irbis.menuSelection -= 11; }
                        else
                        { Irbis.Irbis.menuSelection++; }
                    }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    { Irbis.Irbis.menuSelection--; }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection < 11)
                        { Irbis.Irbis.menuSelection += 11; }
                        else if (Irbis.Irbis.menuSelection < 22)
                        { Irbis.Irbis.menuSelection -= 11; }
                        else
                        { Irbis.Irbis.menuSelection--; }
                    }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    { Irbis.Irbis.menuSelection = 0; }
                    if (Irbis.Irbis.menuSelection < 0)
                    { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                    //game DECIDES WHAT EACH BUTTON DOES
                    if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 22: //save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 0, false);
                                break;
                            case 23: //cancel
                                game.LoadMenu(1, 0, false);
                                break;
                            default:
                                Irbis.Irbis.listenForNewKeybind = true;
                                break;
                        }
                    }

                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 0, false);
                    }
                }
                break;
            case 3:     //options - camera
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 5)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('.')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8) //-
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if (char.IsDigit(Irbis.Irbis.textInputBuffer[0]) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (Irbis.Irbis.GetEnterKeyDown))
                    {
                        Irbis.Irbis.acceptTextInput = false;
                        Irbis.Irbis.listenForNewKeybind = false;
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == Anchor X
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                else
                                {
                                    Irbis.Irbis.boundingBox.X = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement) - (Irbis.Irbis.boundingBox.Width / 2);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.boundingBox.Center.X.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.boundingBox.Center.X.ToString() + "<";
                                }
                                break;
                            case 1:                         //1 == Anchor Y
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Irbis.Irbis.boundingBox.Y = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement) - (Irbis.Irbis.boundingBox.Height / 2);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.boundingBox.Center.Y.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.boundingBox.Center.Y.ToString() + "<";
                                }
                                break;
                            case 2:                         //2 == Width
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Point oldCenter = Irbis.Irbis.boundingBox.Center;
                                    Irbis.Irbis.boundingBox.Width = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    Irbis.Irbis.boundingBox.X = oldCenter.X - (Irbis.Irbis.boundingBox.Width / 2);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.boundingBox.Width.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.boundingBox.Width.ToString() + "<";
                                }

                                break;
                            case 3:                         //3 == Height
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Point oldCenter = Irbis.Irbis.boundingBox.Center;
                                    Irbis.Irbis.boundingBox.Height = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    Irbis.Irbis.boundingBox.Y = oldCenter.Y - (Irbis.Irbis.boundingBox.Height / 2);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.boundingBox.Height.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.boundingBox.Height.ToString() + "<";
                                }

                                break;
                            //case 4:                         //4 == Camera Lerp

                            //    break;
                            case 5:                         //5 == Lerp X Speed
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    float floatResult;
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    { Irbis.Irbis.cameraLerpXSpeed = floatResult; }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraLerpXSpeed.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraLerpXSpeed.ToString() + "<";
                                }
                                break;
                            case 6:                         //6 == Lerp Y Speed
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    float floatResult;
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    { Irbis.Irbis.cameraLerpYSpeed = floatResult; }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraLerpYSpeed.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraLerpYSpeed.ToString() + "<";
                                }
                                break;

                            //    break;
                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }
                        Irbis.Irbis.printList[0].Update(new Point(Irbis.Irbis.boundingBox.Center.X, Irbis.Irbis.boundingBox.Center.Y));
                    }
                }
                else
                {
                    for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                    {
                        if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                        { Irbis.Irbis.menuSelection = i; }
                    }
                    for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                    {
                        if (i == Irbis.Irbis.menuSelection)
                        { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                        else
                        { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                    }
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if ((Irbis.Irbis.menuSelection >= 0 && Irbis.Irbis.menuSelection <= 2) || Irbis.Irbis.menuSelection == 5)
                        { Irbis.Irbis.menuSelection += 2; }
                        else
                        { Irbis.Irbis.menuSelection++;                        }
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    { Irbis.Irbis.menuSelection++; }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    {
                        if ((Irbis.Irbis.menuSelection >= 1 && Irbis.Irbis.menuSelection <= 3) || Irbis.Irbis.menuSelection == 6)
                        { Irbis.Irbis.menuSelection -= 2; }
                        else
                        { Irbis.Irbis.menuSelection--; }
                    }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    { Irbis.Irbis.menuSelection--; }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    { Irbis.Irbis.menuSelection = 0; }
                    if (Irbis.Irbis.menuSelection < 0)
                    { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                    //game DECIDES WHAT EACH BUTTON DOES
                    if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == Anchor X
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 1:                         //1 == Anchor Y
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 2:                         //2 == Width
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 3:                         //3 == Height
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 4:                         //4 == smartCamera
                                Irbis.Irbis.smartCamera = !Irbis.Irbis.smartCamera;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.smartCamera.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.smartCamera.ToString() + "<";
                                break;
                            case 5:                         //5 == Lerp Speed
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 6:                         //6 == Lerp Speed
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 7:                         //6 == cameraShake
                                Irbis.Irbis.cameraShakeSetting = !Irbis.Irbis.cameraShakeSetting;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraShakeSetting.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraShakeSetting.ToString() + "<";
                                break;
                            case 8: //save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 1, false);
                                break;
                            case 9: //cancel
                                game.LoadMenu(1, 1, false);
                                break;

                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }


                    }

                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 1, false);
                    }
                }
                break;
            case 4:     //options - video
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 2)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('\u002e')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if (char.IsDigit(Irbis.Irbis.textInputBuffer[0]) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (Irbis.Irbis.GetEnterKeyDown))
                    {
                        Irbis.Irbis.acceptTextInput = false;
                        Irbis.Irbis.listenForNewKeybind = false;
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 2:                         //2 == Screen Scale
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    if (float.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement) > 100)
                                    {
                                        Irbis.Irbis.SetScreenScale(100);
                                    }
                                    else
                                    {
                                        Irbis.Irbis.SetScreenScale(float.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement));
                                    }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.screenScale.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.screenScale.ToString() + "<";
                                    if (!Irbis.Irbis.resetRequired)
                                    {
                                        Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.textScale*/;
                                        Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.textScale*/;
                                        Irbis.Irbis.graphics.ApplyChanges();
                                    }
                                }
                                break;
                            case 3:                         //3 == Irbis.Irbis.resolution.X
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Irbis.Irbis.tempResolution.X = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.tempResolution.X.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.tempResolution.X.ToString() + "<";
                                    PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                    Irbis.Irbis.resetRequired = true;
                                    game.LoadMenu(4, 8, false);
                                }
                                break;
                            case 4:                         //4 == Irbis.Irbis.resolution.Y
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Irbis.Irbis.tempResolution.Y = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.tempResolution.Y.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.tempResolution.Y.ToString() + "<";
                                    PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                    Irbis.Irbis.resetRequired = true;
                                    game.LoadMenu(4, 9, false);
                                }
                                break;
                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }
                    }
                }
                else
                {
                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 2, false);
                    }
                    for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                    {
                        if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                        { Irbis.Irbis.menuSelection = i; }
                    }
                    for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                    {
                        if (i == Irbis.Irbis.menuSelection)
                        { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                        else
                        { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                    }
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection >= 0 && Irbis.Irbis.menuSelection <= 1)
                        { Irbis.Irbis.menuSelection = 2; }
                        else if (Irbis.Irbis.menuSelection == 2)
                        { Irbis.Irbis.menuSelection = 3; }
                        else if (Irbis.Irbis.menuSelection >= 3 && Irbis.Irbis.menuSelection <= 4)
                        { Irbis.Irbis.menuSelection = 5; }
                        else
                        { Irbis.Irbis.menuSelection++; }
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    { Irbis.Irbis.menuSelection++; }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection >= 0 && Irbis.Irbis.menuSelection <= 1)
                        { Irbis.Irbis.menuSelection = 7; }
                        else if (Irbis.Irbis.menuSelection == 2)
                        { Irbis.Irbis.menuSelection = 0; }
                        else if (Irbis.Irbis.menuSelection >= 3 && Irbis.Irbis.menuSelection <= 4)
                        { Irbis.Irbis.menuSelection = 2; }
                        else if (Irbis.Irbis.menuSelection == 5)
                        { Irbis.Irbis.menuSelection = 3; }
                        else
                        { Irbis.Irbis.menuSelection--; }
                    }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    { Irbis.Irbis.menuSelection--; }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    { Irbis.Irbis.menuSelection = 0; }
                    if (Irbis.Irbis.menuSelection < 0)
                    { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                    //game DECIDES WHAT EACH BUTTON DOES
                    if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == windowed
                                Irbis.Irbis.graphics.IsFullScreen = false;
                                Irbis.Irbis.buttonList[0].buttonStatement = Irbis.Irbis.buttonList[0].highlightStatement;
                                Irbis.Irbis.buttonList[1].buttonStatement = Irbis.Irbis.buttonList[1].originalStatement;
                                Irbis.Irbis.sList[0].drawTex = Irbis.Irbis.sList[1].drawTex = true;
                                Irbis.Irbis.sList[2].drawTex = Irbis.Irbis.sList[3].drawTex = false;
                                
                                break;
                            case 1:                         //1 == fullscreen
                                                            //playerSettings.fullscreen = Irbis.Irbis.graphics.IsFullScreen = true;
                                Irbis.Irbis.buttonList[0].buttonStatement = Irbis.Irbis.buttonList[0].originalStatement;
                                Irbis.Irbis.buttonList[1].buttonStatement = Irbis.Irbis.buttonList[1].highlightStatement;
                                Irbis.Irbis.sList[0].drawTex = Irbis.Irbis.sList[1].drawTex = false;
                                Irbis.Irbis.sList[2].drawTex = Irbis.Irbis.sList[3].drawTex = true;
                                break;
                            case 2:                         //2 == ___x
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 3:                         //3 == Irbis.Irbis.resolution.X
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 4:                         //4 == Irbis.Irbis.resolution.Y
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 5:                         //5 == vSync
                                Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace = game.IsFixedTimeStep = !game.IsFixedTimeStep;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = game.IsFixedTimeStep.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + game.IsFixedTimeStep.ToString() + "<";
                                break;
                            case 6:                         //6 == lighting
                                Irbis.Irbis.lightingEnabled = !Irbis.Irbis.lightingEnabled;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.lightingEnabled.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.lightingEnabled.ToString() + "<";
                                break;
                            case 7:                         //Save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 2, false);
                                break;
                            case 8:                         //Cancel
                                game.LoadMenu(1, 2, false);
                                break;

                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }
                    }
                }
                break;
            case 5:     //options - audio
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 5)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('\u002e')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('\u002e')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false); }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (Irbis.Irbis.GetEnterKeyDown))
                    {
                        Irbis.Irbis.acceptTextInput = false;
                        Irbis.Irbis.listenForNewKeybind = false;
                        float floatResult;
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == master audio
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    {
                                        if (floatResult > 100f)
                                        { Irbis.Irbis.masterAudioLevel = 100f; }
                                        else
                                        { Irbis.Irbis.masterAudioLevel = floatResult; }
                                    }
                                    else
                                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.masterAudioLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.masterAudioLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.masterAudioLevel);
                                    MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                                    //update sound effects volume
                                }
                                break;
                            case 1:                         //1 == music
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                else
                                {
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    {
                                        if (floatResult > 100f)
                                        { Irbis.Irbis.musicLevel = 100f; }
                                        else
                                        { Irbis.Irbis.musicLevel = floatResult; }
                                    }
                                    else
                                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.musicLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.musicLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.musicLevel);
                                    MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                                }
                                break;
                            case 2:                         //2 == sound effects
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                else
                                {
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    {
                                        if (floatResult > 100f)
                                        { Irbis.Irbis.soundEffectsLevel = 100f; }
                                        else
                                        { Irbis.Irbis.soundEffectsLevel = floatResult; }
                                    }
                                    else
                                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement; }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.soundEffectsLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.soundEffectsLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.soundEffectsLevel);
                                    //update sound effects volume
                                }
                                break;
                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }
                    }
                }
                else
                {
                    if (Irbis.Irbis.GetBackKeyDown)
                    {
                        Irbis.Irbis.sliderList.Clear();
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 3, false);
                    }
                    if (Irbis.Irbis.sliderPressed < 0)
                    {
                        for (int i = 0; i < Irbis.Irbis.sliderList.Count; i++)
                        {
                            if (Irbis.Irbis.sliderList[i].Pressed(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetPreviousMouseState.LeftButton != ButtonState.Pressed)
                            {
                                Irbis.Irbis.sliderPressed = i;
                            }
                        }
                    }
                    else if (Irbis.Irbis.GetMouseState.LeftButton != ButtonState.Pressed)
                    {
                        Irbis.Irbis.sliderPressed = -1;
                    }






                    switch (Irbis.Irbis.sliderPressed)
                    {
                        case 0:
                            Irbis.Irbis.masterAudioLevel = ((float)(Irbis.Irbis.GetMouseState.X - Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Left) / (float)(Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Width)) * 100;
                            if (Irbis.Irbis.masterAudioLevel >= 100f)
                            {
                                Irbis.Irbis.masterAudioLevel = 100f;
                            }
                            else if (Irbis.Irbis.masterAudioLevel <= 0f)
                            {
                                Irbis.Irbis.masterAudioLevel = 0f;
                            }
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].originalStatement = Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement = Irbis.Irbis.masterAudioLevel.ToString("0.0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].highlightStatement = ">" + Irbis.Irbis.masterAudioLevel.ToString("0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].Update(Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement);
                            Irbis.Irbis.sliderList[Irbis.Irbis.sliderPressed].UpdateValue(Irbis.Irbis.masterAudioLevel);
                            MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                            //update sound effects volume
                            break;
                        case 1:
                            Irbis.Irbis.musicLevel = ((float)(Irbis.Irbis.GetMouseState.X - Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Left) / (float)(Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Width)) * 100;
                            if (Irbis.Irbis.musicLevel >= 100f)
                            {
                                Irbis.Irbis.musicLevel = 100f;
                            }
                            else if (Irbis.Irbis.musicLevel <= 0f)
                            {
                                Irbis.Irbis.musicLevel = 0f;
                            }
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].originalStatement = Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement = Irbis.Irbis.musicLevel.ToString("0.0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].highlightStatement = ">" + Irbis.Irbis.musicLevel.ToString("0.0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].Update(Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement);
                            Irbis.Irbis.sliderList[Irbis.Irbis.sliderPressed].UpdateValue(Irbis.Irbis.musicLevel);
                            MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                            break;
                        case 2:
                            Irbis.Irbis.soundEffectsLevel = ((float)(Irbis.Irbis.GetMouseState.X - Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Left) / (float)(Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].bounds.Width)) * 100;
                            if (Irbis.Irbis.soundEffectsLevel >= 100f)
                            {
                                Irbis.Irbis.soundEffectsLevel = 100f;
                            }
                            else if (Irbis.Irbis.soundEffectsLevel <= 0f)
                            {
                                Irbis.Irbis.soundEffectsLevel = 0f;
                            }
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].originalStatement = Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement = Irbis.Irbis.soundEffectsLevel.ToString("0.0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].highlightStatement = ">" + Irbis.Irbis.soundEffectsLevel.ToString("0.0");
                            Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].Update(Irbis.Irbis.buttonList[Irbis.Irbis.sliderPressed].buttonStatement);
                            Irbis.Irbis.sliderList[Irbis.Irbis.sliderPressed].UpdateValue(Irbis.Irbis.soundEffectsLevel);
                            //update sound effects volume
                            break;
                        default:
                            for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                            {
                                if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                                {
                                    Irbis.Irbis.menuSelection = i;
                                }
                                if (i < Irbis.Irbis.sliderList.Count)
                                {
                                    if (Irbis.Irbis.sliderList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                                    {
                                        Irbis.Irbis.menuSelection = i;
                                    }
                                }
                            }
                            if ((Irbis.Irbis.GetDownKeyDown))
                            {
                                Irbis.Irbis.menuSelection++;
                            }
                            if ((Irbis.Irbis.GetRightKeyDown))
                            {
                                switch (Irbis.Irbis.menuSelection)
                                {
                                    case 0:
                                        Irbis.Irbis.masterAudioLevel += 5;
                                        if (Irbis.Irbis.masterAudioLevel >= 100f)
                                        {
                                            Irbis.Irbis.masterAudioLevel = 100f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.masterAudioLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.masterAudioLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.masterAudioLevel);
                                        break;
                                    case 1:
                                        Irbis.Irbis.musicLevel += 5;
                                        if (Irbis.Irbis.musicLevel >= 100f)
                                        {
                                            Irbis.Irbis.musicLevel = 100f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.musicLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.musicLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.musicLevel);
                                        break;
                                    case 2:
                                        Irbis.Irbis.soundEffectsLevel += 5;
                                        if (Irbis.Irbis.soundEffectsLevel >= 100f)
                                        {
                                            Irbis.Irbis.soundEffectsLevel = 100f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.soundEffectsLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.soundEffectsLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.soundEffectsLevel);
                                        break;
                                }
                                MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                                //update sound effects volume
                            }

                            if ((Irbis.Irbis.GetUpKeyDown))
                            {
                                Irbis.Irbis.menuSelection--;
                            }
                            if ((Irbis.Irbis.GetLeftKeyDown))
                            {
                                switch (Irbis.Irbis.menuSelection)
                                {
                                    case 0:
                                        Irbis.Irbis.masterAudioLevel -= 5;
                                        if (Irbis.Irbis.masterAudioLevel <= 0f)
                                        {
                                            Irbis.Irbis.masterAudioLevel = 0f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.masterAudioLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.masterAudioLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.masterAudioLevel);
                                        break;
                                    case 1:
                                        Irbis.Irbis.musicLevel -= 5;
                                        if (Irbis.Irbis.musicLevel <= 0f)
                                        {
                                            Irbis.Irbis.musicLevel = 0f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.musicLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.musicLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.musicLevel);
                                        break;
                                    case 2:
                                        Irbis.Irbis.soundEffectsLevel -= 5;
                                        if (Irbis.Irbis.soundEffectsLevel <= 0f)
                                        {
                                            Irbis.Irbis.soundEffectsLevel = 0f;
                                        }
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.soundEffectsLevel.ToString("0");
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.soundEffectsLevel.ToString("0");
                                        Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.soundEffectsLevel);
                                        break;
                                }
                                MediaPlayer.Volume = (Irbis.Irbis.masterAudioLevel * Irbis.Irbis.musicLevel) / 10000f;
                                //update sound effects volume
                            }
                            if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                            {
                                Irbis.Irbis.menuSelection = 0;
                            }
                            if (Irbis.Irbis.menuSelection < 0)
                            {
                                Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                            }

                            for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                            {
                                if (i == Irbis.Irbis.menuSelection)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                                }
                                else
                                {
                                    Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                                }
                            }

                            //game DECIDES WHAT EACH BUTTON DOES
                            if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                            {
                                switch (Irbis.Irbis.menuSelection)
                                {
                                    case 0:                         //7 == ___x
                                        Irbis.Irbis.acceptTextInput = true;
                                        Irbis.Irbis.listenForNewKeybind = true;
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                        break;
                                    case 1:                         //8 == Irbis.Irbis.resolution.X
                                        Irbis.Irbis.acceptTextInput = true;
                                        Irbis.Irbis.listenForNewKeybind = true;
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                        break;
                                    case 2:                         //9 == Irbis.Irbis.resolution.Y
                                        Irbis.Irbis.acceptTextInput = true;
                                        Irbis.Irbis.listenForNewKeybind = true;
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                        break;
                                    case 3:                         //Save
                                        Irbis.Irbis.sliderList.Clear();
                                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                        game.LoadMenu(1, 3, false);
                                        break;
                                    case 4:                         //Cancel
                                        Irbis.Irbis.sliderList.Clear();
                                        game.LoadMenu(1, 3, false);
                                        break;

                                    default:
                                        Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                        break;
                                }
                            }
                            break;
                    }

                }
                break;
            case 6:     //options - misc
                if (Irbis.Irbis.GetBackKeyDown)
                { game.LoadMenu(1, 4, false); }
                break;
            case 7:     //new game level select
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (Irbis.Irbis.buttonList[i].Contains(Irbis.Irbis.GetMouseState) && Irbis.Irbis.GetMouseState != Irbis.Irbis.GetPreviousMouseState)
                    { Irbis.Irbis.menuSelection = i; }
                }
                for (int i = 0; i < Irbis.Irbis.buttonList.Count; i++)
                {
                    if (i == Irbis.Irbis.menuSelection)
                    { Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper()); }
                    else
                    { Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement); }
                }
                if (Irbis.Irbis.menuSelection < Irbis.Irbis.levelList.Length &&
                    (Irbis.Irbis.GetDownKeyDown))
                { Irbis.Irbis.menuSelection++; }
                if (Irbis.Irbis.menuSelection > 1 &&
                    (Irbis.Irbis.GetUpKeyDown))
                { Irbis.Irbis.menuSelection--; }
                if (Irbis.Irbis.GetRightKeyDown)
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    { Irbis.Irbis.menuSelection = 2; }
                    else
                    { Irbis.Irbis.menuSelection = 0; }
                }
                if (Irbis.Irbis.GetLeftKeyDown)
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    { Irbis.Irbis.menuSelection = 2; }
                    else
                    { Irbis.Irbis.menuSelection = 0; }
                }
                if (Irbis.Irbis.isMenuScrollable)
                {
                    if (Irbis.Irbis.menuSelection == 1 && Irbis.Irbis.buttonList[1].data != Irbis.Irbis.levelList[0])
                    {
                        Irbis.Irbis.levelListCounter--;
                        //button 0 is back
                        for (int i = Irbis.Irbis.buttonList.Count - 1; i > 0; i--)
                        {
                            Irbis.Irbis.buttonList[i].data = Irbis.Irbis.buttonList[i - 1].data;
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i - 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i - 1].highlightStatement;
                        }

                        Irbis.Irbis.buttonList[1].data = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                        Irbis.Irbis.buttonList[1].originalStatement = Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter]);
                        Irbis.Irbis.buttonList[1].highlightStatement = ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter]);
                        Irbis.Irbis.menuSelection = 2;
                    }
                    else if (Irbis.Irbis.menuSelection >= Irbis.Irbis.maxButtonsOnScreen && Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].data != Irbis.Irbis.levelList[Irbis.Irbis.levelList.Length - 1])
                    {
                        Irbis.Irbis.levelListCounter++;
                        //button 0 is back
                        for (int i = 1; i < Irbis.Irbis.buttonList.Count - 1; i++)
                        {
                            Irbis.Irbis.buttonList[i].data = Irbis.Irbis.buttonList[i + 1].data;
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i + 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i + 1].highlightStatement;
                        }

                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].data = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement = Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1]);
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].highlightStatement = ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1]);
                        Irbis.Irbis.menuSelection = Irbis.Irbis.maxButtonsOnScreen - 1;
                    }

                    if (Irbis.Irbis.GetMouseState.ScrollWheelValue > Irbis.Irbis.GetPreviousMouseState.ScrollWheelValue && Irbis.Irbis.buttonList[1].data != Irbis.Irbis.levelList[0])      //scroll up
                    {
                        Irbis.Irbis.levelListCounter--;
                        //button 0 is back
                        for (int i = Irbis.Irbis.buttonList.Count - 1; i > 0; i--)
                        {
                            Irbis.Irbis.buttonList[i].data = Irbis.Irbis.buttonList[i - 1].data;
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i - 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i - 1].highlightStatement;
                        }
                        Irbis.Irbis.buttonList[1].data = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                        Irbis.Irbis.buttonList[1].originalStatement = Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter]);
                        Irbis.Irbis.buttonList[1].highlightStatement = ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter]);
                    }
                    if (Irbis.Irbis.GetMouseState.ScrollWheelValue < Irbis.Irbis.GetPreviousMouseState.ScrollWheelValue && Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].data != Irbis.Irbis.levelList[Irbis.Irbis.levelList.Length - 1])      //scroll down
                    {
                        Irbis.Irbis.levelListCounter++;
                        //button 0 is back
                        for (int i = 1; i < Irbis.Irbis.buttonList.Count - 1; i++)
                        {
                            Irbis.Irbis.buttonList[i].data = Irbis.Irbis.buttonList[i + 1].data;
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i + 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i + 1].highlightStatement;
                        }
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].data = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement = Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1]);
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].highlightStatement = ">" + Irbis.Irbis.GetLevelName(Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1]);
                    }
                }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                { Irbis.Irbis.menuSelection = 0; }
                if (Irbis.Irbis.menuSelection < 0)
                { Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1; }
                //game DECIDES WHAT EACH BUTTON DOES
                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    { game.LoadMenu(0, 0, false); }
                    else
                    {
                        Thread loadlevel = new Thread(new ParameterizedThreadStart(game.LoadLevel));
                        loadlevel.Start(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].data);
                        //game.LoadLevel(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].data, true);
                    }
                }
                if (Irbis.Irbis.GetBackKeyDown)
                { game.LoadMenu(0, 0, false); }
                break;
            default:
                Irbis.Irbis.WriteLine("Error. Irbis.Irbis.scene ID " + Irbis.Irbis.scene + " is not in MenuUpdate list");
                break;
        }
        return true;
    }
}
