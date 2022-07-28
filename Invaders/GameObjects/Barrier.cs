using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class Barrier : SpriteGameObject
    {
        Color[] pixelData, startPixelData;
        int index;
        public Barrier(int index) : base($"Sprites/Barrier{index}", .75f)
        {
            SetOriginToCenter();

            this.index = index;

            
            pixelData = new Color[Width * Height];
            
            startPixelData = new Color[Width * Height];

            Texture.GetData(startPixelData);


            Reset();

            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            foreach (Projectile obj in MainScene.ProjectilesForAlien)
            {
                if (CollisionDetection.ShapesIntersect(obj.CustomBox, BoundingBox))
                    CollideWithBarrier(obj);
            }

            Rectangle determineRectangle = (MainScene.Projectile.Speed > 0) ? MainScene.Projectile.BoundingBox : MainScene.Projectile.CustomBox;

            //If projectile collides with texture bounds of barrier sprite...
            if (CollisionDetection.ShapesIntersect(determineRectangle, BoundingBox))
                CollideWithBarrier(MainScene.Projectile);
                
        }


        public void CollideWithBarrier(Projectile projectile)
        {
            Texture.GetData(pixelData);
            //...Then enable collision detection with individual pixels within barrier
            for (int i = 0; i < Width * Height; i++)
            {
                //If projectile collides with individual pixels that are not transparent
                if (CollisionDetection.ShapesIntersect(pixelData[i].BoundingBox(LocalPosition, Origin, i, Width), projectile.CustomBox) && pixelData[i].A != 0)
                {
                    //Then make the pixels transparent (to give the illusion that they disappeared)
                    //and run the timer that sets the duration that the projectile is active
                    pixelData[i] = Color.Transparent;
                    projectile.RunTimer();
                }
            }
            Texture.SetData(pixelData);
        }

        public override void Reset()
        {
            base.Reset();

            ResetBarrierPixels();

        }

        void ResetBarrierPixels()
        {
            startPixelData.CopyTo(pixelData, 0);
            Texture.SetData(pixelData);

            var wrong = false;

            for (int i = 0; i < Width * Height; i++)
            {
                if (pixelData[i] != startPixelData[i])
                    wrong = true;
            }

            if (wrong)
                ResetBarrierPixels();
        }

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
