using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using System.Windows.Forms;

namespace PenAndPaperSudoku.NumberScanModel
{
    public class NumberScanModel
    {
        private String DataDir = "";

        private bool EnableSaveSample = true;

        private int CommonWidth = 100;
        private int CommonHeight = 100;



        public NumberScanModel(string DataDir)
        {
            this.DataDir = DataDir;

            CreateDataDir();
            LoadAllData();
        }

/// create data directories //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private void CreateDataDir()
        {
            for(int i=0; i<=9; i++)
            {
                string path = DataDir + "/" + i.ToString();
                Directory.CreateDirectory(path);

                if(! File.Exists(path + "/" + i.ToString() + ".txt") )
                    CreateEmptyData(i);
            }
        }

        private void CreateEmptyData(int num)
        {
            string path = DataDir + "/" + num.ToString() + "/" + num.ToString() + ".txt";
            StreamWriter file = new StreamWriter(path);

            // Number Of Samples
            file.WriteLine(0);

            // Epsilon
            file.WriteLine(0);

            // size
            file.WriteLine(CommonWidth);
            file.WriteLine(CommonHeight);

            // Samples
            for(int x=0; x<CommonWidth; x++)
            { 
                for(int y=0; y<CommonHeight; y++)
                    file.Write("0 ");
                file.WriteLine();
            }

            file.Close();

            StreamReader fileRead = new StreamReader(path);
            Data[num] = new NumberData(fileRead);

            fileRead.Close();
        }

        private void SaveData(int num)
        {
            string path = DataDir + "/" + num.ToString() + "/" + num.ToString() + ".txt";
            StreamWriter file = new StreamWriter(path);

            // Number Of Samples
            file.WriteLine(Data[num].Count);

            // Epsilon
            file.WriteLine(Data[num].Eps);

            // size
            file.WriteLine(Data[num].Width);
            file.WriteLine(Data[num].Height);

            // Samples
            for (int y = 0; y < Data[num].Height; y++)
            {
                for (int x = 0; x < Data[num].Width; x++)
                    file.Write(Data[num].data[x,y].ToString() + " ");
                file.WriteLine();
            }

            file.Close();
        }

        public void ClearData()
        {
            for(int i=0; i<=9; i++)
                CreateEmptyData(i);
        }

        /// load data //////////////////////////////////////////////////////////////////////////////////////////////////// 

        private NumberData[] Data = new NumberData[10];

        private void LoadData(int num)
        {
            string path = DataDir + "/" + num.ToString() + "/" + num.ToString() + ".txt";
            StreamReader file = new StreamReader(path);

            Data[num] = new NumberData(file);

            file.Close();
        }

        private void LoadAllData()
        {
            for(int i=0; i<=9; i++)
                LoadData(i); 
        }

/// Learn //////////////////////////////////////////////////////////////////////////////////////////////////// 

        private void SaveSample(DirectBitmap sample, int num)
        {
            if(!EnableSaveSample)
                return;

            string dirPath = DataDir + "/" + num.ToString() + "/";
            string fileName = num.ToString() + "_" + Directory.GetFiles(dirPath, "*.png").Length.ToString() + ".png";
            sample.Bitmap.Save(dirPath + fileName, ImageFormat.Png);
        }

        public void Learn(DirectBitmap sample, int num, bool enableSaveSample = true)
        {
            if(sample == null)
                return;

            if(enableSaveSample && this.EnableSaveSample)
                SaveSample(sample, num);
            
            int[,] encodedV = Encode(sample);

            if(encodedV == null)
                return;

            Data[num].Count++;
            for (int x = 0; x < Data[num].Width; x++)
                for (int y = 0; y < Data[num].Height; y++)
                    Data[num].data[x,y] += encodedV[x,y];

            SaveData(num);
            Data[num].DataImage().Bitmap.Save(num.ToString()+"hihi.png");
        }

        public void LearnFromImageFile(string path, int num)
        {
            Bitmap bmp;
            
            // path = Application.StartupPath + path;
            // path = path.Replace('/', '\\');

            try
            {
                bmp = new Bitmap(path);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            DirectBitmap dbmp = new DirectBitmap(bmp);
            Learn(dbmp, num, false);
        }

        public void LearnFromDir(string dir, int num)
        {
            if(dir == null)
                dir = DataDir + "/" + num.ToString() + "/";

            string[] files = Directory.GetFiles(dir, "*.png");

            foreach (string file in files)
            {
                LearnFromImageFile(file, num);
            }
        }

        private int[,] Encode(DirectBitmap raw)
        {
            if(raw == null)
                return null;

            int[,] result = new int[CommonWidth, CommonHeight];
            Bitmap adjustedbitmap = new Bitmap(CommonWidth, CommonHeight);

            try
            { 
                adjustedbitmap = Utilities.MakeBlack(Utilities.Trim(raw))?.Bitmap;
                if(adjustedbitmap == null)
                {
                    return null;
                }

                adjustedbitmap = new Bitmap(adjustedbitmap, CommonWidth, CommonHeight); 
            }
            catch
            {
                return null;
            }

            for(int x=0; x<CommonWidth; x++)
                for(int y=0; y<CommonHeight; y++)
                { 
                    result[x,y] = adjustedbitmap.GetPixel(x,y).A;
                }

            return result;
        }

        /// Matching //////////////////////////////////////////////////////////////////////////////////////////////////// 

        public List<KeyValuePair<double, int>> CalAllMatch(DirectBitmap sample)
        {
            List<KeyValuePair<double, int>> result = new List<KeyValuePair<double, int>>();

            int[,] encodedV = Encode(sample);

            if(encodedV == null)
                return null;

            for(int i=1; i<10; i++)
            {
                var temp = CalMatch(encodedV, i);
                result.Add(temp);
            }

            return result;
        }

        private KeyValuePair<double, int> CalMatch(int[,] encodedV, int num)
        {
            if(encodedV == null || Data[num].Count == 0)
            {     
                return new KeyValuePair<double, int>(-1f, num); 
            }

            double distance = 0;

            for(int x=0; x<CommonWidth; x++)
                for(int y=0; y<CommonHeight; y++)
                {
                    double temp = (float)Data[num].data[x,y] / Data[num].Count;
                    distance += Math.Pow(encodedV[x,y] - temp, 2);
                }

            return new KeyValuePair<double, int>(distance, num);
        }

    }
}
