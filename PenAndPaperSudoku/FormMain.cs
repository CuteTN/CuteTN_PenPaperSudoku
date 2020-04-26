using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenAndPaperSudoku
{
    public enum FormState
    {
        Pencil,
        Marker,
        HighLight,
    }

    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            CustomInit();
        }

        public void CustomInit()
        {
            this.DoubleBuffered = true;

            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint_Drawing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint_Grid);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint_Cursor);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormMain_Paint_ScanResult);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown_Learn);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown_BoardGenerating);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown_UserDraw);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove_UpdateDrawing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseMove_UpdateHighlightCell);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseUp_UserDraw);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseWheel_UserDraw);

            PencilBitmap = new DirectBitmap(GridWidth, GridHeight);
            MarkerBitmap = new DirectBitmap(GridWidth, GridHeight);
            HighlighterBitmap = new DirectBitmap(GridWidth, GridHeight);
            IsDrawingBitmap = new DirectBitmap(GridWidth, GridHeight);

            for(int x=0; x<9; x++)
                for(int y=0; y<9; y++)
                    GridNums[x,y] = 0;

            foreach(var e in this.Controls)
                if(e is Button)
                { 
                    (e as Button).KeyDown += FormMain_KeyDown;
                    (e as Button).KeyDown += FormMain_KeyDown_Learn;
                    (e as Button).KeyDown += FormMain_KeyDown_BoardGenerating;
                    (e as Button).KeyUp += FormMain_KeyUp;
                }
        }

        // Handle hotkey //////////////////////////////////////////////////////////////////////////////////////////////////// 

        private void HandleHotkeysUp(Keys k)
        {
            switch (k)
            {
                case Keys.Control:
                    CtrlKeyIsDown = false;
                    break;

                default:
                    break;
            }
        }

        private void HandleHotkeysDown(Keys k)
        {
            switch(k)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;

                case Keys.Control:
                    CtrlKeyIsDown = true;
                    break;

                default:
                    break;
            }
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            HandleHotkeysUp(e.KeyCode);
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            HandleHotkeysDown(e.KeyCode); 
        }

        // Grid drawing //////////////////////////////////////////////////////////////////////////////////////////////////// 
        // drawing
        
        private int GridWidth = 800;
        private int GridHeight = 800;
        private Point CornerPos = new Point(370, 30);

        private int CurCellX = -1;
        private int CurCellY = -1;

        private int[,] GridNums = new int[9,9];

        public void DrawGridBorders(Graphics g)
        {         
            Pen p;
            p = new Pen(Color.Black, 5);

            for(int x=0; x<3; x++)
                for(int y=0; y<3; y++)
                {
                    Point cellCornerPos = new Point(CornerPos.X + x*GridWidth/3, CornerPos.Y + y*GridWidth/3);
                    g.DrawRectangle(p, cellCornerPos.X, cellCornerPos.Y, GridWidth/3, GridHeight/3);
                }

            p = new Pen(Color.Black, 1);
            for(int x=0; x<9; x++)
                for(int y=0; y<9; y++)
                {
                    Point cellCornerPos = new Point(CornerPos.X + x * GridWidth / 9, CornerPos.Y + y * GridWidth / 9);
                    g.DrawRectangle(p, cellCornerPos.X, cellCornerPos.Y, GridWidth/9, GridHeight/9);
                }
        }

        public void DrawGridNumbers(Graphics g)
        {
            if(GridNums == null)
                return;

            Brush b = new SolidBrush(Color.DarkMagenta);

            for(int x=0; x<9; x++)
                for(int y=0; y<9; y++)
                    if(GridNums[x,y] != 0)
                    {
                        Point p = new Point(CornerPos.X + x * GridWidth / 9 + 15, CornerPos.Y + y * GridWidth / 9 + 10);
                        g.DrawString(GridNums[x,y].ToString(), new Font("Arial", 50, FontStyle.Bold), b, p);
                    }
        }

        public void DrawCellHighlight(Graphics g)
        {            
            if (CurCellX < 0 || CurCellX > 8)
                return;

            if (CurCellY < 0 || CurCellY > 8)
                return;

            Brush b = new SolidBrush(Color.FromArgb(150, Color.LightPink));
            Point cellCornerPos = new Point(CornerPos.X + CurCellX * GridWidth / 9, CornerPos.Y + CurCellY * GridWidth / 9);
            g.FillRectangle(b, cellCornerPos.X, cellCornerPos.Y, GridWidth / 9, GridHeight / 9);
        }

        private void FormMain_MouseMove_UpdateHighlightCell(object sender, MouseEventArgs e)
        {
            CurCellX = (int)Math.Floor( (MousePosition.X - CornerPos.X) * 9f / GridWidth );
            CurCellY = (int)Math.Floor( (MousePosition.Y - CornerPos.Y) * 9f / GridHeight );

            this.Refresh();
        }

        private void FormMain_Paint_Grid(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // do not highlight cell when user is drawing
            if (! UserIsEditting )
                DrawCellHighlight(g);

            DrawGridBorders(g);
            DrawGridNumbers(g);
        }


        // User drawing //////////////////////////////////////////////////////////////////////////////////////////////////// 

        bool LeftMouseIsDown = false;
        bool RightMouseIsDown = false;
        bool CtrlKeyIsDown = false;
        bool UserIsDrawing = false;
        bool UserIsErasing = false;

        bool UserIsEditting
        {
            get
            {
                return UserIsDrawing || UserIsErasing;
            }
        }
        FormState State = FormState.Marker;

        Point PrevDrawPos = MousePosition;
        
        int HighLighterRadius
        {
            get
            {
                return (int)PenHighlighter.Width / 2;
            }
            set
            {
                PenHighlighter.Width = value * 2;
            }
        }
        int MarkerRadius
        {
            get
            {
                return (int)PenMarker.Width / 2;
            }
            set
            {
                PenMarker.Width = value * 2;
            }
        }
        int EraserRadius
        {
            get
            {
                return (int)PenEraser.Width / 2;
            }
            set
            {
                PenEraser.Width = value * 2;
            }
        }

        DirectBitmap IsDrawingBitmap;
        DirectBitmap PencilBitmap;
        DirectBitmap MarkerBitmap;
        DirectBitmap HighlighterBitmap;

        Pen PenPencil = new Pen(Color.Black, 2);
        Pen PenMarker = new Pen(Color.HotPink, 10);
        Brush BrushMarker = new SolidBrush(Color.HotPink);
        Pen PenHighlighter = new Pen(Color.Yellow, 70);
        Brush BrushHighlighter = new SolidBrush(Color.Yellow);

        Pen PenEraser = new Pen(Color.LightGray, 30);
        Brush BrushEraser = new SolidBrush(Color.LightGray);

        public Color ColorPencil
        {
            get
            {
                return PenPencil.Color;
            }
            set
            {
                PenPencil.Color = value;
            }
        }
        public Color ColorMarker
        {
            get
            {
                return PenMarker.Color;
            }
            set
            {
                PenMarker.Color = value;
                (BrushMarker as SolidBrush).Color = value;
            }
        }
        public Color ColorHighlighter
        {
            get
            {
                return PenHighlighter.Color;
            }
            set
            {
                PenHighlighter.Color = value;
                (BrushHighlighter as SolidBrush).Color = value;
            }
        }

        private void StartDrawing()
        {
            PrevDrawPos = MousePosition;
            UserIsDrawing = true;
        }

        private void StartErasing()
        {
            PrevDrawPos = MousePosition;
            UserIsErasing = true;
        }

        private void UpdateDrawing()
        {
            if(!UserIsDrawing)
                return;

            switch (State)
            {
                case FormState.Pencil:
                    {
                        Point p1 = new Point(PrevDrawPos.X - CornerPos.X, PrevDrawPos.Y - CornerPos.Y);
                        Point p2 = new Point(MousePosition.X - CornerPos.X, MousePosition.Y - CornerPos.Y);

                        using( Graphics g = Graphics.FromImage(IsDrawingBitmap.Bitmap) )
                        {
                            g.DrawLine(PenPencil, p1, p2); 
                        }

                        break;
                    }
                case FormState.Marker:
                    {
                        Point p1 = new Point(PrevDrawPos.X - CornerPos.X, PrevDrawPos.Y - CornerPos.Y);
                        Point p2 = new Point(MousePosition.X - CornerPos.X, MousePosition.Y - CornerPos.Y);

                        using (Graphics g = Graphics.FromImage(IsDrawingBitmap.Bitmap))
                        {
                            g.DrawLine(PenMarker, p1, p2);
                            g.FillEllipse(BrushMarker, p2.X - MarkerRadius, p2.Y - MarkerRadius, 2 * MarkerRadius, 2 * MarkerRadius);
                        }

                        break;
                    }
                case FormState.HighLight:
                    {
                        Point p1 = new Point(PrevDrawPos.X - CornerPos.X, PrevDrawPos.Y - CornerPos.Y);
                        Point p2 = new Point(MousePosition.X - CornerPos.X, MousePosition.Y - CornerPos.Y);

                        using (Graphics g = Graphics.FromImage(IsDrawingBitmap.Bitmap))
                        {
                            g.DrawLine(PenHighlighter, p1, p2);
                            g.FillEllipse(BrushHighlighter, p2.X - HighLighterRadius, p2.Y - HighLighterRadius, 2*HighLighterRadius, 2*HighLighterRadius);
                        }

                        break;
                    }
            }

            PrevDrawPos = MousePosition;
        }

        private void UpdateErasing()
        {
            Point p1 = new Point(PrevDrawPos.X - CornerPos.X, PrevDrawPos.Y - CornerPos.Y);
            Point p2 = new Point(MousePosition.X - CornerPos.X, MousePosition.Y - CornerPos.Y);

            using (Graphics g = Graphics.FromImage(IsDrawingBitmap.Bitmap))
            {
                g.DrawLine(PenEraser, p1, p2);
                g.FillEllipse(BrushEraser, p2.X - EraserRadius, p2.Y - EraserRadius, 2 * EraserRadius, 2 * EraserRadius);
            }

            PrevDrawPos = MousePosition;
        }

        private void EndDrawing()
        {
            switch(State)
            {
                case FormState.Pencil:
                    {
                        for(int x=0; x<GridWidth; x++)
                            for(int y=0; y<GridHeight; y++)
                            { 
                                Color pxl = IsDrawingBitmap.GetPixel(x,y);

                                if(pxl.A != 0)
                                    PencilBitmap.SetPixel(x,y,pxl);
                            }
                        break;
                    }
                case FormState.Marker:
                    {
                        for (int x = 0; x < GridWidth; x++)
                            for (int y = 0; y < GridHeight; y++)
                            {
                                Color pxl = IsDrawingBitmap.GetPixel(x, y);

                                if (pxl.A != 0)
                                    MarkerBitmap.SetPixel(x, y, pxl);
                            }
                        break;
                    }
                case FormState.HighLight:
                    {
                        for (int x = 0; x < GridWidth; x++)
                            for (int y = 0; y < GridHeight; y++)
                            {
                                Color pxl = IsDrawingBitmap.GetPixel(x, y);

                                if (pxl.A != 0)
                                    HighlighterBitmap.SetPixel(x, y, Color.FromArgb(100, pxl));
                            }
                        break;
                    }
            }

            for (int x = 0; x < GridWidth; x++)
                for (int y = 0; y < GridHeight; y++)
                    IsDrawingBitmap.SetPixel(x, y, Color.FromArgb(0));

            UserIsDrawing = false;
        }

        private void EndErasing()
        {
            for (int x = 0; x < GridWidth; x++)
                for (int y = 0; y < GridHeight; y++)
                {
                    Color pxl = IsDrawingBitmap.GetPixel(x, y);

                    if (pxl.A != 0)
                    {
                        switch (State)
                        {
                            case FormState.Pencil:
                                {
                                    PencilBitmap.SetPixel(x, y, Color.Empty);
                                    break;
                                }
                            case FormState.Marker:
                                {
                                    MarkerBitmap.SetPixel(x, y, Color.Empty);
                                    break;
                                }
                            case FormState.HighLight:
                                {
                                    HighlighterBitmap.SetPixel(x, y, Color.Empty);
                                    break;
                                }
                        }
                    }
                }

            for (int x = 0; x < GridWidth; x++)
                for (int y = 0; y < GridHeight; y++)
                    IsDrawingBitmap.SetPixel(x, y, Color.FromArgb(0));

            UserIsErasing = false;
        }

        private void FormMain_MouseDown_UserDraw(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            { 
                LeftMouseIsDown = true;
                StartDrawing();
            }
            if (e.Button == MouseButtons.Right)
            {
                RightMouseIsDown = true;
                StartErasing();
            }

        }

        private void FormMain_MouseUp_UserDraw(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            { 
                LeftMouseIsDown = false;
                EndDrawing();
            }
            if (e.Button == MouseButtons.Right)
            {
                RightMouseIsDown = false;
                EndErasing();
            }
        }

        private void FormMain_MouseWheel_UserDraw(object sender, MouseEventArgs e)
        {
            int del = e.Delta / 120;
            switch(State)
            {
                case FormState.HighLight:
                    {
                        int newR = HighLighterRadius + del;

                        if (newR > 0 && newR <= 200)
                            HighLighterRadius = newR;

                        break;
                    }
                default:
                    {
                        int newR = EraserRadius + del;

                        if (newR > 0 && newR <= 200)
                            EraserRadius = newR;

                        break;
                    }
            }

            this.Refresh();
        }

        private void FormMain_MouseMove_UpdateDrawing(object sender, MouseEventArgs e)
        {
            if(UserIsDrawing)
            { 
                UpdateDrawing();
                this.Refresh();
            }
            if(UserIsErasing)
            {
                UpdateErasing();
                this.Refresh();
            }
        }

        private void FormMain_Paint_Drawing(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(HighlighterBitmap.Bitmap, CornerPos);
            g.DrawImage(MarkerBitmap.Bitmap, CornerPos);
            g.DrawImage(PencilBitmap.Bitmap, CornerPos);
            g.DrawImage(IsDrawingBitmap.Bitmap, CornerPos);
        }

        private void FormMain_Paint_Cursor(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            switch(State)
            {
                case FormState.HighLight:
                    {
                        Brush b = new SolidBrush(Color.FromArgb(100, ColorHighlighter));
                        g.FillEllipse(b, MousePosition.X - HighLighterRadius, MousePosition.Y - HighLighterRadius, 2 * HighLighterRadius, 2 * HighLighterRadius);
                        break;
                    }
                case FormState.Marker:
                    {
                        Brush b = new SolidBrush(Color.FromArgb(150, ColorMarker));
                        g.FillEllipse(b, MousePosition.X - MarkerRadius, MousePosition.Y - MarkerRadius, 2 * MarkerRadius, 2 * MarkerRadius);
                        break;
                    }
            }
        }

        private void BtnPencil_Click(object sender, EventArgs e)
        {
            State = FormState.Pencil;
        }

        private void BtnMarker_Click(object sender, EventArgs e)
        {
            State = FormState.Marker;
        }

        private void BtnHighlight_Click(object sender, EventArgs e)
        {
            State = FormState.HighLight;
        }

        // Scan Model //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private NumberScanModel.NumberScanModel ScanModel = new NumberScanModel.NumberScanModel("NumberData");

        bool EnableLearning = false;

        private DirectBitmap GetCellMarkerBitmap()
        {
            if(CurCellX < 0 || CurCellX > 8)
                return null;

            if(CurCellY < 0 || CurCellY > 8)
                return null;

            int CellWidth = GridWidth / 9;
            int CellHeight = GridHeight / 9;

            DirectBitmap result = Utilities.Crop(MarkerBitmap, CurCellX*CellWidth, CurCellY*CellHeight, CellWidth, CellHeight);

            return result;
        }

        private void FormMain_KeyDown_Learn(object sender, KeyEventArgs e)
        {
            if(! EnableLearning)
                return;

            if(e.KeyCode == Keys.Delete)
            {     
                ScanModel.ClearData();
                if(e.Shift)
                    for(int i=0; i<10; i++)
                    { 
                        ScanModel.LearnFromDir(null, i);
                    }
            }

            int num = -1;

            try
            {
                num = int.Parse(e.KeyCode.ToString().Remove(0,6));
            } 
            catch
            { }

            if(num != -1)
            {   
                DirectBitmap CurCellContent = GetCellMarkerBitmap();
                this.CreateGraphics().DrawImage(CurCellContent.Bitmap, 10, 100);

                ScanModel.Learn(CurCellContent, num);
            }
        }

        // show scan result //////////////////////////////////////////////////////////////////////////////////////////////////// 
        List<KeyValuePair<double, int>> ScanResult;
       
        Point ScanResultPos = new Point(1200, 20);

        int CurScanCellX = -1;
        int CurScanCellY = -1;
            
        private void ShowScanResult(Graphics g)
        {
            DirectBitmap CurCellBmp;

            if(CurScanCellX != CurCellX || CurScanCellY != CurCellY)
            {
                CurScanCellX = CurCellX;
                CurScanCellY = CurCellY;

                CurCellBmp = GetCellMarkerBitmap();
                ScanResult = ScanModel.CalAllMatch(CurCellBmp);
            }

            if(ScanResult == null)
                return;

            for(int i=0; i<8; i++)
                for(int j=i+1; j<9; j++)
                { 
                    if(ScanResult[i].Key > ScanResult[j].Key)
                    {
                        var temp = ScanResult[i];
                        ScanResult[i] = ScanResult[j];
                        ScanResult[j] = temp;
                    }
                }
                        
            int linespacing = 50;
            int cnt = 0;

            Font font = new Font("Arial", 30, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.HotPink);
            Point drawPos;

            foreach (var i in ScanResult)
            {
                brush = new SolidBrush(Color.HotPink);
                drawPos = new Point(ScanResultPos.X, ScanResultPos.Y + cnt*linespacing);
                g.DrawString(i.Value.ToString(), font, brush, drawPos);

                brush = new SolidBrush(Color.LightPink);
                drawPos = new Point(drawPos.X + 50, drawPos.Y + 7);
                int chartWidth = (int)(i.Key / 1000000);
                g.FillRectangle(brush, drawPos.X, drawPos.Y, chartWidth, 30);
                cnt++;
            }
        }

        private void FormMain_Paint_ScanResult(object sender, PaintEventArgs e)
        {
            ShowScanResult(e.Graphics);
        }

        // board generator //////////////////////////////////////////////////////////////////////////////////////////////////// 
        BoardGenerator boardGenerator = new BoardGenerator();

        private void ResetBoard(DifficultyType difficulty)
        {
            MarkerBitmap = new DirectBitmap(GridWidth, GridHeight);
            HighlighterBitmap = new DirectBitmap(GridWidth, GridHeight);
            PencilBitmap = new DirectBitmap(GridWidth, GridHeight);
            IsDrawingBitmap = new DirectBitmap(GridWidth, GridHeight);
            
            var temp = boardGenerator.GetBoard(difficulty);
            if(temp != null)
                GridNums = temp;

            this.Refresh();
        }

        private void FormMain_KeyDown_BoardGenerating(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.N && e.Control)
            {
                FormNewGameInfo frm = new FormNewGameInfo();
                frm.ShowDialog();
                
                if(frm.DialogResult == DialogResult.OK)
                    ResetBoard(frm.DifficultyResult);
            }
        }
    }
}
