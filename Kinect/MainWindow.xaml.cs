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
        private int callCount;
        private int numLoop;
        private List<GameButton> buttonsList;

        //bad ideas
        GameButton toPlay;
        Button playButton;


        public MainWindow()
        {
            InitializeComponent();
            var games = GameManager.ListGames();
            Console.WriteLine(games.Count);

            SelectedTitle.FontSize = 45;
            SelectedTitle.Text = "Welcome!";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = "Press on a game to find out more info. Grip to scroll.";

            buttonsList = new List<GameButton>();
            numLoop = 1;

            int row = 0;
            int col = 0;

            GameButton main = new GameButton("Main", "Press on a game to find out more info. Grip to scroll.","");
            GameButton about = new GameButton("About", "Original Group: \nField Session 1: \n Field Session 2: \nIndependent Study: David Alexander, Chris Copper, Krista Horn, Jason Santilli", "");

            // add main button
            Grid.SetRow(main, row);
            Grid.SetColumn(main, col);
            main.Margin = new Thickness(10, 10, 10, 10);
            main.Content = main.Title;
            main.Click += new RoutedEventHandler(Special_MouseClick);
            this.MainGrid.Children.Add(main);
            row++;

            // add about button
            Grid.SetRow(about, row);
            Grid.SetColumn(about, col);
            about.Margin = new Thickness(10, 10, 10, 10);
            about.Content = about.Title;
            about.Click += new RoutedEventHandler(Special_MouseClick);
            this.MainGrid.Children.Add(about);
            row++;

            foreach (GameInfo g in games) {
               //TODO: add new column definition when moving to new column
                if (row > 3)
                {
                    row = 0;
                    col++;
                }
                
                GameButton button = new GameButton(g.Title, g.Description, g.Path);
                BitmapImage bitmap = new BitmapImage(new Uri(g.ImagePath, System.UriKind.Relative));
                button.Content = bitmap;

               // button.MouseEnter += new MouseEventHandler(Button_MouseEnter);
				button.Click += new RoutedEventHandler(Button_MouseClick);
                // add padding
                button.Margin = new Thickness(10, 10, 10, 10);
           
                //set the buttons position in the grid
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col);
                row++;
                
                //add the button
                this.MainGrid.Children.Add(button);
                buttonsList.Add(button);
            }

            callCount = buttonsList.Count;
            
            // timer with 2 minute intervals
            focusTimer = new System.Timers.Timer(120000);

            //event associated with elapsed time
            focusTimer.Elapsed += OnTimedEvent;
            focusTimer.Enabled = true;
        }
        //Skeleton count
      /*  private int GetTotalSkeleton(EventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null) return 0;
                skeletonFrameData.CopySkeletonDataTo(allSkeletons);
                return allSkeletons.Count(s => s.TrackingState != SkeletonTrackingState.NotTracked);
            }
        }*/


        //Update the side panel when the user hovers over a new game
        private void Button_MouseEnter(Object sender, EventArgs e)
        {
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = b.Description;
        }

        private void Special_MouseClick(Object sender, RoutedEventArgs e)
        {
            this.PlayGrid.Children.Remove(playButton);
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.TextWrapping = TextWrapping.Wrap;
            SelectedDescription.Text = b.Description + "\n";



        }


		private void Button_MouseClick(Object sender, RoutedEventArgs e) {
		/*	GameButton b = (GameButton)sender;
			String path = b.Path;
			GameManager.LaunchGame(path);*/
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.TextWrapping = TextWrapping.Wrap;
            SelectedDescription.Text = b.Description + "\n";

            toPlay = b;

            playButton = new Button();
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

        //change to "welcome" when time has elapsed
        private void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
          /*  if (callCount == 0)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SelectedTitle.Text = "Welcome!";
                    SelectedDescription.Text = "Press on a game to find out more info. Grip to scroll.";
                    this.PlayGrid.Children.Remove(playButton);
                }));
                callCount = 1;
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    SelectedTitle.Text = "About";
                    SelectedDescription.Text = "Information and/or pictures here.";
                    this.PlayGrid.Children.Remove(playButton);
                }));
                callCount = 0;
            }*/
            
            //have list of buttons loop through buttonsList[buttonsList.Count % callCount];
            this.Dispatcher.Invoke((Action)(() =>
            {
                SelectedTitle.Text = buttonsList[callCount - (buttonsList.Count * numLoop)].Title;
                SelectedDescription.Text = buttonsList[callCount - (buttonsList.Count * numLoop)].Description;
               // this.PlayGrid.Children.Remove(playButton);  -- only remove if welcome or about button (1st and 2nd index?)
            }));
            callCount++;
            if(callCount % 6 == 0)
                numLoop++;
            
        }



    }
}
