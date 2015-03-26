using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MarriageManiac.Core
{
    static class GraphicsHelper
    {
        public  static Texture2D CreatePixel(Color color)
        {
            // Make a 1x1 texture named pixel.  
            Texture2D pixel = new Texture2D(GoofyGame.GRAPHICS.GraphicsDevice, 1, 1);

            // Create a 1D array of color data to fill the pixel texture with.  
            Color[] colorData = { color };

            // Set the texture data with our color information.  
            pixel.SetData<Color>(colorData);

            return pixel;
        }
    }
}