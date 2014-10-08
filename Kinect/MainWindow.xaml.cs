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
            foreach (GameInfo g in games) {
                Button button = new Button();
                BitmapImage bitmap = new BitmapImage(new Uri(g.ImagePath, System.UriKind.Relative));

                button.Content = bitmap;


               this.MainGrid.Children.Add(button);

            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
        }
    }
}
