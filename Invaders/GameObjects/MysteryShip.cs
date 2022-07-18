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
        bool runTimer = false;
        Point windowSize;
        float appearanceDelay;
        TextGameObject debug = new TextGameObject("Fonts/debug", 1, Color.White);
        enum State
        {
            Pause,
            GoingLeft,
            GoingRight
        }

        State currentState;
        State previousState;

        int expectedShots;

        public MysteryShip(Point windowSize) : base("Sprites/mysteryShip", 1f)
        {
            
            Reset();
            this.windowSize = windowSize;
            SetOriginToCenter();
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //debug.Text = inBounds.ToString();
;

            if (MainScene.Player.ShotsFired % 15 == 0 && MainScene.Player.ShotsFired != 0 && !runTimer && currentState == State.Pause)
            {
                
                appearanceDelay = (float)ExtendedGame.Random.NextDouble(1, 8);
                runTimer = true;
            }

            if (runTimer)
            {

                appearanceDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;


                if (appearanceDelay <= 0 && currentState == State.Pause)
                {
                    
                    if (previousState == State.GoingLeft)
                    {
                        //ExtendedGame.BackgroundColor = Color.White;
                        previousState = currentState;
                        currentState = State.GoingRight;
                        
                    }
                    else if (previousState == State.GoingRight || previousState == State.Pause)
                    {
                        previousState = currentState;
                        currentState = State.GoingLeft;
                        
                    }
                    runTimer = false;
                }
            }

            if (currentState != State.Pause)
            {

                if (currentState == State.GoingLeft)
                {
                    xVelocity = -150;
                }
                else if (currentState == State.GoingRight)
                {
                    xVelocity = Math.Abs(150);
                } 

                if ((currentState == State.GoingLeft && GlobalPosition.X < -Width/2) ||
                    (currentState == State.GoingRight && (GlobalPosition.X > windowSize.X + Width/2)))
                {
                    xVelocity = 0;
                    previousState = currentState;
                    currentState = State.Pause;
                }

                
            } 

            velocity = new Vector2(xVelocity , 0);
        }

        public override void Reset()
        {
            base.Reset();
            expectedShots = 22;
            currentState = State.Pause;
            previousState = currentState;
            LocalPosition = new Vector2(windowSize.X + (Width/2), 40);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            debug.Draw(gameTime, spriteBatch);
        }

        

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
