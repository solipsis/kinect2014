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
                
                Button button = new GameButton(g.Title, g.Description);
                BitmapImage bitmap = new BitmapImage(new Uri(g.ImagePath, System.UriKind.Relative));

                button.Content = bitmap;

                button.MouseEnter += new MouseEventHandler(Button_MouseEnter);

                //set the buttons position in the grid
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                row++;
                
                //add the button
                this.MainGrid.Children.Add(button);

            }
        }

        //Update the side panel when the user hovers over a new game
        private void Button_MouseEnter(Object sender, EventArgs e)
        {
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = b.Description;
        }
    }
}
