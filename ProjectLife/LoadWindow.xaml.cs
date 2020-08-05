using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace ProjectLife
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {

        DBConnect con;
        bool isLoaded = false;

        public LoadWindow(DBConnect con)
        {
            InitializeComponent();
            this.con = con;
        }

        private DataTable GetSavedGames()
        {
            using (SqlDataReader reader = con.LoadGame())
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("guid");
                dt.Columns.Add("generation");
                dt.Columns.Add("sequence");
                while (reader.Read())
                {
                    dt.Rows.Add((int)reader["id"], (Guid)reader["guid"], (int)reader["generation"], (string)reader["sequence"]);
                }
                return dt;
            }
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            Globals.isChosen = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loading_data.ItemsSource = GetSavedGames().DefaultView;
            if (Globals.isConSuccess)
                isLoaded = true;
        }

        private void load_button_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoaded || loading_data.SelectedIndex < 0)
                return;

            try
            {
                DataRowView drv = (DataRowView)loading_data.SelectedItem;
                Globals.guid = Guid.Parse((string)drv.Row["guid"]);
                Globals.sequence = (string)drv.Row["sequence"];
                Globals.generation = Int32.Parse((string)drv.Row["generation"]);
                Globals.isChosen = true;
                MessageBox.Show("Загрузка произведена успешно");
                Close();
            }
            catch (Exception) { }
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (!isLoaded || loading_data.SelectedIndex < 0 || !Globals.isConSuccess)
                return;
            try
            {
                DataRowView drv = (DataRowView)loading_data.SelectedItem;
                if (con.DeleteGame(Guid.Parse((string)drv.Row["guid"])))
                {
                    loading_data.ItemsSource = GetSavedGames().DefaultView;
                    MessageBox.Show("Запись удалена");
                    return;
                }

                MessageBox.Show("Что-то пошло не так");
            }
            catch (Exception) { }
        }
    }
}
