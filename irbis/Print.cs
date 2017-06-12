using Irbis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
public class Print
{
    Texture2D tex;
    Rectangle displayRect;
    
    Color fontColor;
    int width;
    int height;
    int maxWidth;
    List<Rectangle> fontSourceRect;
    public string statement;
    float depth;
    Point origin;
    Direction align;
    int characterHeight;
    bool monoSpace;
    public int lines;
    public float timer;

    public Print(int mW, Font font, Color colorForFont, bool monospace, float drawDepth)
    {
        bool debug = false;
        align = Direction.left;
        origin = Point.Zero;
        depth = drawDepth;
        tex = font.tex;
        width = 0;
        height = 0;
        maxWidth = mW;
        fontColor = colorForFont;
        characterHeight = font.charHeight;
        lines = 0;
        timer = 0f;

        monoSpace = monospace;

        statement = "";

        fontSourceRect = new List<Rectangle>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Rectangle tempRect;
                if (debug) { Console.WriteLine((i * 10) + j); }
                tempRect = new Rectangle((j * font.charHeight) + (font.charHeight - font.charWidth[(i * 10) + j]), i * font.charHeight, font.charWidth[(i * 10) + j], font.charHeight);
                if (debug) { Console.WriteLine(tempRect); }
                fontSourceRect.Add(tempRect);
            }
        }
        displayRect = new Rectangle(origin.X, origin.Y, characterHeight, characterHeight);
        /// <summary>
        ///for (int i = 0; i <= 96; i++)
        ///{
        ///    Rectangle tempRect;
        ///    if (i >= 0 && i < 10)
        ///    {
        ///        tempRect = new Rectangle(i * characterHeight, 0, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 10 && i < 20)
        ///    {
        ///        tempRect = new Rectangle((i - 10) * 8, 8, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 20 && i < 30)
        ///    {
        ///        tempRect = new Rectangle((i - 20) * 8, 16, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 30 && i < 40)
        ///    {
        ///        tempRect = new Rectangle((i - 30) * 8, 24, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 40 && i < 50)
        ///    {
        ///        tempRect = new Rectangle((i - 40) * 8, 32, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 50 && i < 60)
        ///    {
        ///        tempRect = new Rectangle((i - 50) * 8, 40, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 60 && i < 70)
        ///    {
        ///        tempRect = new Rectangle((i - 60) * 8, 48, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 70 && i < 80)
        ///    {
        ///        tempRect = new Rectangle((i - 70) * 8, 56, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 80 && i < 90)
        ///    {
        ///        tempRect = new Rectangle((i - 80) * 8, 64, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else if (i >= 90 && i < 100)
        ///    {
        ///        tempRect = new Rectangle((i - 90) * 8, 72, 8, 8);
        ///        fontSourceRect.Add(tempRect);
        ///    }
        ///    else
        ///    {
        ///        tempRect = new Rectangle(72, 72, 8, 8);
        ///        if (!fontSourceRect.Contains(tempRect))
        ///        {
        ///            fontSourceRect.Add(tempRect);
        ///        }
        ///    }
        ///}
        /// </summary>

    }

    public Print(int mW, Font font, Color colorForFont, bool monospace, Point location, Direction alignSide, float drawDepth)
    {
        bool debug = false;
        align = alignSide;
        depth = drawDepth;
        tex = font.tex;
        width = 0;
        height = 0;
        maxWidth = mW;
        fontColor = colorForFont;
        characterHeight = font.charHeight;
        timer = 0f;
        lines = 0;

        //if (align == Direction.forward)
        //{
            origin.X = location.X;
            origin.Y = location.Y - (characterHeight / 2);
        //}
        //else
        //{
        //    origin = location;
        //}

        monoSpace = monospace;

        statement = "";
        if (debug) { Console.WriteLine("     font: " + font.ToString()); }
        if (debug) { Console.WriteLine("    align: " + align); }
        if (debug) { Console.WriteLine("   origin: " + origin); }
        if (debug) { Console.WriteLine(" maxWidth: " + maxWidth); }
        if (debug) { Console.WriteLine("fontColor: " + fontColor); }
        if (debug) { Console.WriteLine("monoSpace: " + monoSpace); }

        fontSourceRect = new List<Rectangle>();

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Rectangle tempRect;
                //if (debug) { Console.WriteLine((i * 10) + j); }
                tempRect = new Rectangle((j * font.charHeight) + (font.charHeight - font.charWidth[(i * 10) + j]), i * font.charHeight, font.charWidth[(i * 10) + j], font.charHeight);
                //if (debug) { Console.WriteLine(tempRect); }
                fontSourceRect.Add(tempRect);
            }
        }
        displayRect = new Rectangle(origin.X, origin.Y, characterHeight, characterHeight);
    }

    public void Clear()
    {
        //fontSourceRect.Clear();
        statement = "";
    }

    public void Update(string input, bool clear)
    {
        //fontSourceRect.Clear();
        if (clear)
        {
            statement = input;
        }
        else
        {
            statement += input;
        }
    }

    public void Update(string input)
    {
        //fontSourceRect.Clear();
        statement += input;
    }

    public void Update(Point location)
    {
        origin = location;
    }

    public int ReturnCharacterIndex(char c)
    {
        switch (c)
        {
            case '\u001b'://ESCAPE
                return 79;

            case '\u0020'://SPACE
                return 99;

            case '\u0021'://!
                return 62;

            case '\u0022'://"
                return 94;

            case '\u0023'://#
                return 64;

            case '\u0024'://$
                return 65;

            case '\u0025'://%
                return 66;

            case '\u0026'://&
                return 68;

            case '\u0027'://'
                return 90;

            case '\u0028'://(
                return 70;

            case '\u0029'://)
                return 71;

            case '\u002a'://*
                return 69;

            case '\u002b'://+
                return 75;

            case '\u002c'://,
                return 85;

            case '\u002d'://-
                return 72;

            case '\u002e'://.
                return 83;

            case '\u002f':// /
                return 88;

            case '\u0030'://0
                return 00;

            case '\u0031'://1
                return 01;

            case '\u0032'://2
                return 02;

            case '\u0033'://3
                return 03;

            case '\u0034'://4
                return 04;

            case '\u0035'://5
                return 05;

            case '\u0036'://6
                return 06;

            case '\u0037'://7
                return 07;

            case '\u0038'://8
                return 08;

            case '\u0039'://9
                return 09;

            case '\u003a'://:
                return 86;

            case '\u003b'://;
                return 87;

            case '\u003c'://<
                return 77;

            case '\u003d'://=
                return 74;

            case '\u003e'://>
                return 76;

            case '\u003f'://?
                return 63;

            case '\u0040'://@
                return 89;

            case '\u0041'://A
                return 10;

            case '\u0042'://B
                return 11;

            case '\u0043'://C
                return 12;

            case '\u0044'://D
                return 13;

            case '\u0045'://E
                return 14;

            case '\u0046'://F
                return 15;

            case '\u0047'://G
                return 16;

            case '\u0048'://H
                return 17;

            case '\u0049'://I
                return 18;

            case '\u004a'://J
                return 19;

            case '\u004b'://K
                return 20;

            case '\u004c'://L
                return 21;

            case '\u004d'://M
                return 22;

            case '\u004e'://N
                return 23;

            case '\u004f'://O
                return 24;

            case '\u0050'://P
                return 25;

            case '\u0051'://Q
                return 26;

            case '\u0052'://R
                return 27;

            case '\u0053'://S
                return 28;

            case '\u0054'://T
                return 29;

            case '\u0055'://U
                return 30;

            case '\u0056'://V
                return 31;

            case '\u0057'://W
                return 32;

            case '\u0058'://X
                return 33;

            case '\u0059'://Y
                return 34;

            case '\u005a'://Z
                return 35;

            case '\u005b'://[
                return 91;

            case '\u005c'://\
                return 92;

            case '\u005d'://]
                return 93;

            case '\u005e'://^
                return 39;

            case '\u005f'://_
                return 73;

            case '\u0060'://`
                return 84;

            case '\u0061'://a
                return 36;

            case '\u0062'://b
                return 37;

            case '\u0063'://c
                return 38;

            case '\u0064'://d
                return 39;

            case '\u0065'://e
                return 40;

            case '\u0066'://f
                return 41;

            case '\u0067'://g
                return 42;

            case '\u0068'://h
                return 43;

            case '\u0069'://i
                return 44;

            case '\u006a'://j
                return 45;

            case '\u006b'://k
                return 46;

            case '\u006c'://l
                return 47;

            case '\u006d'://m
                return 48;

            case '\u006e'://n
                return 49;

            case '\u006f'://o
                return 50;

            case '\u0070'://p
                return 51;

            case '\u0071'://q
                return 52;

            case '\u0072'://r
                return 53;

            case '\u0073'://s
                return 54;

            case '\u0074'://t
                return 55;

            case '\u0075'://u
                return 56;

            case '\u0076'://v
                return 57;

            case '\u0077'://w
                return 58;

            case '\u0078'://x
                return 59;

            case '\u0079'://y
                return 60;

            case '\u007a'://z
                return 61;

            case '\u007b'://{
                return 80;

            case '\u007c'://|
                return 82;

            case '\u007d'://}
                return 81;

            case '\u007e'://~
                return 78;

            default:
                return 95;

        }
    }

    public void WriteLine(string line)
    {
        statement += line + "\n";
        lines++;
    }

    public void DeleteLine()
    {
        statement = statement.Substring(statement.IndexOf("\n") + 1);
        timer = 0f;
        lines--;
    }

    public void Draw(SpriteBatch sb)
    {

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||MONOSPACE
        if (monoSpace)
        {
            if (align == Direction.left)
            {
                width = 0;
                height = 0;
                int statementLength = statement.Length;
                for (int i = 0; i < statementLength; i++)
                {
                    if (statement[i].Equals('\n'))
                    {
                        width = 0;
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = 0;
                            height++;
                        }

                        Char c = statement[i];
                        int charIndex = ReturnCharacterIndex(c);

                        //only this part changes in monospace
                        //displayRect = new Rectangle((int)(width * characterHeight + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = (int)(width * characterHeight + (int)origin.X);
                        displayRect.Y = height * characterHeight + (int)origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                        width++;
                    }
                }
            }
            else if (align == Direction.right)
            {
                width = -1;
                height = 0;
                int statementLengthMinOne = statement.Length - 1;
                //for (int i = 0; i < statementLength; i++)
                for (int i = statementLengthMinOne; i >= 0; i--)
                {
                    if (statement[i].Equals('\n'))
                    {
                        width = -1;
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = -1;
                            height++;
                        }


                        Char c = statement[i];
                        int charIndex = ReturnCharacterIndex(c);

                        //only this part changes in monospace
                        //displayRect = new Rectangle((int)(width * characterHeight + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = (int)(width * characterHeight + (int)origin.X);
                        displayRect.Y = height * characterHeight + (int)origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                        width--;

                    }
                }
            }
            else
            {
                maxWidth += 12;
                int maxUsedWidth = 0;
                width = 0;
                height = 0;

                for (int i = 0; i < statement.Length; i++)
                {
                    if (statement[i].Equals('\n'))
                    {
                        width = 0;
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = 0;
                            height++;
                        }
                        width += fontSourceRect[ReturnCharacterIndex(statement[i])].Width;
                        if (maxUsedWidth > width)
                        {
                            maxUsedWidth = width;
                        }
                    }
                }

                width = -maxUsedWidth / 2;
                height = 0;

                for (int i = 0; i < statement.Length; i++)
                {
                    if (statement[i].Equals('\n'))
                    {
                        width = -maxUsedWidth / 2;
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = -maxUsedWidth / 2;
                            height++;
                        }

                        Char c = statement[i];
                        int charIndex = ReturnCharacterIndex(c);

                        //only this part changes in monospace

                        //displayRect = new Rectangle((int)(width + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = (int)(width + (int)origin.X);
                        displayRect.Y = height * characterHeight + (int)origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                        width -= fontSourceRect[charIndex].Width + 1;






                    }
                }
            }
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||NON MONOSPACE
        else
        {
            if (align == Direction.left)
            {
                width = 0;
                height = 0;
                int statementLength = statement.Length;
                foreach (char c in statement)
                {
                    if (c.Equals('\n'))
                    {
                        width = 0;
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = 0;
                            height++;
                        }
                        int charIndex = ReturnCharacterIndex(c);
                        //only this part changes in monospace
                        //displayRect = new Rectangle((int)(width + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = (width + origin.X);
                        displayRect.Y = (height * characterHeight) + origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                        width += fontSourceRect[charIndex].Width + 1;
                    }
                }
            }
            else if (align == Direction.right)
            {
                width = 0;
                int maxUsedHeight = 0;
                foreach (char c in statement)
                {
                    if (c.Equals('\n'))
                    {
                        maxUsedHeight++;
                    }
                }
                height = maxUsedHeight;
                for (int i = statement.Length - 1;  i >= 0; i--)
                {
                    if (statement[i].Equals('\n'))
                    {
                        width = 0;
                        height--;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = 0;
                            height--;
                        }
                        Char c = statement[i];
                        int charIndex = ReturnCharacterIndex(c);
                        width += fontSourceRect[charIndex].Width + 1;
                        //displayRect = new Rectangle((int)(width + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = origin.X - width;
                        displayRect.Y = height * characterHeight + origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                    }
                }
            }
            else
            {
                int maxUsedWidth = 0;
                width = 0;
                height = 0;
                foreach (char c in statement)
                {
                    if (c.Equals('\n'))
                    {
                        width = 0;
                        height++;
                    }
                    else
                    {
                        width += fontSourceRect[ReturnCharacterIndex(c)].Width + 1; if (width >= maxWidth)
                        {
                            width = 0;
                            height++;
                            maxUsedWidth = maxWidth;
                        }
                        if (width > maxUsedWidth)
                        {
                            maxUsedWidth = width - 1;
                        }
                    }
                }
                width = -maxUsedWidth / 2;
                height = 0;
                foreach (char c in statement)
                {
                    if (c.Equals('\n'))
                    {
                        width = -maxUsedWidth / 2;
                        height++;
                    }
                    else
                    {
                        if (width >= maxUsedWidth)
                        {
                            width = -maxUsedWidth / 2;
                            height++;
                        }
                        int charIndex = ReturnCharacterIndex(c);
                        //displayRect = new Rectangle((int)(width + (int)origin.X), height * characterHeight + (int)origin.Y, fontSourceRect[charIndex].Width, characterHeight);
                        displayRect.X = width + origin.X;
                        displayRect.Y = height * characterHeight + origin.Y;
                        displayRect.Width = fontSourceRect[charIndex].Width;
                        //displayRect.Height = characterHeight;
                        sb.Draw(tex, displayRect, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
                        width += fontSourceRect[charIndex].Width + 1;
                    }
                }
            }
        }
    }
}
