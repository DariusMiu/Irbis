using Irbis;
using System;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

[DataContract]
public class Print
{
    public string Konsole
    {
        get
        { return konsole.ToString(); }
    }
    StringBuilder konsole;
    Texture2D tex;
    private string texname;

    [DataMember]
    Vector2 displayPosition;
    [DataMember]
    public Color fontColor;
    [DataMember]
    int width;
    [DataMember]
    int height;
    [DataMember]
    int maxWidth;
    [DataMember]
    Rectangle[] fontSourceRect;
    [DataMember]
    public string statement;
    [DataMember]
    public float depth;
    [DataMember]
    public Point origin;
    [DataMember]
    public Direction align;
    [DataMember]
    public int characterHeight;
    [DataMember]
    bool monoSpace;
    [DataMember]
    public bool scrollDown;
    [DataMember]
    public int textScale;
    [DataMember]
    public int lines;

    string printStatement;
    int printLines;
    public bool isConsole;

    public Print(int MaxWidth, Font font, Color colorForFont, bool monospace, Point location, Direction alignSide, float drawDepth)
    {
        bool debug = false;
        align = alignSide;
        depth = drawDepth;
        tex = font.tex;
        width = 0;
        height = 0;
        maxWidth = MaxWidth;
        fontColor = colorForFont;
        characterHeight = font.charHeight;
        lines = 0;
        scrollDown = true;
        origin.X = location.X;
        origin.Y = location.Y - (characterHeight / 2);

        textScale = Irbis.Irbis.textScale;

        monoSpace = monospace;

        statement = printStatement = string.Empty;

        if (debug) { Irbis.Irbis.WriteLine("     font: " + font.ToString()); }
        if (debug) { Irbis.Irbis.WriteLine("    align: " + align); }
        if (debug) { Irbis.Irbis.WriteLine("   origin: " + origin); }
        if (debug) { Irbis.Irbis.WriteLine(" maxWidth: " + maxWidth); }
        if (debug) { Irbis.Irbis.WriteLine("fontColor: " + fontColor); }
        if (debug) { Irbis.Irbis.WriteLine("monoSpace: " + monoSpace); }

        fontSourceRect = new Rectangle[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            { fontSourceRect[(i * 10) + j] = new Rectangle((j * font.charHeight) + (font.charHeight - font.charWidth[(i * 10) + j]), i * font.charHeight, font.charWidth[(i * 10) + j], font.charHeight); }
        }
        displayPosition = origin.ToVector2();
    }

    public Print(Font CONSOLE)
    {
        isConsole = true;
        konsole = new StringBuilder();
        align = Direction.Left;
        depth = 1f;
        tex = CONSOLE.tex;
        width = 0;
        height = 0;
        maxWidth = Irbis.Irbis.resolution.X;
        fontColor = Color.White;
        characterHeight = CONSOLE.charHeight;
        lines = 0;
        scrollDown = monoSpace = true;
        origin.X = 1;
        origin.Y = -14;

        textScale = Irbis.Irbis.textScale;

        fontSourceRect = new Rectangle[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            { fontSourceRect[(i * 10) + j] = new Rectangle((j * CONSOLE.charHeight) + (CONSOLE.charHeight - CONSOLE.charWidth[(i * 10) + j]), i * CONSOLE.charHeight, CONSOLE.charWidth[(i * 10) + j], CONSOLE.charHeight); }
        }
    }

    [OnSerializing]
    void OnSerializing(StreamingContext c)
    { texname = tex.Name; }

    [OnSerialized]
    void OnSerialized(StreamingContext c)
    { texname = null; }

    [OnDeserializing]
    void OnDeserializing(StreamingContext c)
    { }

    [OnDeserialized]
    void OnDeserialized(StreamingContext c)
    {
        tex = Irbis.Irbis.LoadTexture(texname);
        texname = null;
    }

