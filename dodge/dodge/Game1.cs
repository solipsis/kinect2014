using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.Speech.Recognition;
using System.Collections;

using Kinect.ScoreAPI;

namespace dodge
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Boolean debugging = false;

        Rectangle screenRectangle;

        Texture2D colorVideo, depthVideo;
        Texture2D background;
        Texture2D obstacleTexture;

        ArrayList obstacleSpawners = new ArrayList();

        private Player player;
        private ArrayList obstacles = new ArrayList();

        private Vector2 scorePos = new Vector2(20, 20);
        public SpriteFont Font { get; set; }
        public int Score { get; set; }

        bool running = true;

        KinectSensor kinect;

        SpeechRecognitionEngine sre;

        Skeleton[] skeletonData;

        int loopCounter = 0;
        int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            Content.RootDirectory = "Content";

            setupSpawners();
            setupSpeech();

            
            
        }



        public void setupSpeech()
        {
            sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            GrammarBuilder gb = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add(new string[] { "bang", "restart", "quit" });
            gb.Append(choices);

            GrammarBuilder.Add(choices, gb);


            sre.LoadGrammar(new Grammar(gb));
            sre.SpeechRecognized += speechRecognizedEvent;
            sre.SetInputToDefaultAudioDevice();
            sre.BabbleTimeout = TimeSpan.FromSeconds(.001);
            sre.InitialSilenceTimeout = TimeSpan.FromSeconds(.001);
            sre.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(.001);
            sre.EndSilenceTimeout = TimeSpan.FromSeconds(.001);

            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            try
            {
                colorVideo = new Texture2D(graphics.GraphicsDevice, 640, 480);
                depthVideo = new Texture2D(graphics.GraphicsDevice, 640, 480);

                kinect = KinectSensor.KinectSensors[0];

                kinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                // Linear regression to smooth out the reticule movement
                TransformSmoothParameters parameters = new TransformSmoothParameters();
                parameters.Smoothing = 0.4f;
                parameters.Correction = 0.7f;
                parameters.Prediction = 0.2f;
                parameters.JitterRadius = 1.0f;
                parameters.MaxDeviationRadius = 0.5f;
                kinect.SkeletonStream.Enable(parameters);

                skeletonData = new Skeleton[kinect.SkeletonStream.FrameSkeletonArrayLength];

                kinect.SkeletonFrameReady += this.skeletonFrameReady;
                // kinect.AllFramesReady += this.AllFramesReady;

                kinect.Start();

                

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }
            base.Initialize();
        }



        private void skeletonFrameReady(object sender, SkeletonFrameReadyEventArgs SkeletonFrames)
        {
            using (SkeletonFrame skeletonFrame = SkeletonFrames.OpenSkeletonFrame()) // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData); // get the skeletal information in this frame
                }
            }
        }


        private void Restart()
        {
            //setupLaunchers();
            obstacles = new ArrayList();
            setupSpawners();
            running = true;
            score = 0;
            running = true;

        }



        public void speechRecognizedEvent(object sender, SpeechRecognizedEventArgs e)
        {
            
            if (!running)
            {
                if (e.Result.Text.Equals("restart"))
                {
                    Restart();
                }
                if (e.Result.Text.Equals("quit"))
                {
                    base.Exit();
                }
            }
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>("Arial");
            Texture2D tempTexture = Content.Load<Texture2D>("player3");
            player = new Player(tempTexture, screenRectangle);          
            background = Content.Load<Texture2D>("space-background");
            obstacleTexture = Content.Load<Texture2D>("missile");
            

   
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            kinect.Stop();
            kinect.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Console.WriteLine(score);
            // TODO: Add your update logic here
            if (running)
            {
                updateObstacles();
                updateSpawners();
                updatePlayerPosition();
                checkCollisions();
                loopCounter++;
                score++;
                // if (loopCounter > 60) { loopCounter = 0; }
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, screenRectangle, Color.White);
            player.Draw(spriteBatch, Color.White);
            
            drawObstacles(spriteBatch);
            
            spriteBatch.DrawString(Font,
                "Score: " + score.ToString(),
                scorePos,
                Color.White);

            


            if (!running)
            {
                spriteBatch.DrawString(Font,
                    "YOU LOSE: say 'restart'  to try again or 'quit' to exit the game ",
                    new Vector2(500, 300),
                    Color.White);
                //Restart();
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void updateSpawners()
        {
            foreach (ObstacleSpawner spawner in obstacleSpawners)
            {
                spawner.update();
            }
        }

        public void updateObstacles()
        {

            foreach (Obstacle obs in obstacles)
            {
                obs.update();
            }
        }


        public void drawObstacles(SpriteBatch spritebatch)
        {
            foreach (Obstacle obs in obstacles)
            {
                spriteBatch.Draw(obstacleTexture, obs.getPosition(), Color.White);
            }
        }

        public void enemyMissed()
        {
            running = false;
        }


        public void setupSpawners()
        {     
            obstacleSpawners = new ArrayList();
            obstacleSpawners.Add(new ObstacleSpawner(0, screenRectangle, obstacles));
        }

        public void updatePlayerPosition()
        {
            if (skeletonData[0] != null)
            {
                Skeleton player_skeleton = skeletonData[0];
                if (player_skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    player.setPosition(player_skeleton.Joints[JointType.HandRight].Position.X, player_skeleton.Joints[JointType.HandRight].Position.Y); 
                }
            }
        }

        public void checkCollisions()
        {
            Rectangle playerHitbox = player.getHitbox();

            foreach (Obstacle obs in obstacles)
            {
                if (playerHitbox.Intersects(obs.getHitbox(obstacleTexture))) {
                    running = false;
                    ScoreAPI.SubmitScore("game", "dave", score);
                }
            }
        }


    }
}