using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public class Font
{
    public Texture2D tex;
    public int charHeight;
    public int[] charWidth;

	public Font(Texture2D fontTexture, int characterHeight, int[] characterWidth, bool monospace)
	{
        
        if (monospace)
        {
            tex = fontTexture;
            charHeight = characterHeight;
            charWidth = new int[100];

            for (int i = 99; i >= 0; i--)
            {
                charWidth[i] = characterHeight;
            }
        }
        else
        {
            tex = fontTexture;
            charHeight = characterHeight;
            charWidth = characterWidth;
        }

    }
}
