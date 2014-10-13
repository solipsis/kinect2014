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

namespace Kinect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var games = GameManager.ListGames();
            Console.WriteLine(games.Count);

            int row = 0;
            int col = 0;

            foreach (GameInfo g in games) {
               //TODO: add new column definition when moving to new column
                if (row > 3)
                {
                    row = 0;
                    col++;
                }
                
                Console.WriteLine(g.Description);
                Console.WriteLine(g.Title);
                Button button = new Button();
                BitmapImage bitmap = new BitmapImage(new Uri(g.ImagePath, System.UriKind.Relative));

                button.Content = bitmap;

                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                
                this.MainGrid.Children.Add(button);
                row++;

            }
            Console.WriteLine("potato");
            Console.WriteLine(this.MainGrid.Children.Count);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
        }
    }
}
