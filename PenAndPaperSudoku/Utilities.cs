using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PenAndPaperSudoku
{
    static public class Utilities
    {
        static public DirectBitmap Crop(DirectBitmap bitmap, int x, int y, int w, int h)
        {
            if(bitmap == null)
                return null;


            DirectBitmap result = new DirectBitmap(w,h);

            for (int xi = x; xi < x+w; xi++)
                for (int yi = y; yi <= y+h; yi++)
                {
                    result.SetPixel(xi - x, yi - y, bitmap.GetPixel(xi, yi));
                }

            return result;
        }

        /// Crop the minimal the rectangle without transparency on the edges
        static public DirectBitmap Trim(DirectBitmap bitmap)
        {
            if(bitmap == null)
                return null;

            DirectBitmap result;

            int u,d,l,r;
            l = bitmap.Width;
            u = bitmap.Height;
            d = r = -1;

            for(int x=0; x<bitmap.Width; x++)
                for(int y=0; y<bitmap.Height; y++)
                    if( bitmap.GetPixel(x,y).A > 0)
                    {
                        u = Math.Min(u, y);
                        d = Math.Max(d, y);
                        l = Math.Min(l, x);
                        r = Math.Max(r, x);
                    }

            if(l>=r)
                return null;

            result = Crop(bitmap, l, u, r-l+1, d-u+1);

            return result;
        }

        /// Make all non-transparent pixels black
        static public DirectBitmap MakeBlack(DirectBitmap bitmap)
        {
            if(bitmap == null)
                return null;

            DirectBitmap result = new DirectBitmap(bitmap.Width, bitmap.Height);

            for(int x=0; x<bitmap.Width; x++)
                for(int y=0; y<bitmap.Height; y++)
                {
                    Color pxl = Color.FromArgb(bitmap.GetPixel(x,y).A, 0, 0, 0);
                    result.SetPixel(x, y, pxl);
                }

            return result;
        }

    }
}
