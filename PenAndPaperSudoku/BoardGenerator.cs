using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace PenAndPaperSudoku
{
    public enum DifficultyType
    {
        Random = 0,
        Easy = 1,
        Medium = 2,
        Hard = 3,
    }

    /// <summary>
    ///  this class call to an API to generate the board
    ///  https://github.com/berto/sugoku
    /// </summary>
    class BoardGenerator
    {
        private string APIBaseUrl = "https://sugoku.herokuapp.com/board";

        public BoardGenerator()
        {

        }

        private string DiffToStr(DifficultyType difficulty)
        {
            switch(difficulty)
            {
                case DifficultyType.Easy:
                    return "easy";
                case DifficultyType.Medium:
                    return "medium";
                case DifficultyType.Hard:
                    return "hard";
                default:
                    return "random";
            }
        }

        public string GetBoardJson(DifficultyType difficulty=0)
        {
            string strResult = null;

            try
            { 
                string url = APIBaseUrl + "?difficulty=" + DiffToStr(difficulty);
                WebRequest reqObj = WebRequest.Create(url);
                reqObj.Method = "GET";

                HttpWebResponse resObj = null;
                resObj = (HttpWebResponse)reqObj.GetResponse();

                using (Stream stream = resObj.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    strResult = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("API calling failed :^(");
            }

            return strResult;
        }

        public int[,] GetBoard(DifficultyType difficulty=0)
        {
            int[,] result = new int[9,9];

            string json = GetBoardJson(difficulty);
            if(json == null)
                return null;

            var jsonObj = new JavaScriptSerializer().Deserialize<Dictionary<string,int[][]>>(json);
            if(jsonObj == null)
                return null;

            for(int x=0; x<9; x++)
                for(int y=0; y<9; y++)
                    result[x,y] = jsonObj["board"][x][y];

            return result;
        }


    }
}
