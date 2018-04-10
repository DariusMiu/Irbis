using Irbis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public struct Font
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

    public Point PrintSize(string StringToMeasure, int maxWidth, bool monoSpace)
    {
        int width = 0;
        int height = 0;
        int maxUsedWidth = 0;
        int maxUsedHeight = 0;

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

                    width += (int)(charHeight * Irbis.Irbis.screenScale);
                }
            }
        }
        else
        {
            foreach (char c in StringToMeasure)
            {
                if (c.Equals('\n'))
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
                    width += (int)(charWidth[Print.ReturnCharacterIndex(c)] * Irbis.Irbis.screenScale) + 1;
                }
            }
        }

        if (maxUsedWidth <= 0 || maxUsedHeight <= 0)
        {
            return new Point(width + (int)(charHeight * Irbis.Irbis.screenScale), (height + 1) * (int)(charHeight * Irbis.Irbis.screenScale));
        }

        return new Point(maxUsedWidth, (maxUsedHeight + 1) * (int)(charHeight * Irbis.Irbis.screenScale));
    }

    /// <summary>
    /// This Font will always be monospaced
    /// </summary>
    public Font(Texture2D fontTexture, int characterHeight)
    {
        tex = fontTexture;
        charHeight = characterHeight;
        charWidth = new int[100];
        for (int i = 99; i >= 0; i--)
        {
            charWidth[i] = characterHeight;
        }
    }
}
