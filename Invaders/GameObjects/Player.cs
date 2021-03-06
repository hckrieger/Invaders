using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class Player : SpriteGameObject
    {
        float speed = 250;
        public Rectangle CustomRectangle { get; set; }
        Point windowSize;
        Vector2 startPosition;
        public int ShotsFired { get; set; }
        public Player(Point windowSize) : base("Sprites/Player", .5f)
        {
            startPosition = new Vector2(windowSize.X / 2, windowSize.Y - 20);
            SetOriginToCenter();
            this.windowSize = windowSize;
            Reset();
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //CustomRectangle = CustomBounds(new Rectangle(0, 8, 48, 16));

            foreach (Projectile obj in MainScene.ProjectilesForAlien)
            {
                if (CollisionDetection.ShapesIntersect(obj.BoundingBox, BoundingBox))
                {
                    MainScene.CurrentState = MainScene.State.Died;
                    MainScene.Lives--;
                    ShotsFired = 0;
                    break;
                }
            }


        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (MainScene.CurrentState == MainScene.State.Playing)
            {
                if (inputHelper.KeyDown(Keys.Left) && GlobalPosition.X >= 10)
                    velocity.X = -speed;
                else if (inputHelper.KeyDown(Keys.Right) && GlobalPosition.X <= windowSize.X - 10)
                    velocity.X = speed;
                else
                    velocity.X = 0;


                if (inputHelper.KeyPressed(Keys.X))
                {
                    var projectile = MainScene.Projectile;
                    if (!projectile.Active)
                    {
                        projectile.StopTimer();
                        projectile.Active = true;
                        projectile.LocalPosition = GlobalPosition + new Vector2(0, -21);
                        ShotsFired++;
                    }
                    
                }
            } else
            {
                velocity.X = 0;
            }
        }

        public override void Reset()
        {
            base.Reset();
            LocalPosition = startPosition;
            ShotsFired = 0;
        }

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