    public Point PrintSize(string StringToMeasure)
    {
        int maxUsedWidth = 0;
        int maxUsedHeight = 0;
        int tempWidth = 0;

        if (monoSpace)
        {
            foreach (char c in StringToMeasure)
            {
                if (c.Equals('\n') || c.Equals('\u000D'))
                {
                    if (tempWidth > maxUsedWidth)
                    { maxUsedWidth = tempWidth; }
                    tempWidth = 0;
                    maxUsedHeight++;
                }
                else
                {
                    if (tempWidth >= maxWidth)
                    {
                        if (tempWidth > maxUsedWidth)
                        { maxUsedWidth = tempWidth; }
                        tempWidth = 0;
                        maxUsedHeight++;
                    }

                    tempWidth += (int)(characterHeight * textScale);
                }
            }
        }
        else
        {
            foreach (char c in StringToMeasure)
            {
                if (c.Equals('\n'))
                {
                    if (tempWidth > maxUsedWidth)
                    { maxUsedWidth = tempWidth; }
                    tempWidth = 0;
                    maxUsedHeight++;
                }
                else
                {
                    if (tempWidth >= maxWidth)
                    {
                        tempWidth = 0;
                        maxUsedHeight++;
                    }
                    if (tempWidth > maxUsedWidth)
                    { maxUsedWidth = tempWidth; }
                    int charIndex = ReturnCharacterIndex(c);
                    if (charIndex < 100)
                    { tempWidth += (int)((fontSourceRect[ReturnCharacterIndex(c)].Width + 1) * textScale); }
                    else
                    { tempWidth += (int)(13 * textScale); }
                }
            }
        }

        if (maxUsedHeight <= 0)
        {
            return new Point(maxUsedWidth, (int)(characterHeight * textScale));
        }

        return new Point(maxUsedWidth, (maxUsedHeight + 1) * (int)(characterHeight * textScale));
    }

    public Point PrintSizeNoScale(string StringToMeasure)
    {
        int maxUsedWidth = 0;
        int maxUsedHeight = 0;
        int tempWidth = 0;

        if (monoSpace)
        {
            foreach (char c in StringToMeasure)
            {
                if (c.Equals('\n') || c.Equals('\u000D'))
                {
                    if (width > maxUsedWidth)
                    { maxUsedWidth = width; }
                    width = 0;
                    maxUsedHeight++;
                }
                else
                {
                    if (width >= maxWidth)
                    {
                        if (width > maxUsedWidth)
                        { maxUsedWidth = width; }
                        width = 0;
                        maxUsedHeight++;
                    }

                    width += (int)(characterHeight);
                }
            }
        }
        else
        {
            foreach (char c in StringToMeasure)
            {
                if (c.Equals('\n'))
                {
                    if (tempWidth > maxUsedWidth)
                    { maxUsedWidth = tempWidth; }
                    tempWidth = 0;
                    maxUsedHeight++;
                }
                else
                {
                    if (tempWidth >= maxWidth)
                    {
                        if (tempWidth > maxUsedWidth)
                        { maxUsedWidth = tempWidth; }
                        tempWidth = 0;
                        maxUsedHeight++;
                    }
                    int charIndex = ReturnCharacterIndex(c);
                    if (charIndex < 100)
                    { tempWidth += (int)((fontSourceRect[ReturnCharacterIndex(c)].Width + 1)); }
                    else
                    { tempWidth += 13; }
                }
            }
        }

        if (maxUsedHeight <= 0)
        {
            return new Point(maxUsedWidth, (int)(characterHeight));
        }

        return new Point(maxUsedWidth, (maxUsedHeight + 1) * (int)(characterHeight));
    }

    public void Clear()
    {
        statement = printStatement = string.Empty;
        if (konsole != null)
        {
            konsole.Clear();
            lines = printLines = 0;
        }
    }

    public void Update(string input, bool clear)
    {
        if (clear)
        { statement = input; }
        else
        { statement += input; }
    }

    public void Update(string input)
    {
        statement += input;
    }

    public void Update(Point location)
    {
        origin = location;
    }

    public void Update(int MaxWidth)
    {
        maxWidth = MaxWidth;
    }

