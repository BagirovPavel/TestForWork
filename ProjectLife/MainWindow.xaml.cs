using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;

namespace ProjectLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Guid uniqueIdentifier = Guid.NewGuid();
        private int generations = 0;
        private bool newGame = true;
        private bool isPaused = false;
        private int resolution = 4;
        private int density = 2;
        private bool[,] last;
        private bool[,] arr;
        private int rows;
        private int columns;
        private bool isLocked;
        DispatcherTimer timer;
        DispatcherTimer timerForLogs;
        DBConnect con = new DBConnect();
        bool isLogSaved = false;

        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Tick += new EventHandler(timer_tick);
            timerForLogs = new DispatcherTimer();
            timerForLogs.Interval = new TimeSpan(0, 0, 1);
            timerForLogs.Tick += new EventHandler(timerForLogs_tick);

            rows = ((int)(canvas_row.Height.Value) - (int)(canvas_row.Height.Value) % resolution) / resolution;
            columns = ((int)(canvas_column.Width.Value) - (int)(canvas_column.Width.Value) % resolution) / resolution;
            arr = new bool[columns, rows];
            main_window.Title = $"Generation №{generations}";
        }

        private void timerForLogs_tick(object sender, EventArgs e)
        {
            using (SqlDataReader reader = con.GetLogs())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("oper_type");
                dt.Columns.Add("current_date");
                while (reader.Read())
                {
                    dt.Rows.Add((string)reader["oper_type"], (DateTime)reader["current_date"]);
                }
                dt.DefaultView.Sort = "current_date desc";
                dg_logs.ItemsSource = dt.DefaultView;
            }
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if (Globals.isConSuccess && !isLogSaved && generations == 1)
            {
                con.SaveLog(CalcForProjectLife.ArrayToString(arr, columns, rows), "START");
                isLogSaved = true;
            }
            last = (bool[,])arr.Clone();
            arr = NextGeneration(arr);
            main_window.Title = $"Generation №{generations}";
            if (CalcForProjectLife.AnalizeGame(arr, last, columns, rows))
            {
                MessageBox.Show("Игра окончена");
                newGame = true;
                if (Globals.isConSuccess)
                    con.SaveLog(CalcForProjectLife.ArrayToString(arr, columns, rows), "END");
                isLogSaved = false;
                timer.Stop();
            }
        }

        //private int CheckNeighbours(bool[,] arrIn, bool universeIsLocked, int x, int y)
        //{
        //    bool isSelf;
        //    bool hasLife;
        //    int col;
        //    int row;
        //    int count = 0;
        //    if (!universeIsLocked)
        //    {
        //        //int xmin = Math.Max(x - 1, 0),
        //        //    xmax = Math.Min(x + 2, columns),
        //        //    ymin = Math.Max(y - 1, 0),
        //        //    ymax = Math.Min(y + 2, rows);

        //        for (int i = -1; i < 2; i++)
        //        {
        //            for (int j = -1; j < 2; j++)
        //            {
        //                col = x + i;
        //                row = y + j;

        //                isSelf = col == x && row == y;
        //                if (col < 0 || col >= columns || row < 0 || row >= rows)
        //                    hasLife = false;
        //                else
        //                    hasLife = arrIn[col, row];

        //                if (hasLife && !isSelf)
        //                    count++;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = - 1; i < 2; i++)
        //        {
        //            for (int j = - 1; j < 2; j++)
        //            {
        //                col = (x + i + columns) % columns;
        //                row = (y + j + rows) % rows;

        //                isSelf = col == x && row == y;
        //                hasLife = arrIn[col, row];

        //                if (hasLife && !isSelf)
        //                    count++;
        //            }
        //        }
        //    }
        //    return count;
        //}


        //private bool CheckIsAlive(int neighbours, bool isAlive)
        //{
        //    bool result;

        //    if (!isAlive && neighbours == 3)
        //        result = true;
        //    else if (isAlive && (neighbours < 2 || neighbours > 3))
        //        result = false;
        //    else
        //        result = isAlive;

        //    return result;
        //}


        private void CanvasRectangleShow(int x, int y, bool isAlive)
        {
            Rectangle rect = CalcForProjectLife.GetRectangle(isAlive, resolution);
            Canvas.SetLeft(rect, x * resolution);
            Canvas.SetTop(rect, y * resolution);
            canvas_field.Children.Add(rect);
        }


        private bool[,] NextGeneration(bool[,] arrIn)
        {
            generations++;
            canvas_field.Children.Clear();
            bool[,] arrOut = new bool[columns, rows];

            if (newGame)
            {
                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        int r = rnd.Next(0, density + 1);
                        bool isAlive = r == density;
                        arrOut[x, y] = isAlive;
                        //Rectangle rect = GetRectangle(isAlive);
                        //Rectangle rect = CalcForProjectLife.GetRectangle(isAlive, resolution);
                        //Canvas.SetLeft(rect, x * resolution);
                        //Canvas.SetTop(rect, y * resolution);
                        //canvas_field.Children.Add(rect);
                        CanvasRectangleShow(x, y, isAlive);
                    }
                }
                newGame = false;
            }
            else
            {
                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        //arrOut[x, y] = CalcForProjectLife.CheckIsAlive(CheckNeighbours(arrIn, isLocked, x, y), arrIn[x, y]);
                        arrOut[x, y] = CalcForProjectLife.CheckIsAlive(CalcForProjectLife.CheckNeighbours(arrIn, isLocked, x, y, columns, rows), arrIn[x, y]);
                        //Rectangle rect = GetRectangle(arrOut[x, y]);
                        //Rectangle rect = CalcForProjectLife.GetRectangle(arrOut[x, y], resolution);
                        //Canvas.SetLeft(rect, x * resolution);
                        //Canvas.SetTop(rect, y * resolution);
                        //canvas_field.Children.Add(rect);
                        CanvasRectangleShow(x, y, arrOut[x, y]);
                    }
                }
            }

            return arrOut;
        }


        //private Rectangle GetRectangle(bool isAlive)
        //{
        //    Rectangle rect = new Rectangle();
        //    if (isAlive)
        //    {
        //        rect.Height = resolution;
        //        rect.Width = resolution;
        //        rect.Fill = Brushes.Green;
        //        rect.Stroke = Brushes.Green;
        //    }
        //    else
        //    {
        //        rect.Height = resolution;
        //        rect.Width = resolution;
        //        rect.Fill = Brushes.Black;
        //        rect.Stroke = Brushes.Black;
        //    }
        //    return rect;
        //}

        private void start_game_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                return;

            isLocked = cb_isLocked.IsChecked == true;
            timer.Start();
        }

        private void pause_game_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
                return;

            timer.Stop();
        }

        private void end_game_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();

            arr = new bool[columns, rows];
            if (newGame)
            {
                canvas_field.Children.Clear();
                main_window.Title = $"Generation №{0}";
            }
            newGame = true;
            generations = 0;
            isLogSaved = false;
        }

        private void main_grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
                return;

            Point point = e.GetPosition(canvas_field);
            int x = ((int)point.X - (int)point.X % resolution) / resolution;
            int y = ((int)point.Y - (int)point.Y % resolution) / resolution;
            arr[x, y] = false;
            //Rectangle rect = GetRectangle(false);
            //Rectangle rect = CalcForProjectLife.GetRectangle(false, resolution);
            //Canvas.SetLeft(rect, x * resolution);
            //Canvas.SetTop(rect, y * resolution);
            //canvas_field.Children.Add(rect);
            CanvasRectangleShow(x, y, false);
            foreach (bool val in arr)
            {
                if (val)
                    return;
            }
            newGame = true;
        }

        private void main_grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
                return;

            Point point = e.GetPosition(canvas_field);
            int x = ((int)point.X - (int)point.X % resolution) / resolution;
            int y = ((int)point.Y - (int)point.Y % resolution) / resolution;
            arr[x, y] = true;
            //Rectangle rect = GetRectangle(true);
            //Rectangle rect = CalcForProjectLife.GetRectangle(true, resolution);
            //Canvas.SetLeft(rect, x * resolution);
            //Canvas.SetTop(rect, y * resolution);
            //canvas_field.Children.Add(rect);
            CanvasRectangleShow(x, y, true);
            newGame = false;
            if (generations == 0)
                generations = 1;
        }

        private void main_window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!con.CheckConnection())
            {
                Globals.isConSuccess = false;
                MessageBox.Show("Не удаётся подключиться к базе данных");
                return;
            }
            else
            {
                con.Open();
                timerForLogs.Start();
            }
        }

        private void save_game_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled || !Globals.isConSuccess)
                return;
            
            DataTable dt = new DataTable();

            using (SqlDataReader reader = con.CheckSave(CalcForProjectLife.ArrayToString(arr, columns, rows)))
            {
                dt.Columns.Add("code");
                dt.Columns.Add("message");
                while (reader.Read())
                {
                    dt.Rows.Add((int)reader["code"], (string)reader["message"]);
                }
            }
            if ((string)dt.Rows[0]["code"] == "1")
            {
                if (con.SaveGame(CalcForProjectLife.ArrayToString(arr, columns, rows), uniqueIdentifier, generations))
                    MessageBox.Show("Сохранение прошло успешно");
                else
                    MessageBox.Show("Что-то пошло не так");
            }
            else
                MessageBox.Show("Такое сохранение уже существует");
        }

        private void main_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timerForLogs.Stop();
            con.Close();
        }

        private void load_game_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
                return;

            LoadWindow lw = new LoadWindow(con);
            lw.ShowDialog();
            if (Globals.isChosen)
            {
                uniqueIdentifier = Globals.guid;
                arr = CalcForProjectLife.StringToArray(Globals.sequence, columns, rows);
                newGame = false;
                isLogSaved = true;
                generations = Globals.generation;

                canvas_field.Children.Clear();
                for (int x = 0; x < columns; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        //Rectangle rect = GetRectangle(isAlive);
                        //Rectangle rect = CalcForProjectLife.GetRectangle(isAlive, resolution);
                        //Canvas.SetLeft(rect, x * resolution);
                        //Canvas.SetTop(rect, y * resolution);
                        //canvas_field.Children.Add(rect);
                        CanvasRectangleShow(x, y, arr[x, y]);
                    }
                }
            }
        }

        private void close_game_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
