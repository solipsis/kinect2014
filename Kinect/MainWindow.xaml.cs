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
using Microsoft.Kinect;

using Kinect.ScoreAPI;


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

        KinectSensor kinect;
        MultiSourceFrameReader reader;
       // BodyFrameReader reader;
        IList<Body> bodies;
        Boolean isNull;


        public MainWindow()
        {
            InitializeComponent();

            kinect = KinectSensor.GetDefault();
            kinect.Open();

            var games = GameManager.ListGames();
            Console.WriteLine(games.Count);

            SelectedTitle.FontSize = 45;
            SelectedTitle.Text = "Welcome!";
            SelectedDescription.FontSize = 30;
            SelectedDescription.Text = "Raise both hands to start. Press on a game to find out more info. Grip to scroll.";
            HighScores.Text = "";

            buttonsList = new List<GameButton>();
            numLoop = 1;

            int row = 0;
            int col = 0;

            GameButton about = new GameButton("About", "Original Group: \nField Session 1: \nField Session 2: \nIndependent Study: David Alexander, Chris Copper, Krista Horn, Jason Santilli", "");


            // add about button
            Grid.SetRow(about, row);
            Grid.SetColumn(about, col);
            about.Margin = new Thickness(10, 10, 10, 10);
            about.Content = about.Title;
            about.Click += new RoutedEventHandler(Special_MouseClick);
            this.MainGrid.Children.Add(about);
            row++;
            buttonsList.Add(about);

            foreach (GameInfo g in games) {
               //TODO: add new column definition when moving to new column
                if (row > 3)
                {
                    row = 0;
                    col++;
                }
                
                GameButton button = new GameButton(g.Title, g.Description, g.Path);
               
                Image i = new Image();
                BitmapImage src = new BitmapImage();
                src.BeginInit();
               // src.UriSource = new Uri("GameDir\\" + g.Title + "\\pic.jpg", UriKind.Relative);
                src.UriSource = new Uri(g.ImagePath, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                i.Source = src;
                button.Content = i;
                
               

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

           /*this.bodies = new Body[this.kinect.BodyFrameSource.BodyCount];
           this.reader = this.kinect.BodyFrameSource.OpenReader();*/



           reader = this.kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Body);
           reader.MultiSourceFrameArrived += reader_MultiSourceFrameArrived;



            // timer with 2 minute intervals
            focusTimer = new System.Timers.Timer(120000);

            //event associated with elapsed time
            focusTimer.Elapsed += OnTimedEvent;
            focusTimer.Enabled = true;
        }

        private void reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    bodies = new Body[frame.BodyFrameSource.BodyCount];

                    frame.GetAndRefreshBodyData(bodies);
                    if (!bodies[0].IsTracked) // why is the body never being tracked?! 
                    {
                        isNull = true;
                        return;
                    }
                    isNull = false;
                }
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


        // for about and main buttons. "play is not displayed"
        private void Special_MouseClick(Object sender, RoutedEventArgs e)
        {
            this.PlayGrid.Children.Clear();
            GameButton b = (GameButton)sender;
            SelectedTitle.FontSize = 48;
            SelectedTitle.Text = b.Title + "\n";
            SelectedDescription.FontSize = 30;
            SelectedDescription.TextWrapping = TextWrapping.Wrap;
            SelectedDescription.Text = b.Description + "\n";

            HighScores.Text = "";


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
            playButton.Content = "PLAY";
            playButton.Background = Brushes.Red;
            playButton.Foreground = Brushes.White;
            playButton.MaxWidth = 200;
            playButton.MaxHeight = 125;
            playButton.Click += new RoutedEventHandler(Play_MouseClick);
            this.PlayGrid.Children.Add(playButton);

            //add high scores from database
            String scoreList =  ""; 
            ScoreAPIResponse scores = ScoreAPI.ScoreAPI.RequestScores(b.Title, 5, 0);
            if (scores.ErrCode == 0)
            {
                int scoreCount = 1;
                foreach (Score s in scores.ScoreSet)
                {
                    scoreList += scoreCount + ": " + s.Name + s.Value; //not sure if this will format correctly
                    scoreList += "\n";
                    scoreCount++;
                }
            }
            HighScores.Text = scoreList;
            

		}

        private void Play_MouseClick(Object sender, RoutedEventArgs e)
        {
            String path = toPlay.Path;
            GameManager.LaunchGame(path);
        }

        //change to "welcome" when time has elapsed
        private void OnTimedEvent(Object sender, ElapsedEventArgs e)
        {
            //have list of buttons loop through buttonsList[buttonsList.Count % callCount];
            // need to check if works after somebody leaves then enters again.
      //      if (isNull)
         //   {
                Console.WriteLine("null");
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (isNull)
                    {
                    String scoreList = "";   
                    ScoreAPIResponse scores = ScoreAPI.ScoreAPI.RequestScores(buttonsList[callCount - (buttonsList.Count * numLoop)].Title, 5, 0);
                    if (scores.ErrCode == 0)
                    {
                        int scoreCount = 1;
                        foreach (Score s in scores.ScoreSet)
                        {
                            scoreList += scoreCount + ": " + s.Name + s.Value; //not sure if this will format correctly
                            scoreList += "\n";
                            scoreCount++;
                        }
                    }
                    HighScores.Text = scoreList;
                    SelectedTitle.Text = buttonsList[callCount - (buttonsList.Count * numLoop)].Title;
                    SelectedDescription.Text = buttonsList[callCount - (buttonsList.Count * numLoop)].Description;
                    HighScores.Text = "";
               // }));
                callCount++;
                if (callCount % buttonsList.Count == 0)
                    numLoop++;
                    }
                }));
            
            
        }



    }
}
