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

        List<Projectile> projectilesForAlien = new List<Projectile>();
        Aliens aliens;

        CountDownTimer countDownTimer;
        TextGameObject livesFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        TextGameObject scoreFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        public int Lives { get; set; }
        public bool EarnedExtraLife { get; set; }
        public int Score { get; set; } = 0;
        Point windowSize;
        float startTransitionDelay, transitionDelay = 2f;

        public MainScene(Point windowSize)
        {
            this.windowSize = windowSize;

            countDownTimer = new CountDownTimer(windowSize);
            gameObjects.AddChild(countDownTimer);

            player = new Player(windowSize);
            gameObjects.AddChild(player);

            

            for (int i = 0; i < 5; i++)
            {
                projectilesForAlien.Add(new Projectile(windowSize, 390, .033f));
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

            livesFont.LocalPosition = new Vector2(windowSize.X - 100, 10);
            gameObjects.AddChild(livesFont);

            startTransitionDelay = transitionDelay;

            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            livesFont.Text = $"Lives\n   {Lives}";
            scoreFont.Text = $"Score\n  {Score}";

            //If the score goves over 1500 then get an extra life
            if (Score >= 1500 && !EarnedExtraLife)
            {
                Lives++;
                EarnedExtraLife = true;
            }

            TransitionDelay(gameTime);

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Space) &&
               (CurrentState == State.Lost))
            {
                Reset();
            }

        }


        public override void Reset()
        {
            base.Reset();
            
            aliens.Reset();

            if (CurrentState == State.Lost || CurrentState == State.Start)
            {
                Lives = 3;
                aliens.AlienYPosition = windowSize.Y - 525;
                Score = 0;
                EarnedExtraLife = false;
            }
            else if (CurrentState == State.Won)
            {
                if (aliens.AlienYPosition < windowSize.Y - 525)
                    aliens.AlienYPosition += 33;
            }
            
            aliens.LocalPosition = new Vector2(25, aliens.AlienYPosition);

            if (CurrentState == State.Start)
                CurrentState = State.CountDown;
        }

        public void TransitionDelay(GameTime gameTime)
        {
            if ((CurrentState == State.Died ||
                 CurrentState == State.Won)
                 && Lives > 0)
            {
                transitionDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (transitionDelay <= 0)
                {

                    if (aliens.AlienBreach || CurrentState == State.Won)
                    {
                        Reset();
                        foreach (Barrier obj in Barriers)
                            obj.Reset();


                    }
                    CurrentState = State.CountDown;
                    transitionDelay = startTransitionDelay;
                }
            }
            else if (CurrentState == State.Died && Lives == 0)
            {
                CurrentState = State.Lost;
            }
        }

        public Projectile Projectile => projectile;

        public List<Projectile> ProjectilesForAlien => projectilesForAlien;

        public Aliens Aliens => aliens;

        public Player Player => player;

        public Barrier[] Barriers => barriers;
    }
}
