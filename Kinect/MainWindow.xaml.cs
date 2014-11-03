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
using System.Timers;
using Microsoft.Kinect.Toolkit;

namespace Kinect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Timer focusTimer;
        List<Button> buttonsList;
        //bad idea
        GameButton toPlay;


        public MainWindow()
        {
            InitializeComponent();
            var games = GameManager.ListGames();
            Console.WriteLine(games.Count);

            SelectedTitle.FontSize = 45;
            SelectedTitle.Text = "Welcome!";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = "Press on a game to find out more info. Grip to scroll.";

            // for timing purposes -- doesn't work... yet
            buttonsList = new List<Button>();

            int row = 0;
            int col = 0;

            foreach (GameInfo g in games) {
               //TODO: add new column definition when moving to new column
                if (row > 3)
                {
                    row = 0;
                    col++;
                }
                
                Button button = new GameButton(g.Title, g.Description, g.Path);
                BitmapImage bitmap = new BitmapImage(new Uri(g.ImagePath, System.UriKind.Relative));

                button.Content = bitmap;

               // button.MouseEnter += new MouseEventHandler(Button_MouseEnter);
				button.Click += new RoutedEventHandler(Button_MouseClick);
           
                //set the buttons position in the grid
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                row++;
                
                //add the button
                this.MainGrid.Children.Add(button);
                buttonsList.Add(button);

            }
            
            // timer with 30 second intervals
            focusTimer = new System.Timers.Timer(2000);

            //event associated with elapsed time
            focusTimer.Elapsed += OnTimedEvent;
            focusTimer.Enabled = true;
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

		private void Button_MouseClick(Object sender, RoutedEventArgs e) {
		/*	GameButton b = (GameButton)sender;
			String path = b.Path;
			GameManager.LaunchGame(path);*/
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = b.Description + "\n";

            toPlay = b;

            Button playButton = new Button();
            playButton.Content = "Play";
            playButton.Background = Brushes.Black;
            playButton.Foreground = Brushes.White;
            playButton.MaxWidth = 200;
            playButton.MaxHeight = 125;
            playButton.Click += new RoutedEventHandler(Play_MouseClick);
            this.PlayGrid.Children.Add(playButton);

            

		}

        private void Play_MouseClick(Object sender, RoutedEventArgs e)
        {
           // GameButton b = (GameButton)sender;
            String path = toPlay.Path;
            GameManager.LaunchGame(path);
        }

        //set focus when time has elapsed
        private void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            int count = 0;
           // Console.WriteLine("Event: ", e.SignalTime);
            //loops through ten times
            while (count != buttonsList.Count * 10)
            {
                // set description to button text
            }
        }



    }
}
