using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Menu
{
    public bool Exists
    {
        get
        {
            return exists;
        }
    }
    private bool exists;
    public Menu ()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Menu.Menu"); }
        exists = true;
    }

    public void Create(int scene)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Menu.Create"); }
        Point tempLP;
        Point tempDP;
        switch (scene)
        {
            case 0:     //main menu
                tempLP = new Point(500, (int)(250 * Irbis.Irbis.screenScale));       //distance from the bottom right corner of the screen
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(000 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "New game", ">NEW GAME", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(050 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Continue", ">CONTINUE", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(100 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Options", ">OPTIONS", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(150 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Exit();", ">EXIT();", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 1:     //options menu
                tempLP = new Point(500, (int)(250 * Irbis.Irbis.screenScale));       //distance from the bottom right corner of the screen
                if (!Irbis.Irbis.resetRequired)
                {
                    Print op11t = new Print(Irbis.Irbis.resolution.X - 32, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - 32, Irbis.Irbis.resolution.Y - 26), Direction.right, 0.5f);
                    op11t.Update("For even more options and details, view the playerSettings.ini file");
                    Irbis.Irbis.printList.Add(op11t);
                }
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y + (int)(025 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Controls", ">Controls", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(025 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Camera", ">Camera", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(075 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Video", ">Video", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(125 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Audio", ">Audio", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (int)(175 * Irbis.Irbis.screenScale)), 500, 51), Direction.left, "Misc", ">Misc", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bBack", "<\u001bBack", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 2:     //options - controls
                tempLP = new Point(400, 314);       //distance from the bottom right corner of the screen
                tempDP = new Point(100, 24);
                Irbis.Irbis.listenForNewKeybind = false;

                for (int i = 0; i < 10; i++)
                {
                    if (i < 11)
                    {
                        Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 41), Irbis.Irbis.resolution.Y - (tempLP.Y - 3 - (24 * i))), 280, 1, false, true, true, 0.5f));
                        Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(27, 28, 32), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 42), Irbis.Irbis.resolution.Y - (tempLP.Y - 4 - (24 * i))), 280, 1, false, true, true, 0.5f));
                    }
                }
                Print op201t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 145), Irbis.Irbis.resolution.Y - (tempLP.Y + 48)), Direction.forward, 0.5f);
                op201t.Update("Key");
                Irbis.Irbis.printList.Add(op201t);
                Print op202t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 245), Irbis.Irbis.resolution.Y - (tempLP.Y + 48)), Direction.forward, 0.5f);
                op202t.Update("Alt.");
                Irbis.Irbis.printList.Add(op202t);

                Print op21t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y + 8)), Direction.right, 0.5f);
                op21t.Update("Attack");
                Irbis.Irbis.printList.Add(op21t);
                Print op22t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 16)), Direction.right, 0.5f);
                op22t.Update("Jump");
                Irbis.Irbis.printList.Add(op22t);
                Print op23t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 40)), Direction.right, 0.5f);
                op23t.Update("Roll");
                Irbis.Irbis.printList.Add(op23t);
                Print op24t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 64)), Direction.right, 0.5f);
                op24t.Update("Potion");
                Irbis.Irbis.printList.Add(op24t);
                Print op25t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 88)), Direction.right, 0.5f);
                op25t.Update("Shield");
                Irbis.Irbis.printList.Add(op25t);
                Print op26t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 112)), Direction.right, 0.5f);
                op26t.Update("Shockwave");
                Irbis.Irbis.printList.Add(op26t);
                Print op27t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 136)), Direction.right, 0.5f);
                op27t.Update("Up");
                Irbis.Irbis.printList.Add(op27t);
                Print op28t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 160)), Direction.right, 0.5f);
                op28t.Update("Down");
                Irbis.Irbis.printList.Add(op28t);
                Print op29t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 184)), Direction.right, 0.5f);
                op29t.Update("Left");
                Irbis.Irbis.printList.Add(op29t);
                Print op210t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 208)), Direction.right, 0.5f);
                op210t.Update("Right");
                Irbis.Irbis.printList.Add(op210t);
                Print op211t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 030), Irbis.Irbis.resolution.Y - (tempLP.Y - 232)), Direction.right, 0.5f);
                op211t.Update("Use");
                Irbis.Irbis.printList.Add(op211t);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y + 20), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.attackKey.ToString(), ">" + Irbis.Irbis.attackKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 4), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.jumpKey.ToString(), ">" + Irbis.Irbis.jumpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 28), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.rollKey.ToString(), ">" + Irbis.Irbis.rollKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 52), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.potionKey.ToString(), ">" + Irbis.Irbis.potionKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 76), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.shieldKey.ToString(), ">" + Irbis.Irbis.shieldKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 100), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.shockwaveKey.ToString(), ">" + Irbis.Irbis.shockwaveKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 124), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.upKey.ToString(), ">" + Irbis.Irbis.upKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 148), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.downKey.ToString(), ">" + Irbis.Irbis.downKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 172), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.leftKey.ToString(), ">" + Irbis.Irbis.leftKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 196), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.rightKey.ToString(), ">" + Irbis.Irbis.rightKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 220), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.useKey.ToString(), ">" + Irbis.Irbis.useKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y + 20), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altAttackKey.ToString(), ">" + Irbis.Irbis.altAttackKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 4), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altJumpKey.ToString(), ">" + Irbis.Irbis.altJumpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 28), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altRollKey.ToString(), ">" + Irbis.Irbis.altRollKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 52), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altPotionKey.ToString(), ">" + Irbis.Irbis.altPotionKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 76), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altShieldKey.ToString(), ">" + Irbis.Irbis.altShieldKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 100), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altShockwaveKey.ToString(), ">" + Irbis.Irbis.altShockwaveKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 124), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altUpKey.ToString(), ">" + Irbis.Irbis.altUpKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 148), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altDownKey.ToString(), ">" + Irbis.Irbis.altDownKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 172), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altLeftKey.ToString(), ">" + Irbis.Irbis.altLeftKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 196), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altRightKey.ToString(), ">" + Irbis.Irbis.altRightKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 195), Irbis.Irbis.resolution.Y - (tempLP.Y - 220), tempDP.X, tempDP.Y), Direction.forward, Irbis.Irbis.altUseKey.ToString(), ">" + Irbis.Irbis.altUseKey.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(100, Irbis.Irbis.resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 3:     //options - camera
                Print BB = new Print(Irbis.Irbis.boundingBox.Width, Irbis.Irbis.font, Color.Magenta, false, new Point(Irbis.Irbis.boundingBox.Center.X, Irbis.Irbis.boundingBox.Center.Y), Direction.forward, 0.5f);
                BB.Update("Bounding Box");
                Irbis.Irbis.printList.Add(BB);

                Irbis.Irbis.listenForNewKeybind = false;
                tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                tempDP = new Point(75, 24);
                Print op31t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y + 000)), Direction.forward, 0.5f);
                op31t.Update("Bounding Box (anchor)");
                Irbis.Irbis.printList.Add(op31t);
                Print op31at = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 50), Irbis.Irbis.resolution.Y - (tempLP.Y - 18)), Direction.forward, 0.5f);
                op31at.Update("X:");
                Irbis.Irbis.printList.Add(op31at);
                Print op31bt = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 12), Irbis.Irbis.resolution.Y - (tempLP.Y - 18)), Direction.forward, 0.5f);
                op31bt.Update("Y:");
                Irbis.Irbis.printList.Add(op31bt);
                Print op32t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 48)), Direction.forward, 0.5f);
                op32t.Update("Bounding Box (Width/Height)");
                Irbis.Irbis.printList.Add(op32t);
                Print op32at = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 50), Irbis.Irbis.resolution.Y - (tempLP.Y - 66)), Direction.forward, 0.5f);
                op32at.Update("X:");
                Irbis.Irbis.printList.Add(op32at);
                Print op32bt = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 12), Irbis.Irbis.resolution.Y - (tempLP.Y - 66)), Direction.forward, 0.5f);
                op32bt.Update("Y:");
                Irbis.Irbis.printList.Add(op32bt);
                Print op33t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 96)), Direction.forward, 0.5f);
                op33t.Update("Camera Lerp");
                Irbis.Irbis.printList.Add(op33t);
                Print op34t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 144)), Direction.forward, 0.5f);
                op34t.Update("Lerp Speed");
                Irbis.Irbis.printList.Add(op34t);
                Print op35t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 192)), Direction.forward, 0.5f);
                op35t.Update("Camera Shake");
                Irbis.Irbis.printList.Add(op35t);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 43), Irbis.Irbis.resolution.Y - (tempLP.Y - 10), 40, 16), Direction.forward, Irbis.Irbis.boundingBox.Center.X.ToString(), ">" + Irbis.Irbis.boundingBox.Center.X.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 19), Irbis.Irbis.resolution.Y - (tempLP.Y - 10), 40, 16), Direction.forward, Irbis.Irbis.boundingBox.Center.Y.ToString(), ">" + Irbis.Irbis.boundingBox.Center.Y.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 43), Irbis.Irbis.resolution.Y - (tempLP.Y - 58), 40, 16), Direction.forward, Irbis.Irbis.boundingBox.Width.ToString(), ">" + Irbis.Irbis.boundingBox.Width.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 19), Irbis.Irbis.resolution.Y - (tempLP.Y - 58), 40, 16), Direction.forward, Irbis.Irbis.boundingBox.Height.ToString(), ">" + Irbis.Irbis.boundingBox.Height.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 50, 16), Direction.forward, Irbis.Irbis.cameraLerp.ToString(), ">" + Irbis.Irbis.cameraLerp.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 154), 50, 16), Direction.forward, Irbis.Irbis.cameraLerpSpeed.ToString(), ">" + Irbis.Irbis.cameraLerpSpeed.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 202), 50, 16), Direction.forward, Irbis.Irbis.cameraShakeSetting.ToString(), ">" + Irbis.Irbis.cameraShakeSetting.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(100, Irbis.Irbis.resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                break;
            case 4:     //options - video
                Irbis.Irbis.listenForNewKeybind = false;
                tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                tempDP = new Point(75, 24);
                Print op41t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 48)), Direction.forward, 0.5f);
                op41t.Update("Display");
                Irbis.Irbis.printList.Add(op41t);
                Print op42t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 96)), Direction.forward, 0.5f);
                op42t.Update("Window Scale");
                Irbis.Irbis.printList.Add(op42t);
                Print op42at = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 95), Irbis.Irbis.resolution.Y - (tempLP.Y - 114)), Direction.forward, 0.5f);
                op42at.Update("x");
                Irbis.Irbis.printList.Add(op42at);
                Print op43t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 144)), Direction.forward, 0.5f);
                op43t.Update("Resolution");
                Irbis.Irbis.printList.Add(op43t);
                Print op43at = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 52), Irbis.Irbis.resolution.Y - (tempLP.Y - 162)), Direction.forward, 0.5f);
                op43at.Update("X:");
                Irbis.Irbis.printList.Add(op43at);
                Print op43bt = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X - 11), Irbis.Irbis.resolution.Y - (tempLP.Y - 162)), Direction.forward, 0.5f);
                op43bt.Update("Y:");
                Irbis.Irbis.printList.Add(op43bt);
                Print op46t = new Print(100, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 192)), Direction.forward, 0.5f);
                op46t.Update("vSync");
                Irbis.Irbis.printList.Add(op46t);

                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point(Irbis.Irbis.resolution.X - (tempLP.X + 83), Irbis.Irbis.resolution.Y - (tempLP.Y - 73)), 65, 1, false, true, true, 0.5f));
                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(027, 028, 032), new Point(Irbis.Irbis.resolution.X - (tempLP.X + 82), Irbis.Irbis.resolution.Y - (tempLP.Y - 74)), 65, 1, false, true, true, 0.5f));

                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(223, 227, 236), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 73)), 69, 1, false, true, true, 0.5f));
                Irbis.Irbis.sList.Add(new Square(Irbis.Irbis.nullTex, new Color(027, 028, 032), new Point(Irbis.Irbis.resolution.X - (tempLP.X - 26), Irbis.Irbis.resolution.Y - (tempLP.Y - 74)), 69, 1, false, true, true, 0.5f));

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

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 90), Irbis.Irbis.resolution.Y - (tempLP.Y - 58), 80, 16), Direction.forward, "windowed", ">windowed<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 20), Irbis.Irbis.resolution.Y - (tempLP.Y - 58), 80, 16), Direction.forward, "fullscreen", ">fullscreen<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 90), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "1x", ">1x<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 60), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "2x", ">2x<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 30), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "3x", ">3x<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 00), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "4x", ">4x<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 30), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "5x", ">5x<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 60), Irbis.Irbis.resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "", ">  <", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 45), Irbis.Irbis.resolution.Y - (tempLP.Y - 154), 40, 16), Direction.forward, Irbis.Irbis.tempResolution.X.ToString(), ">" + Irbis.Irbis.tempResolution.X.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X - 18), Irbis.Irbis.resolution.Y - (tempLP.Y - 154), 40, 16), Direction.forward, Irbis.Irbis.tempResolution.Y.ToString(), ">" + Irbis.Irbis.tempResolution.Y.ToString() + "<", new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 25), Irbis.Irbis.resolution.Y - (tempLP.Y - 200), 50, 20), Direction.forward, Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace.ToString(), ">" + Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace.ToString() + "<", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(100, Irbis.Irbis.resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));

                break;
            case 5:     //options - audio
                Irbis.Irbis.listenForNewKeybind = false;
                tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                tempDP = new Point(75, 24);
                Print op52t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 48)), Direction.forward, 0.5f);
                op52t.Update("Master");
                Irbis.Irbis.printList.Add(op52t);
                Print op53t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 96)), Direction.forward, 0.5f);
                op53t.Update("Music");
                Irbis.Irbis.printList.Add(op53t);
                Print op54t = new Print(200, Irbis.Irbis.font, Color.White, false, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 000), Irbis.Irbis.resolution.Y - (tempLP.Y - 144)), Direction.forward, 0.5f);
                op54t.Update("Sound Effects");
                Irbis.Irbis.printList.Add(op54t);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 170), Irbis.Irbis.resolution.Y - (tempLP.Y - 060), 40, 16), Direction.forward, Irbis.Irbis.masterAudioLevel.ToString("0"), ">" + Irbis.Irbis.masterAudioLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.left, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 125), Irbis.Irbis.resolution.Y - (tempLP.Y - 058)), 250, 20, 100, Color.Red, new Color(166, 030, 030), Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.font, false, false, 0.905f, 0.95f, 0.4f));
                Irbis.Irbis.sliderList[0].UpdateValue(Irbis.Irbis.masterAudioLevel);
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 170), Irbis.Irbis.resolution.Y - (tempLP.Y - 108), 40, 16), Direction.forward, Irbis.Irbis.musicLevel.ToString("0"), ">" + Irbis.Irbis.musicLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.left, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 125), Irbis.Irbis.resolution.Y - (tempLP.Y - 106)), 250, 20, 100, Color.Red, new Color(255, 170, 000), Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.font, false, false, 0.905f, 0.95f, 0.4f));
                Irbis.Irbis.sliderList[1].UpdateValue(Irbis.Irbis.musicLevel);
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - (tempLP.X + 170), Irbis.Irbis.resolution.Y - (tempLP.Y - 156), 40, 16), Direction.forward, Irbis.Irbis.soundEffectsLevel.ToString("0"), ">" + Irbis.Irbis.soundEffectsLevel.ToString("0"), new Color(223, 227, 236), Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, true, 0.5f));
                Irbis.Irbis.sliderList.Add(new UIElementSlider(Direction.left, new Point(Irbis.Irbis.resolution.X - (tempLP.X + 125), Irbis.Irbis.resolution.Y - (tempLP.Y - 154)), 250, 20, 100, Color.Red, new Color(000, 234, 255), Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.nullTex, Irbis.Irbis.font, false, false, 0.905f, 0.95f, 0.4f));
                Irbis.Irbis.sliderList[2].UpdateValue(Irbis.Irbis.soundEffectsLevel);

                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(100, Irbis.Irbis.resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 6:     //options - misc
                Console.WriteLine("Coming soon!\nHit escape to go back");

                //playerSettings = Load(".\\Content\\playerSettings.ini");
                //Print op51t = new Print(100, Irbis.Irbis.font, Color.White, false, new Vector2(400, 200), Direction.right, 0.5f);
                //op51t.Update("vSync");
                //Irbis.Irbis.printList.Add(op51t);
                //Button op51 = new Button(new Rectangle(400, 200, 200, 12), Direction.left, "Controls", ">Controls", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                //Irbis.Irbis.buttonList.Add(op51);
                //Button op52 = new Button(new Rectangle(400, 213, 200, 12), Direction.left, "Camera", ">Camera", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                //Irbis.Irbis.buttonList.Add(op52);
                //Button op53 = new Button(new Rectangle(400, 225, 200, 12), Direction.left, "Video", ">Video", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                //Irbis.Irbis.buttonList.Add(op53);
                //Button op54 = new Button(new Rectangle(400, 237, 200, 12), Direction.left, "Audio", ">Audio", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f);
                //Irbis.Irbis.buttonList.Add(op54);
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(100, Irbis.Irbis.resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                break;
            case 7:     //new game level select
                Irbis.Irbis.buttonList.Add(new Button(new Rectangle(32, Irbis.Irbis.resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bBack", "<\u001bBack", Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                Irbis.Irbis.levelList = Directory.GetFiles(".\\levels");
                Irbis.Irbis.levelListCounter = 0;
                int buttonHeight = 25;
                Irbis.Irbis.maxButtonsOnScreen = Irbis.Irbis.resolution.Y / 25;

                if (true)
                {
                    List<string> tempLevelList = new List<string>();

                    foreach (string s in Irbis.Irbis.levelList)
                    {
                        tempLevelList.Add(s);
                    }

                    for (int i = 0; i < tempLevelList.Count; i++)
                    {
                        if (tempLevelList[i].StartsWith(".\\levels\\") && tempLevelList[i].EndsWith(".lvl"))
                        {
                            tempLevelList[i] = tempLevelList[i].Substring(9);
                            tempLevelList[i] = tempLevelList[i].Remove(tempLevelList[i].Length - 4);
                        }
                        else
                        {
                            tempLevelList.RemoveAt(i);
                        }
                    }

                    Irbis.Irbis.levelList = tempLevelList.ToArray();
                }

                Irbis.Irbis.WriteLine("level files formatted properly:" + Irbis.Irbis.levelList.Length);

                if (Irbis.Irbis.levelList.Length > Irbis.Irbis.maxButtonsOnScreen)
                {
                    tempLP = new Point(500, Irbis.Irbis.resolution.Y);       //distance from the bottom right corner of the screen
                    Irbis.Irbis.isMenuScrollable = true;
                }
                else if (Irbis.Irbis.levelList.Length < 10)
                {
                    tempLP = new Point(500, (Irbis.Irbis.levelList.Length * 25) + ((10 - Irbis.Irbis.levelList.Length) * 25));
                    Irbis.Irbis.isMenuScrollable = false;
                }
                else
                {
                    tempLP = new Point(500, Irbis.Irbis.levelList.Length * 25);
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
                        Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (buttonHeight * i)), 500, buttonHeight), Direction.left, Irbis.Irbis.levelList[i], ">" + Irbis.Irbis.levelList[i], Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                    }
                }
                else
                {
                    for (int i = 0; i < Irbis.Irbis.levelList.Length; i++)
                    {
                        Irbis.Irbis.buttonList.Add(new Button(new Rectangle(Irbis.Irbis.resolution.X - tempLP.X, Irbis.Irbis.resolution.Y - (tempLP.Y - (buttonHeight * i)), 500, buttonHeight), Direction.left, Irbis.Irbis.levelList[i], ">" + Irbis.Irbis.levelList[i], Color.Magenta, Irbis.Irbis.nullTex, Irbis.Irbis.font, Color.Magenta, false, false, 0.5f));
                    }
                }

                //menuSelection = 1;
                break;
            default:
                Console.WriteLine("Error. Scene ID " + scene + " is not in LoadMenu list");
                break;
        }

    }

    public void Update(Irbis.Irbis game)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Menu.Update"); }
        switch (Irbis.Irbis.scene)
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
                    {
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                    }
                    else
                    {
                        Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                    }
                }
                if ((Irbis.Irbis.GetDownKeyDown) || (Irbis.Irbis.GetRightKeyDown))
                {
                    Irbis.Irbis.menuSelection++;
                }
                if ((Irbis.Irbis.GetUpKeyDown) || (Irbis.Irbis.GetLeftKeyDown))
                {
                    Irbis.Irbis.menuSelection--;
                }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                {
                    Irbis.Irbis.menuSelection = 0;
                }
                if (Irbis.Irbis.menuSelection < 0)
                {
                    Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                }
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
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Continue");
                            Irbis.Irbis.sceneIsMenu = false;
                            if (Irbis.Irbis.geralt == null) { Irbis.Irbis.levelEditor = true; return; }
                            if (Irbis.Irbis.levelLoaded > 0)        ///game MEANS A /TRUE/ LEVEL (one not loaded exclusively for the titlescreen) HAS ALREADY BEEN LOADED
                            {
                                if (Irbis.Irbis.debug <= 0) { game.IsMouseVisible = false; }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine("    loading " + Irbis.Irbis.savefile.lastPlayedLevel);
                                game.LoadLevel(Irbis.Irbis.savefile.lastPlayedLevel, true);
                            }
                            break;
                        case 2:                         //2 == Options
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Options");
                            game.LoadMenu(1, 0, false);
                            break;
                        case 3:                         //3 == Quit
                            Irbis.Irbis.buttonList.Clear();
                            Irbis.Irbis.WriteLine("    Quit();");
                            game.Quit();
                            break;
                        default:
                            Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                            break;
                    }
                }
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                {
                    if (Irbis.Irbis.levelLoaded > 0)
                    {
                        Irbis.Irbis.buttonList.Clear();
                        Irbis.Irbis.WriteLine("    Continue");
                        Irbis.Irbis.sceneIsMenu = false;
                        if (Irbis.Irbis.debug <= 0) { game.IsMouseVisible = false; }
                    }
                    else
                    {
                        game.Quit();
                    }
                }
                break;
            case 1:     //options
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
                    {
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                    }
                    else
                    {
                        Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                    }
                }
                if ((Irbis.Irbis.GetDownKeyDown) || (Irbis.Irbis.GetRightKeyDown))
                {
                    Irbis.Irbis.menuSelection++;
                }
                if ((Irbis.Irbis.GetUpKeyDown) || (Irbis.Irbis.GetLeftKeyDown))
                {
                    Irbis.Irbis.menuSelection--;
                }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                {
                    Irbis.Irbis.menuSelection = 0;
                }
                if (Irbis.Irbis.menuSelection < 0)
                {
                    Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                }
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
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                {
                    game.LoadMenu(0, 2, false);
                }
                break;
            case 2:     //options - controls
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("_");
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                    }
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
                        {
                            Irbis.Irbis.menuSelection = i;
                            //Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].highlightStatement.ToUpper());
                        }
                        if (i == Irbis.Irbis.menuSelection)
                        {
                            Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                        }
                        else
                        {
                            Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                        }
                    }
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection == 10)
                        {
                            Irbis.Irbis.menuSelection += 11;
                        }
                        Irbis.Irbis.menuSelection++;
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection < 11)
                        {
                            Irbis.Irbis.menuSelection += 11;
                        }
                        else if (Irbis.Irbis.menuSelection < 22)
                        {
                            Irbis.Irbis.menuSelection -= 11;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection++;
                        }
                    }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    {
                        Irbis.Irbis.menuSelection--;
                    }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection < 11)
                        {
                            Irbis.Irbis.menuSelection += 11;
                        }
                        else if (Irbis.Irbis.menuSelection < 22)
                        {
                            Irbis.Irbis.menuSelection -= 11;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection--;
                        }
                    }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    {
                        Irbis.Irbis.menuSelection = 0;
                    }
                    if (Irbis.Irbis.menuSelection < 0)
                    {
                        Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                    }
                    //game DECIDES WHAT EACH BUTTON DOES
                    if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 20: //save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 0, false);
                                break;
                            case 21: //cancel
                                game.LoadMenu(1, 0, false);
                                break;
                            default:
                                Irbis.Irbis.listenForNewKeybind = true;
                                break;
                        }
                    }

                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 0, false);
                    }
                }
                break;
            case 3:     //options - camera
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 5)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0])) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8) //-
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if (char.IsDigit(Irbis.Irbis.textInputBuffer[0]) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
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
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
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
                            case 5:                         //5 == Lerp Speed
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    Irbis.Irbis.cameraLerpSpeed = float.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraLerpSpeed.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraLerpSpeed.ToString() + "<";
                                }

                                break;
                            //case 6:                         //6 == Camera Shake

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
                        {
                            Irbis.Irbis.menuSelection = i;
                            //Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].highlightStatement.ToUpper());
                        }
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
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection >= 0 && Irbis.Irbis.menuSelection <= 2)
                        {
                            Irbis.Irbis.menuSelection += 2;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection++;
                        }
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    {
                        Irbis.Irbis.menuSelection++;
                    }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection >= 1 && Irbis.Irbis.menuSelection <= 3)
                        {
                            Irbis.Irbis.menuSelection -= 2;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection--;
                        }
                    }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    {
                        Irbis.Irbis.menuSelection--;
                    }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    {
                        Irbis.Irbis.menuSelection = 0;
                    }
                    if (Irbis.Irbis.menuSelection < 0)
                    {
                        Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                    }
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
                            case 4:                         //4 == Camera Lerp
                                Irbis.Irbis.cameraLerp = !Irbis.Irbis.cameraLerp;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraLerp.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraLerp.ToString() + "<";
                                break;
                            case 5:                         //5 == Lerp Speed
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 6:                         //6 == Camera Shake
                                Irbis.Irbis.cameraShakeSetting = !Irbis.Irbis.cameraShakeSetting;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.cameraShakeSetting.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.cameraShakeSetting.ToString() + "<";
                                break;
                            case 7: //save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 1, false);
                                break;
                            case 8: //cancel
                                game.LoadMenu(1, 1, false);
                                break;

                            default:
                                Irbis.Irbis.WriteLine("Error. Menu item " + Irbis.Irbis.menuSelection + " does not exist.");
                                break;
                        }


                    }

                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 1, false);
                    }
                }
                break;
            case 4:     //options - video
                if (Irbis.Irbis.listenForNewKeybind)
                {
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 5)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0])) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if (char.IsDigit(Irbis.Irbis.textInputBuffer[0]) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }

                    if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (Irbis.Irbis.GetEnterKeyDown))
                    {
                        Irbis.Irbis.acceptTextInput = false;
                        Irbis.Irbis.listenForNewKeybind = false;
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 7:                         //7 == Screen Scale
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    if (int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement) > 100)
                                    {
                                        Irbis.Irbis.screenScale = 100;
                                    }
                                    else
                                    {
                                        Irbis.Irbis.screenScale = int.Parse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement);
                                    }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.screenScale.ToString();
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.screenScale.ToString() + "<";
                                    if (!Irbis.Irbis.resetRequired)
                                    {
                                        Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                        Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                        Irbis.Irbis.graphics.ApplyChanges();
                                    }
                                }
                                break;
                            case 8:                         //8 == Irbis.Irbis.resolution.X
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
                            case 9:                         //7 == Irbis.Irbis.resolution.Y
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
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 2, false);
                    }
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
                        {
                            Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                        }
                        else
                        {
                            Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                        }
                    }
                    if ((Irbis.Irbis.GetDownKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection == 0)//Irbis.Irbis.menuSelection <= 1)
                        {
                            Irbis.Irbis.menuSelection = 3;
                        }
                        else if (Irbis.Irbis.menuSelection == 1)
                        {
                            Irbis.Irbis.menuSelection = 6;
                        }
                        else if (Irbis.Irbis.menuSelection >= 2 && Irbis.Irbis.menuSelection <= 4)
                        {
                            Irbis.Irbis.menuSelection = 8;
                        }
                        else if (Irbis.Irbis.menuSelection >= 5 && Irbis.Irbis.menuSelection <= 7)
                        {
                            Irbis.Irbis.menuSelection = 9;
                        }
                        else if (Irbis.Irbis.menuSelection >= 8 && Irbis.Irbis.menuSelection <= 9)
                        {
                            Irbis.Irbis.menuSelection = 10;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection++;
                        }
                    }
                    if ((Irbis.Irbis.GetRightKeyDown))
                    {
                        Irbis.Irbis.menuSelection++;
                    }

                    if ((Irbis.Irbis.GetUpKeyDown))
                    {
                        if (Irbis.Irbis.menuSelection >= 2 && Irbis.Irbis.menuSelection <= 4)
                        {
                            Irbis.Irbis.menuSelection = 0;
                        }
                        else if (Irbis.Irbis.menuSelection >= 5 && Irbis.Irbis.menuSelection <= 7)
                        {
                            Irbis.Irbis.menuSelection = 1;
                        }
                        else if (Irbis.Irbis.menuSelection == 8)
                        {
                            Irbis.Irbis.menuSelection = 3;
                        }
                        else if (Irbis.Irbis.menuSelection == 9)
                        {
                            Irbis.Irbis.menuSelection = 6;
                        }
                        else
                        {
                            Irbis.Irbis.menuSelection--;
                        }
                    }
                    if ((Irbis.Irbis.GetLeftKeyDown))
                    {
                        Irbis.Irbis.menuSelection--;
                    }

                    if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                    {
                        Irbis.Irbis.menuSelection = 0;
                    }
                    if (Irbis.Irbis.menuSelection < 0)
                    {
                        Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                    }
                    //game DECIDES WHAT EACH BUTTON DOES
                    if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                    {
                        switch (Irbis.Irbis.menuSelection)
                        {
                            case 0:                         //0 == windowed
                                Irbis.Irbis.graphics.IsFullScreen = false;
                                Irbis.Irbis.buttonList[0].buttonStatement = Irbis.Irbis.buttonList[0].highlightStatement;
                                Irbis.Irbis.buttonList[1].buttonStatement = Irbis.Irbis.buttonList[1].originalStatement;
                                Irbis.Irbis.sList[1].drawTex = Irbis.Irbis.sList[2].drawTex = true;
                                Irbis.Irbis.sList[3].drawTex = Irbis.Irbis.sList[4].drawTex = false;
                                break;
                            case 1:                         //1 == fullscreen
                                                            //playerSettings.fullscreen = Irbis.Irbis.graphics.IsFullScreen = true;
                                Irbis.Irbis.buttonList[0].buttonStatement = Irbis.Irbis.buttonList[0].originalStatement;
                                Irbis.Irbis.buttonList[1].buttonStatement = Irbis.Irbis.buttonList[1].highlightStatement;
                                Irbis.Irbis.sList[1].drawTex = Irbis.Irbis.sList[2].drawTex = false;
                                Irbis.Irbis.sList[3].drawTex = Irbis.Irbis.sList[4].drawTex = true;
                                break;
                            case 2:                         //2 == 1x
                                Irbis.Irbis.screenScale = 1;
                                if (!Irbis.Irbis.resetRequired)
                                {
                                    Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                    Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                }
                                Irbis.Irbis.graphics.ApplyChanges();
                                break;
                            case 3:                         //3 == 2x
                                Irbis.Irbis.screenScale = 2;
                                if (!Irbis.Irbis.resetRequired)
                                {
                                    Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                    Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                }
                                Irbis.Irbis.graphics.ApplyChanges();
                                break;
                            case 4:                         //4 == 3x
                                Irbis.Irbis.screenScale = 3;
                                if (!Irbis.Irbis.resetRequired)
                                {
                                    Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                    Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                }
                                Irbis.Irbis.graphics.ApplyChanges();
                                break;
                            case 5:                         //5 == 4x
                                Irbis.Irbis.screenScale = 4;
                                if (!Irbis.Irbis.resetRequired)
                                {
                                    Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                    Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                }
                                Irbis.Irbis.graphics.ApplyChanges();
                                break;
                            case 6:                         //6 == 5x
                                Irbis.Irbis.screenScale = 5;
                                if (!Irbis.Irbis.resetRequired)
                                {
                                    Irbis.Irbis.graphics.PreferredBackBufferHeight = Irbis.Irbis.resolution.Y /** Irbis.Irbis.screenScale*/;
                                    Irbis.Irbis.graphics.PreferredBackBufferWidth = Irbis.Irbis.resolution.X /** Irbis.Irbis.screenScale*/;
                                }
                                Irbis.Irbis.graphics.ApplyChanges();
                                break;
                            case 7:                         //7 == ___x
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 8:                         //8 == Irbis.Irbis.resolution.X
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 9:                         //9 == Irbis.Irbis.resolution.Y
                                Irbis.Irbis.acceptTextInput = true;
                                Irbis.Irbis.listenForNewKeybind = true;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update("", true);
                                break;
                            case 10:                         //10 == vSync
                                Irbis.Irbis.graphics.SynchronizeWithVerticalRetrace = game.IsFixedTimeStep = !game.IsFixedTimeStep;
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = game.IsFixedTimeStep.ToString();
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + game.IsFixedTimeStep.ToString() + "<";
                                break;
                            case 11:                         //Save
                                PlayerSettings.Save(game, @".\content\playerSettings.ini");
                                game.LoadMenu(1, 2, false);
                                break;
                            case 12:                         //Cancel
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
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        Irbis.Irbis.listenForNewKeybind = false;
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                    if (Irbis.Irbis.menuSelection == 5)
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('\u002e')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
                            Irbis.Irbis.textInputBuffer = Irbis.Irbis.textInputBuffer.Substring(1);
                        }
                    }
                    else
                    {
                        while (Irbis.Irbis.textInputBuffer.Length > 0)
                        {
                            if ((char.IsDigit(Irbis.Irbis.textInputBuffer[0]) || Irbis.Irbis.textInputBuffer[0].Equals('\u002e')) && Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length < 8)
                            {
                                Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.textInputBuffer[0].ToString(), false);
                            }
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
                                        {
                                            Irbis.Irbis.masterAudioLevel = 100f;
                                        }
                                        else
                                        {
                                            Irbis.Irbis.masterAudioLevel = floatResult;
                                        }
                                    }
                                    else
                                    {
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                    }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.masterAudioLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.masterAudioLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.masterAudioLevel);
                                }
                                break;
                            case 1:                         //1 == music
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    {
                                        if (floatResult > 100f)
                                        {
                                            Irbis.Irbis.musicLevel = 100f;
                                        }
                                        else
                                        {
                                            Irbis.Irbis.musicLevel = floatResult;
                                        }
                                    }
                                    else
                                    {
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                    }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.musicLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.musicLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.musicLevel);
                                }
                                break;
                            case 2:                         //2 == sound effects
                                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement.Length <= 0)
                                {
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                }
                                else
                                {
                                    if (float.TryParse(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement, out floatResult))
                                    {
                                        if (floatResult > 100f)
                                        {
                                            Irbis.Irbis.soundEffectsLevel = 100f;
                                        }
                                        else
                                        {
                                            Irbis.Irbis.soundEffectsLevel = floatResult;
                                        }
                                    }
                                    else
                                    {
                                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].buttonStatement = Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement;
                                    }
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement = Irbis.Irbis.soundEffectsLevel.ToString("0");
                                    Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement = ">" + Irbis.Irbis.soundEffectsLevel.ToString("0");
                                    Irbis.Irbis.sliderList[Irbis.Irbis.menuSelection].UpdateValue(Irbis.Irbis.soundEffectsLevel);
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
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                    {
                        Irbis.Irbis.sliderList.Clear();
                        PlayerSettings.Save(game, @".\content\playerSettings.ini");
                        game.LoadMenu(1, 3, false);
                    }
                    for (int i = 0; i < Irbis.Irbis.sliderList.Count; i++)
                    {
                        if (Irbis.Irbis.sliderList[i].Pressed(Irbis.Irbis.GetMouseState) && Irbis.Irbis.sliderPressed < 0)
                        {
                            Irbis.Irbis.sliderPressed = i;
                        }
                    }

                    if (Irbis.Irbis.GetMouseState.LeftButton != ButtonState.Pressed)
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
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                {
                    game.LoadMenu(1, 4, false);
                }
                break;
            case 7:     //new game level select
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
                    {
                        Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Update(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].highlightStatement.ToUpper());
                    }
                    else
                    {
                        Irbis.Irbis.buttonList[i].Update(Irbis.Irbis.buttonList[i].originalStatement);
                    }
                }
                if (Irbis.Irbis.menuSelection < Irbis.Irbis.levelList.Length &&
                    (Irbis.Irbis.GetDownKeyDown))
                {
                    Irbis.Irbis.menuSelection++;
                }
                if (Irbis.Irbis.menuSelection > 1 &&
                    (Irbis.Irbis.GetUpKeyDown))
                {
                    Irbis.Irbis.menuSelection--;
                }
                if (Irbis.Irbis.GetRightKeyDown)
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    {
                        Irbis.Irbis.menuSelection = 2;
                    }
                    else
                    {
                        Irbis.Irbis.menuSelection = 0;
                    }
                }
                if (Irbis.Irbis.GetLeftKeyDown)
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    {
                        Irbis.Irbis.menuSelection = 2;
                    }
                    else
                    {
                        Irbis.Irbis.menuSelection = 0;
                    }
                }
                if (Irbis.Irbis.isMenuScrollable)
                {
                    if (Irbis.Irbis.menuSelection == 1 && Irbis.Irbis.buttonList[1].originalStatement != Irbis.Irbis.levelList[0])
                    {
                        Irbis.Irbis.levelListCounter--;
                        //button 0 is back
                        for (int i = Irbis.Irbis.buttonList.Count - 1; i > 0; i--)
                        {
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i - 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i - 1].highlightStatement;
                        }

                        Irbis.Irbis.buttonList[1].originalStatement = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                        Irbis.Irbis.buttonList[1].highlightStatement = ">" + Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                        Irbis.Irbis.menuSelection = 2;
                    }
                    else if (Irbis.Irbis.menuSelection >= Irbis.Irbis.maxButtonsOnScreen && Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement != Irbis.Irbis.levelList[Irbis.Irbis.levelList.Length - 1])
                    {
                        Irbis.Irbis.levelListCounter++;
                        //button 0 is back
                        for (int i = 1; i < Irbis.Irbis.buttonList.Count - 1; i++)
                        {
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i + 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i + 1].highlightStatement;
                        }

                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].highlightStatement = ">" + Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                        Irbis.Irbis.menuSelection = Irbis.Irbis.maxButtonsOnScreen - 1;
                    }

                    if (Irbis.Irbis.GetMouseState.ScrollWheelValue > Irbis.Irbis.GetPreviousMouseState.ScrollWheelValue && Irbis.Irbis.buttonList[1].originalStatement != Irbis.Irbis.levelList[0])      //scroll up
                    {
                        Irbis.Irbis.levelListCounter--;
                        for (int i = Irbis.Irbis.buttonList.Count - 1; i > 0; i--)
                        {
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i - 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i - 1].highlightStatement;
                        }
                        Irbis.Irbis.buttonList[1].originalStatement = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                        Irbis.Irbis.buttonList[1].highlightStatement = ">" + Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter];
                    }
                    if (Irbis.Irbis.GetMouseState.ScrollWheelValue < Irbis.Irbis.GetPreviousMouseState.ScrollWheelValue && Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement != Irbis.Irbis.levelList[Irbis.Irbis.levelList.Length - 1])      //scroll down
                    {
                        Irbis.Irbis.levelListCounter++;
                        for (int i = 1; i < Irbis.Irbis.buttonList.Count - 1; i++)
                        {
                            Irbis.Irbis.buttonList[i].originalStatement = Irbis.Irbis.buttonList[i + 1].originalStatement;
                            Irbis.Irbis.buttonList[i].highlightStatement = Irbis.Irbis.buttonList[i + 1].highlightStatement;
                        }
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].originalStatement = Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                        Irbis.Irbis.buttonList[Irbis.Irbis.buttonList.Count - 1].highlightStatement = ">" + Irbis.Irbis.levelList[Irbis.Irbis.levelListCounter + Irbis.Irbis.maxButtonsOnScreen - 1];
                    }
                }
                if (Irbis.Irbis.menuSelection >= Irbis.Irbis.buttonList.Count)
                {
                    Irbis.Irbis.menuSelection = 0;
                }
                if (Irbis.Irbis.menuSelection < 0)
                {
                    Irbis.Irbis.menuSelection = Irbis.Irbis.buttonList.Count - 1;
                }
                //game DECIDES WHAT EACH BUTTON DOES
                if (Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].Pressed(Irbis.Irbis.GetMouseState, Irbis.Irbis.GetPreviousMouseState) || Irbis.Irbis.Use())
                {
                    if (Irbis.Irbis.menuSelection == 0)
                    {
                        game.LoadMenu(0, 0, false);
                    }
                    else
                    {
                        game.LoadLevel(Irbis.Irbis.buttonList[Irbis.Irbis.menuSelection].originalStatement, true);
                    }
                }
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */Irbis.Irbis.GetEscapeKeyDown)
                {
                    game.LoadMenu(0, 0, false);
                }
                break;
            default:
                Irbis.Irbis.WriteLine("Error. Irbis.Irbis.scene ID " + Irbis.Irbis.scene + " is not in MenuUpdate list");
                break;
        }
    }
}
