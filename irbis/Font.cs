using Microsoft.Xna.Framework.Graphics;

public struct Font
{
    public Texture2D tex;
    public int charHeight;
    public int[] charWidth;

	public Font(Texture2D fontTexture, int characterHeight, int[] characterWidth, bool monospace)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Font.Font"); }
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

    /// <summary>
    /// This Font will always be monospaced
    /// </summary>
    public Font(Texture2D fontTexture, int characterHeight)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Font.Font"); }
        tex = fontTexture;
        charHeight = characterHeight;
        charWidth = new int[100];
        for (int i = 99; i >= 0; i--)
        {
            charWidth[i] = characterHeight;
        }
    }
}