    public static int ReturnCharacterIndex(char c)
    {
        switch (c)
        {
            case '\u001b': //ESCAPE
                return 79;

            case '\u0020': //SPACE
                return 99;

            case '\u0009': //TAB
                return 99;

            case '\u0021': //!
                return 62;

            case '\u0022': //"
                return 94;

            case '\u0023': //#
                return 64;

            case '\u0024': //$
                return 65;

            case '\u0025': //%
                return 66;

            case '\u0026': //&
                return 68;

            case '\u0027': //'
                return 90;

            case '\u0028': //(
                return 70;

            case '\u0029': //)
                return 71;

            case '\u002a': //*
                return 69;

            case '\u002b': //+
                return 75;

            case '\u002c': //,
                return 85;

            case '\u002d': //-
                return 72;

            case '\u002e': //.
                return 83;

            case '\u002f': // /
                return 88;

            case '\u0030': //0
                return 00;

            case '\u0031': //1
                return 01;

            case '\u0032': //2
                return 02;

            case '\u0033': //3
                return 03;

            case '\u0034': //4
                return 04;

            case '\u0035': //5
                return 05;

            case '\u0036': //6
                return 06;

            case '\u0037': //7
                return 07;

            case '\u0038': //8
                return 08;

            case '\u0039': //9
                return 09;

            case '\u003a': //:
                return 86;

            case '\u003b': //;
                return 87;

            case '\u003c': //<
                return 77;

            case '\u003d': //=
                return 74;

            case '\u003e': //>
                return 76;

            case '\u003f': //?
                return 63;

            case '\u0040': //@
                return 89;

            case '\u0041': //A
                return 10;

            case '\u0042': //B
                return 11;

            case '\u0043': //C
                return 12;

            case '\u0044': //D
                return 13;

            case '\u0045': //E
                return 14;

            case '\u0046': //F
                return 15;

            case '\u0047': //G
                return 16;

            case '\u0048': //H
                return 17;

            case '\u0049': //I
                return 18;

            case '\u004a': //J
                return 19;

            case '\u004b': //K
                return 20;

            case '\u004c': //L
                return 21;

            case '\u004d': //M
                return 22;

            case '\u004e': //N
                return 23;

            case '\u004f': //O
                return 24;

            case '\u0050': //P
                return 25;

            case '\u0051': //Q
                return 26;

            case '\u0052': //R
                return 27;

            case '\u0053': //S
                return 28;

            case '\u0054': //T
                return 29;

            case '\u0055': //U
                return 30;

            case '\u0056': //V
                return 31;

            case '\u0057': //W
                return 32;

            case '\u0058': //X
                return 33;

            case '\u0059': //Y
                return 34;

            case '\u005a': //Z
                return 35;

            case '\u005b': //[
                return 91;

            case '\u005c': //\
                return 92;

            case '\u005d': //]
                return 93;

            case '\u005e': //^
                return 39;

            case '\u005f': //_
                return 73;

            case '\u0060': //`
                return 84;

            case '\u0061': //a
                return 36;

            case '\u0062': //b
                return 37;

            case '\u0063': //c
                return 38;

            case '\u0064': //d
                return 39;

            case '\u0065': //e
                return 40;

            case '\u0066': //f
                return 41;

            case '\u0067': //g
                return 42;

            case '\u0068': //h
                return 43;

            case '\u0069': //i
                return 44;

            case '\u006a': //j
                return 45;

            case '\u006b': //k
                return 46;

            case '\u006c': //l
                return 47;

            case '\u006d': //m
                return 48;

            case '\u006e': //n
                return 49;

            case '\u006f': //o
                return 50;

            case '\u0070': //p
                return 51;

            case '\u0071': //q
                return 52;

            case '\u0072': //r
                return 53;

            case '\u0073': //s
                return 54;

            case '\u0074': //t
                return 55;

            case '\u0075': //u
                return 56;

            case '\u0076': //v
                return 57;

            case '\u0077': //w
                return 58;

            case '\u0078': //x
                return 59;

            case '\u0079': //y
                return 60;

            case '\u007a': //z
                return 61;

            case '\u007b': //{
                return 80;

            case '\u007c': //|
                return 82;

            case '\u007d': //}
                return 81;

            case '\u007e': //~
                return 78;

            case '\u1d6e': // ᵮ furaffinity logo
                return 102;

            case '\u1d75': // ᵵ twitter logo
                return 101;

            case '\u2117': // ℗ patreon logo
                return 100;

            case '\u2764': // ❤ heart icon
                return 103;

            default:
                return 98;
        }
    }

    public string GetLine(int index)
    {
        int occurrences = 0;
        int lastOccurrence = 0;
        string returnString = Konsole;

        while (occurrences < index && lastOccurrence >= 0)
        {
            returnString = returnString.Substring(returnString.IndexOf("\n") + 1);
            occurrences++;
        }
        if (lastOccurrence >= 0 && occurrences >= 0)
        {
            if (returnString.IndexOf("\n") >= 0)
            {
                returnString = returnString.Substring(0, returnString.IndexOf("\n"));
            }
        }
        else
        {
            returnString = string.Empty;
        }

        return returnString;
    }

