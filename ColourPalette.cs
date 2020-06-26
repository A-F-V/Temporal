﻿using System.Drawing;

namespace Temporal
{
    internal class ColourPalette
    {
        private static readonly Color[] MFR =
        {
            Color.FromArgb(255, 190, 15),
            Color.FromArgb(213, 254, 119),
            Color.FromArgb(107, 228, 143),
            Color.FromArgb(27, 208, 161),
            Color.FromArgb(0, 201, 167),
            Color.FromArgb(4, 139, 132),
            Color.FromArgb(8, 76, 97)
        };

        public static ColourPalette MarineFields =
            new ColourPalette(MFR, Color.FromArgb(3, 30, 38), Color.Wheat,
                Color.FromArgb(223, 121, 131)); //Color.FromArgb(208,60,27)

        public Color Background, Body, Second;
        private readonly Color[] colourRange;

        public Color this[int index]
        {
            get
            {
                switch (index)
                {
                    case -1:
                        return Background;
                    case -2:
                        return Body;
                    case -3:
                        return Second;
                    default:
                        if (index <= 0)
                            return colourRange[0];
                        if (index > colourRange.Length - 1)
                            return colourRange[colourRange.Length - 1];
                        return colourRange[index];
                }
            }
        }

        public ColourPalette(Color[] colourRange, Color background, Color body, Color second)
        {
            Background = background;
            Body = body;
            Second = second;
            this.colourRange = colourRange;
        }
    }
}