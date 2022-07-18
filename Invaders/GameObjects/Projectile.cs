using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class Projectile : SpriteGameObject
    {
        float speed;
        public Rectangle CustomBox { get; set; }

        float startTimer, timer;
        public bool runTimer;
        Point windowSize;

        public Projectile(Point windowSize, float speed, float timer) : base("Sprites/Projectile", .33f)
        {
            SetOriginToCenter();
            

            this.speed = speed;
            this.timer = timer;
            this.windowSize = windowSize;
            startTimer = timer;
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainScene.CurrentState == MainScene.State.Died || CollisionDetection.ShapesIntersect(CustomBox, MainScene.Player.CustomRectangle))
                Active = false;

            CustomBox = CustomBounds(new Rectangle(-2, 0, 13, 12));

            if (MainScene.CurrentState == MainScene.State.Playing)
                velocity.Y = speed;

            if (runTimer && MainScene.CurrentState == MainScene.State.Playing)
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0 ||
               (GlobalPosition.Y <= 55 || GlobalPosition.Y >= windowSize.Y) ||
               (MainScene.CurrentState == MainScene.State.Lost || MainScene.CurrentState == MainScene.State.Won))
                Reset();

        }


        public override void Reset()
        {
            base.Reset();
            timer = startTimer;
            Active = false;
            runTimer = false;
        }

        public void RunTimer()
        {
            runTimer = true;
        }

        public void StopTimer()
        {
            runTimer = false;
        }

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