    public void Write(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i].Equals('\n') || line[i].Equals('\u000D'))
            {
                lines++;
                printLines++;
            }
        }
        if (isConsole)
        {
            konsole.Append(line);
            printStatement = string.Join(string.Empty, new string[] { printStatement, line });
        }
        else
        { statement += line; }
    }

    public void WriteLine(string line)
    {
        foreach (char c in line)
        {
            if (c.Equals('\n'))
            {
                lines++;
                printLines++;
            }
        }
        konsole.Append(line + "\n");
        printStatement = string.Join(string.Empty, new string[] { printStatement, line, "\n" });
        lines++;
        printLines++;
    }

    public void WriteLine()
    {
        konsole.Append("\n");
        printStatement = string.Join(string.Empty, new string[] { printStatement, "\n" });
        lines++;
        printLines++;
    }

    public void DeleteLine()
    {
        statement = statement.Substring(statement.IndexOf("\n") + 1);
        lines--;
    }

    public void Draw(SpriteBatch sb)
    {
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||MONOSPACE
        if (monoSpace)
        {
            if (align == Direction.Left)
            {
                width = 0;
                if (scrollDown)
                {
                    height = 0;

                    for (int i = 0; i < statement.Length; i++)
                    {
                        if (statement[i].Equals('\n') || statement[i].Equals('\u000D'))
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
                            displayPosition.X = (int)(width + origin.X);
                            displayPosition.Y = height * (int)(characterHeight * textScale) + (int)origin.Y;
                            if (charIndex < 100)
                            {
                                sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                                width += (int)(characterHeight * textScale);
                            }
                            else
                            {
                                sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                                width += (int)(characterHeight * textScale);
                            }
                        }
                    }
                }
                else
                {
                    height = 0;

                    while (printLines > 50)
                    {
                        printStatement = printStatement.Substring(printStatement.IndexOf("\n") + 1);
                        printLines--;
                    }

                    string tempstatement = printStatement;
                    int lastindexofnewline = -1;

                    while (!string.IsNullOrWhiteSpace(tempstatement))
                    {
                        lastindexofnewline = tempstatement.LastIndexOf('\n');

                        for (int i = lastindexofnewline + 1; i < tempstatement.Length; i++)
                        {
                            if (width >= maxWidth)
                            {
                                width = 0;
                                height--;
                            }

                            Char c = tempstatement[i];
                            int charIndex = ReturnCharacterIndex(c);
                            //only this part changes in monospace
                            displayPosition.X = (int)(width + (int)origin.X);
                            displayPosition.Y = height * (int)(characterHeight * textScale) + (int)origin.Y;
                            if (charIndex < 100)
                            {
                                sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                                width += (int)(characterHeight * textScale);
                            }
                            else
                            {
                                sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                                width += (int)(characterHeight * textScale);
                            }
                        }

                        if (lastindexofnewline > 0)
                        {
                            tempstatement = tempstatement.Substring(0, lastindexofnewline);
                            width = 0;
                            height--;
                        }
                        else
                        {
                            tempstatement = string.Empty;
                        }
                    }
                }

            }
            else if (align == Direction.Right)
            {
                width = -(characterHeight * 2 + 1);
                if (scrollDown)
                { height = 0; }
                else
                { height = -lines; }

                for (int i = statement.Length - 1; i >= 0; i--)
                {
                    if (statement[i].Equals('\n') || statement[i].Equals('\u000D'))
                    {
                        width = -(characterHeight * 2 + 1);
                        height++;
                    }
                    else
                    {
                        if (width >= maxWidth)
                        {
                            width = -(characterHeight * 2 + 1);
                            height++;
                        }


                        Char c = statement[i];
                        int charIndex = ReturnCharacterIndex(c);
                        //only this part changes in monospace
                        displayPosition.X = (int)(width + (int)origin.X);
                        displayPosition.Y = height * (int)(characterHeight * textScale) + (int)origin.Y;
                        if (charIndex < 100)
                        {
                            sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                            width -= characterHeight * textScale;
                        }
                        else
                        {
                            sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                            width -= (int)(characterHeight * textScale);
                        }
                    }
                }
            }
            else
            {
                maxWidth += characterHeight;
                int maxUsedWidth = 0;
                width = 0;
                if (scrollDown)
                {
                    height = 0;
                }
                else
                {
                    height = -lines;
                }

                for (int i = 0; i < statement.Length; i++)
                {
                    if (statement[i].Equals('\n') || statement[i].Equals('\u000D'))
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
                        width += characterHeight;
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
                    if (statement[i].Equals('\n') || statement[i].Equals('\u000D'))
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
                        displayPosition.X = (int)(width + (int)origin.X);
                        displayPosition.Y = height * (int)(characterHeight * textScale) + (int)origin.Y;
                        if (charIndex < 100)
                        {
                            sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                            width -= ((characterHeight + 1) * textScale);
                        }
                        else
                        {
                            sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                            width -= (int)((characterHeight + 3) * textScale);
                        }
                    }
                }
            }
        }
        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||NON MONOSPACE
        else
        {
            if (align == Direction.Left)
            {
                width = 0;
                if (scrollDown)
                { height = 0; }
                else
                { height = -lines; }

                for (int i = 0; i < statement.Length; i++)
                {
                    if (statement[i].Equals('\n') || statement[i].Equals('\u000D'))
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
                        int charIndex = ReturnCharacterIndex(statement[i]);
                        displayPosition.X = width + origin.X;
                        displayPosition.Y = height * (int)(characterHeight * textScale) + origin.Y;
                        if (charIndex < 100)
                        {
                            sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                            width += (int)((fontSourceRect[charIndex].Width + 1) * textScale);
                        }
                        else
                        {
                            sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                            width += (int)((characterHeight + 3) * textScale);
                        }
                    }
                }
            }
            else if (align == Direction.Right)
            {
                int MaxUsedWidth = width = 0;
                if (scrollDown)
                { height = 0; }
                else
                { height = -lines; }
                displayPosition.Y = height * (int)(characterHeight * textScale) + origin.Y;
                string templine = string.Empty;
                for (int i = 0; i <= statement.Length; i++)
                {
                    if (i == statement.Length || statement[i].Equals('\n') || statement[i].Equals('\u000D') || width >= maxWidth)
                    {
                        width = 0;
                        for (int j = templine.Length -1; j >= 0; j--)
                        {
                            char c = templine[j];
                            int charIndex = ReturnCharacterIndex(c);
                            if (charIndex < 100)
                            {
                                width += (int)((fontSourceRect[charIndex].Width + 1) * textScale);
                                displayPosition.X = origin.X - width;
                                sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                            }
                            else
                            {
                                width += (int)((characterHeight + 3) * textScale);
                                displayPosition.X = origin.X - width;
                                sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                            }
                        }
                        height++;
                        displayPosition.Y = height * (int)(characterHeight * textScale) + origin.Y;
                        templine = string.Empty;
                    }
                    else
                    { templine += statement[i]; }
                }
            }
            else
            {
                int maxUsedWidth = 0;
                width = 0;
                if (scrollDown)
                { height = 0; }
                else
                { height = -lines; }

                foreach (char c in statement)
                {
                    if (c.Equals('\n'))
                    {
                        width = 0;
                        height++;
                    }
                    else
                    {
                        int charIndex = ReturnCharacterIndex(c);
                        if (charIndex < 100)
                        { width += (int)((fontSourceRect[ReturnCharacterIndex(c)].Width + 1) * textScale); }
                        else
                        { width += (int)(13 * textScale); }
                        if (width >= maxWidth)
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
                        displayPosition.X = width + origin.X;
                        displayPosition.Y = height * (int)(characterHeight * textScale) + origin.Y;
                        if (charIndex < 100)
                        {
                            sb.Draw(tex, displayPosition, fontSourceRect[charIndex], fontColor, 0f, Vector2.Zero, textScale, SpriteEffects.None, depth);
                            width += (int)((fontSourceRect[charIndex].Width + 1) * textScale);
                        }
                        else
                        {
                            sb.Draw(Irbis.Irbis.fontLogos[charIndex - 100], displayPosition, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
                            width += (int)((characterHeight + 3) * textScale);
                        }
                    }
                }
            }
        }
    }

    public void Draw(SpriteBatch sb, Point location)
    {
        origin.X = location.X;
        origin.Y = (int)(location.Y - ((characterHeight * textScale) / 2));
        Draw(sb);
    }
}


