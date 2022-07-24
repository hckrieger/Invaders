using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class MysteryShip : SpriteGameObject
    {
        public float xVelocity;
        Point windowSize;
        TextGameObject debug = new TextGameObject("Fonts/debug", 1, Color.White);
        TextGameObject scoreDisplay = new TextGameObject("Fonts/debug", 1, Color.Red);
        float scoreTimer, startScoreTimer;
        int addedScore;

        enum State
        {
            Pause,
            GoingLeft,
            GoingRight
        }

        State currentState;
        State previousState;

        int expectedShots;

        int yPosition = 70;

        public MysteryShip(Point windowSize) : base("Sprites/mysteryShip", .1f)
        {
 
            scoreTimer = 1f;
            startScoreTimer = scoreTimer;

            Reset();
            this.windowSize = windowSize;
            SetOriginToCenter();
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            debug.Text = expectedShots.ToString();
;
            AfterCollision(gameTime);

            if (scoreDisplay.Visible)
            {
                scoreTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (scoreTimer <= 0)
                {
                    scoreDisplay.Visible = false;
                    scoreTimer = startScoreTimer;
                }
            }


            if (MainScene.Player.ShotsFired == expectedShots && currentState == State.Pause)
            {
                expectedShots += ExtendedGame.Random.Next(22, 100);

                if (previousState == State.GoingLeft)
                {
                    //ExtendedGame.BackgroundColor = Color.White;
                    ChangeToState(State.GoingRight);    

                }
                else if (previousState == State.GoingRight || previousState == State.Pause)
                {
                    ChangeToState(State.GoingLeft);

                }
            }


                    
            if (MainScene.Player.ShotsFired > expectedShots)
            {
                expectedShots = MainScene.Player.ShotsFired + 25;
            }



            if (currentState == State.GoingLeft)
                xVelocity = -100;
            else if (currentState == State.GoingRight)
                xVelocity = Math.Abs(100);
            else
                xVelocity = 0;

            if ((currentState == State.GoingLeft && GlobalPosition.X < -Width/2) ||
                (currentState == State.GoingRight && (GlobalPosition.X > windowSize.X + Width/2)))
                ChangeToState(State.Pause);

            if (MainScene.CurrentState != MainScene.State.Playing)
            {
                xVelocity = 0;
                if (MainScene.CurrentState == MainScene.State.CountDown)
                    Reset();
            }
                    

                velocity = new Vector2(xVelocity , 0);
        }

        public override void Reset()
        {
            base.Reset();
            expectedShots = 22;
            currentState = State.Pause;
            previousState = currentState;
            LocalPosition = new Vector2(windowSize.X + (Width/2), yPosition);
            scoreDisplay.Visible = false;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            debug.Draw(gameTime, spriteBatch);
            scoreDisplay.Draw(gameTime, spriteBatch);
        }

        void AfterCollision(GameTime gameTime)
        {
            if (CollisionDetection.ShapesIntersect(MainScene.Projectile.CustomBox, BoundingBox))
            {
                var scoreIndex = ExtendedGame.Random.Next(0, 5);
                switch (scoreIndex)
                {
                    case 0:
                        addedScore = 50;
                        break;
                    case 1:
                        addedScore = 100;
                        break;
                    case 2:
                        addedScore = 150;
                        break;
                    case 3:
                        addedScore = 200;
                        break;
                    case 4:
                        addedScore = 300;
                        break;
                }

                scoreDisplay.Visible = true;
                scoreDisplay.Text = addedScore.ToString();
                scoreDisplay.LocalPosition = GlobalPosition;

                MainScene.Score += addedScore;

                if (currentState == State.GoingLeft)
                    LocalPosition = new Vector2(-Width / 2, yPosition);
                else if (currentState == State.GoingRight)
                    LocalPosition = new Vector2(windowSize.X + Width / 2, yPosition);

                ChangeToState(State.Pause);

                MainScene.Projectile.Active = false;
            }
        }


        void ChangeToState(State state)
        {
            previousState = currentState;
            currentState = state;
        }
        

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
