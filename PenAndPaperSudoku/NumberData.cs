using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PenAndPaperSudoku.NumberScanModel
{
    public class NumberData
    {
        public int Count;
        public float Eps;

        public int Width, Height;

        public int[,] data;

        public NumberData(StreamReader input)
        {
            Count = int.Parse(input.ReadLine());
            Eps = float.Parse(input.ReadLine());

            Width = int.Parse(input.ReadLine());
            Height = int.Parse(input.ReadLine());

            data = new int[Width, Height];

            for(int y=0; y<Height; y++)
            { 
                string row = input.ReadLine();
                string[] element = row.Split(' ');

                for(int x=0; x<Width; x++)
                    data[x,y] = int.Parse(element[x]);
            }
        }

        public DirectBitmap DataImage()
        {
            DirectBitmap result = new DirectBitmap(this.Width, this.Height);

            for(int x=0; x<this.Width; x++)
                for(int y=0; y<this.Height; y++)
                { 
                    int alpha = data[x,y];
                    if(Count >= 0)
                        alpha /= Count;

                    result.SetPixel(x,y, System.Drawing.Color.FromArgb(alpha, 0,0,0));
                }

            return result;
        }
    }
}
