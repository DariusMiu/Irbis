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
    Vector2[] locations;
    Rectangle sliderArea;

    Color fillColor;
    Color overlayColor;
    Color borderColor;

    float depth;

    int value;
    int maxValue;

    public UIElementDiscreteSlider(Direction align, Rectangle areaForSlider, Texture2D bordertex, Texture2D overlaytex, Texture2D filltex, Color fillcolor, Color overlaycolor, Color bordercolor, int numberofelements, int negativeSpace, float drawDepth)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementDiscreteSlider.UIElementDiscreteSlider"); }
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
        locations = new Vector2[numberofelements];

        float remainingWidth = areaForSlider.Width;

        for (int i = 0; i < numberofelements; i++)
        {
            int tempWidth = (int)((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i));
            if ((((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i)) - tempWidth) > 0.5)
            {
                tempWidth++;
            }
            bounds[i] = new Rectangle(areaForSlider.X + (areaForSlider.Width - (int)remainingWidth), areaForSlider.Y, tempWidth, areaForSlider.Height);
            locations[i] = bounds[i].Location.ToVector2();
            remainingWidth -= tempWidth;
            remainingWidth -= negativeSpace;
        }
    }

    public void Update(int updateValue)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementDiscreteSlider.Update"); }
        value = updateValue - 1;
    }

    public void Draw(SpriteBatch sb)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementDiscreteSlider.Draw"); }
        for (int i = bounds.Length - 1; i >= 0; i--)
        {
            if (value >= i) { sb.Draw(overlayTex, locations[i] * Irbis.Irbis.screenScale, bounds[i], overlayColor, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth + 0.01f); }
            sb.Draw(fillTex, locations[i] * Irbis.Irbis.screenScale, bounds[i], fillColor, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
            RectangleBorder.Draw(sb, bounds[i], borderColor, true);
        }
    }
}
