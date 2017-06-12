using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class UIElementDiscreteSlider
{
    Texture2D borderTex;
    Texture2D overlayTex;
    Texture2D fillTex;

    Rectangle[] bounds;
    RectangleBorder[] boundsBorder;
    Rectangle sliderArea;

    Color fillColor;
    Color overlayColor;
    Color borderColor;

    float depth;

    int value;
    int maxValue;

    public UIElementDiscreteSlider(Direction align, Rectangle areaForSlider, Texture2D bordertex, Texture2D overlaytex, Texture2D filltex, Color fillcolor, Color overlaycolor, Color bordercolor, int numberofelements, int negativeSpace, float drawDepth)
	{
        sliderArea = areaForSlider;

        borderTex = bordertex;
        overlayTex = overlaytex;
        fillTex = filltex;

        fillColor = fillcolor;
        overlayColor = overlaycolor;
        borderColor = bordercolor;

        value = maxValue = (numberofelements - 1);

        depth = drawDepth;

        bounds = new Rectangle[numberofelements];
        boundsBorder = new RectangleBorder[numberofelements];

        float remainingWidth = areaForSlider.Width;

        for (int i = 0; i < numberofelements; i++)
        {
            int tempWidth = (int)((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i));
            if ((((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i)) - tempWidth) > 0.5)
            {
                tempWidth++;
            }
            bounds[i] = new Rectangle(areaForSlider.X + (areaForSlider.Width - (int)remainingWidth), areaForSlider.Y, tempWidth, areaForSlider.Height);
            boundsBorder[i] = new RectangleBorder(bounds[i], borderColor, depth + 0.1f);
            remainingWidth -= tempWidth;
            remainingWidth -= negativeSpace;
        }
    }

    public void Update(int updateValue)
    {
        value = updateValue - 1;
    }

    public void Draw(SpriteBatch sb)
    {
        for (int i = bounds.Length - 1; i >= 0; i--)
        {
            if (value >= i) { sb.Draw(overlayTex, bounds[i], sliderArea, overlayColor, 0f, Vector2.Zero, SpriteEffects.None, depth + 0.01f); }
            sb.Draw(fillTex, bounds[i], sliderArea, fillColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
            boundsBorder[i].Draw(sb, borderTex);
        }
    }
}
