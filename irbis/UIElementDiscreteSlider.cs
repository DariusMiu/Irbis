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
    Texture2D[] borderTextures;
    Texture2D overlayTexture;
    Texture2D[] fillTextures;

    Rectangle[] bounds;
    Vector2[] borderLocations;
    Vector2[] fillLocations;
    Vector2 primaryLocation;
    Vector2 secondaryLocation;

    Color fillColor;
    Color overlayColor;
    bool overlay;
    Color borderColor;
    bool border;

    float depth;

    int value;
    int maxValue;

    public UIElementDiscreteSlider(Direction Align, Point SliderLocation, Point OverflowLocation, Texture2D[] FillTex, Texture2D[] BorderTex, Texture2D OverlayTex, Color FillColor,
        Color? BorderColor, Color? OverlayColor, int TotalNumberOfElements, int NumberOfElementsAtPrimaryLocation, Point FillSize, Point BorderSize, int NegativeSpace, float DrawDepth)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("UIElementDiscreteSlider.UIElementDiscreteSlider"); }
        primaryLocation = SliderLocation.ToVector2();
        secondaryLocation = OverflowLocation.ToVector2();

        fillTextures = new Texture2D[TotalNumberOfElements];
        borderTextures = new Texture2D[TotalNumberOfElements];
        overlayTexture = OverlayTex;

        fillColor = FillColor;
        if (OverlayColor != null)
        {
            overlayColor = (Color)OverlayColor;
            overlay = true;
        }
        else
        {
            overlay = false;
        }
        if (BorderColor != null)
        {
            borderColor = (Color)BorderColor;
            border = true;
        }
        else
        {
            border = false;
        }

        value = maxValue = (TotalNumberOfElements - 1);

        depth = DrawDepth;

        bounds = new Rectangle[TotalNumberOfElements];
        borderLocations = new Vector2[TotalNumberOfElements];
        fillLocations = new Vector2[TotalNumberOfElements];

        Irbis.Irbis.WriteLine("BorderSize:" + BorderSize + " FillSize:" + FillSize);
        Irbis.Irbis.WriteLine("(BorderSize.X - FillSize.X) / 2:" + ((BorderSize.X - FillSize.X) / 2));

        for (int i = 0; i < NumberOfElementsAtPrimaryLocation; i++)
        {
               fillTextures[i] = FillTex[i % FillTex.Length];
             borderTextures[i] = BorderTex[i % FillTex.Length];
            borderLocations[i] = (SliderLocation + new Point((int)((BorderSize.X + NegativeSpace) * i), 0)).ToVector2();
              fillLocations[i] = (SliderLocation + new Point((int)((BorderSize.X + NegativeSpace) * i), 0) + ((BorderSize - FillSize).ToVector2() / 2).ToPoint()).ToVector2();
        }

        for (int i = NumberOfElementsAtPrimaryLocation; i < TotalNumberOfElements; i++)
        {
               fillTextures[i] = FillTex[i % FillTex.Length];
             borderTextures[i] = BorderTex[i % FillTex.Length];
            borderLocations[i] = (OverflowLocation + new Point((int)((BorderSize.X + NegativeSpace) * (i - NumberOfElementsAtPrimaryLocation)), 0)).ToVector2();
              fillLocations[i] = (OverflowLocation + new Point((int)((BorderSize.X + NegativeSpace) * (i - NumberOfElementsAtPrimaryLocation)), 0) + ((BorderSize - FillSize).ToVector2() / 2).ToPoint()).ToVector2();
        }

        //float remainingWidth = areaForSlider.Width;

        //for (int i = 0; i < numberofelements; i++)
        //{
        //    int tempWidth = (int)((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i));
        //    if ((((remainingWidth - (negativeSpace * ((numberofelements - 1) - i))) / (numberofelements - i)) - tempWidth) > 0.5)
        //    {
        //        tempWidth++;
        //    }
        //    bounds[i] = new Rectangle(areaForSlider.X + (areaForSlider.Width - (int)remainingWidth), areaForSlider.Y, tempWidth, areaForSlider.Height);
        //    locations[i] = bounds[i].Location.ToVector2();
        //    remainingWidth -= tempWidth;
        //    remainingWidth -= NegativeSpace;
        //}
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
            if (value >= i) { sb.Draw(fillTextures[i], fillLocations[i], null, fillColor, 0f, Vector2.Zero, 1, SpriteEffects.None, depth + 0.01f); }
            sb.Draw(borderTextures[i], borderLocations[i], null, borderColor, 0f, Vector2.Zero, 1, SpriteEffects.None, depth);
        }
    }
}
