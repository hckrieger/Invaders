using Engine;
using Invaders.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.Scenes
{
    class MainScene : GameState
    {

        public enum State
        {
            Start,
            CountDown,
            Playing,
            Died,
            Won,
            Lost
        }

        public State CurrentState { get; set; }

        Player player;
        Barrier[] barriers = new Barrier[4];
        Projectile projectile;
        MysteryShip ship;
        List<Projectile> projectilesForAlien = new List<Projectile>();
        Aliens aliens;

        GameOverDisplay gameOverDisplay;

        CountDownTimer countDownTimer;
        TextGameObject livesFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        TextGameObject scoreFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        TextGameObject highScoreFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);

        public int Lives { get; set; }
        public bool EarnedExtraLife { get; set; }
        public int Score { get; set; } = 0;
        private int highScore = 0;
        Point windowSize;
        float startTransitionDelay, transitionDelay = 2f;

        public MainScene(Point windowSize)
        {
            this.windowSize = windowSize;

            countDownTimer = new CountDownTimer(windowSize);
            gameObjects.AddChild(countDownTimer);

            player = new Player(windowSize);
            gameObjects.AddChild(player);

            ship = new MysteryShip(windowSize);
            gameObjects.AddChild(ship);

            for (int i = 0; i < 3; i++)
            {
                projectilesForAlien.Add(new Projectile(windowSize, 300, .066f));
                projectilesForAlien[i].Active = false;
                gameObjects.AddChild(projectilesForAlien[i]);
            }
            
            //Most of the gameplay logic is in the aliens class
            //a little bit more can be seen in the player and barrier class
            aliens = new Aliens(windowSize);
            gameObjects.AddChild(aliens);

            projectile = new Projectile(windowSize, -660, .01f);
            gameObjects.AddChild(projectile);

            for (int i = 0; i < 4; i++)
            {
                barriers[i] = new Barrier(i);
                barriers[i].LocalPosition = new Vector2(87 + i * 145, windowSize.Y - 120);
                gameObjects.AddChild(barriers[i]);
            }

            scoreFont.LocalPosition = new Vector2(50, 10);
            gameObjects.AddChild(scoreFont);

            highScoreFont.LocalPosition = new Vector2(windowSize.X / 2 - 50, 10);
            gameObjects.AddChild(highScoreFont);

            livesFont.LocalPosition = new Vector2(windowSize.X - 100, 10);
            gameObjects.AddChild(livesFont);

            gameOverDisplay = new GameOverDisplay(windowSize);
            gameObjects.AddChild(gameOverDisplay);

            startTransitionDelay = transitionDelay;

            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            livesFont.Text = $"Lives\n   {Lives}";
            highScoreFont.Text = $"High Score\n     {highScore}";
            scoreFont.Text = $"Score\n  {Score}";

            //If the score goves over 1500 then get an extra life
            if (Score >= 1500 && !EarnedExtraLife)
            {
                Lives++;
                EarnedExtraLife = true;
            }

            TransitionDelay(gameTime);

        }


        public override void Reset()
        {
            base.Reset();
            
            if (CurrentState == State.Lost || CurrentState == State.Start)
            {
                Lives = 3;
                aliens.AlienYPosition = windowSize.Y - 525;
                Score = 0;
                EarnedExtraLife = false;
                CurrentState = State.CountDown;


                foreach (Barrier obj in Barriers)
                    obj.Reset();
            }
            else if (CurrentState == State.Won)
            {
                if (aliens.AlienYPosition >= windowSize.Y - 375)
                    aliens.AlienYPosition = windowSize.Y - 375;
                else
                    aliens.AlienYPosition += 50;

                foreach (Barrier obj in Barriers)
                    obj.Reset();
            }
            
            aliens.LocalPosition = new Vector2(25, aliens.AlienYPosition);
            player.Reset();
            aliens.Reset();
            ship.Reset();
            
        }

        public void TransitionDelay(GameTime gameTime)
        {
            if ((CurrentState == State.Died ||
                 CurrentState == State.Won))
            {
                transitionDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (transitionDelay <= 0)
                {

                    if (aliens.AlienBreach || CurrentState == State.Won)
                        Reset();


                    if (Lives == 0)
                    {
                        if (Score > highScore)
                        {
                            highScore = Score;
                            gameOverDisplay.GameOverMessage = "You got the high score!\n\nPlay Again?";
                        } else
                        {
                            gameOverDisplay.GameOverMessage = "Good Game.\n\nPlay Again?";
                        }
                            

                        CurrentState = State.Lost;
                    }
                    else
                    {
                        CurrentState = State.CountDown;
                    }

                    transitionDelay = startTransitionDelay;
                }
            }
            
        }



        public Projectile Projectile => projectile;

        public List<Projectile> ProjectilesForAlien => projectilesForAlien;

        public Aliens Aliens => aliens;

        public Player Player => player;

        public Barrier[] Barriers => barriers;
    }
}
