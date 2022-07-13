using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders
{
    class CountDownTimer : GameObject
    {
        float startCountDownTimer, countDownTimer = 1f;

        TextGameObject countDownFont = new TextGameObject("Fonts/CountDown", 1, Color.White, TextGameObject.Alignment.Center);
        string[] readySetGo;
        int readySetGoIndex = 0;

        public CountDownTimer(Point windowSize)
        {
            startCountDownTimer = countDownTimer;
            countDownFont.LocalPosition = new Vector2(windowSize.X / 2, 300);
            readySetGo = new string[] { "Ready", "Set", "Go" };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainScene.CurrentState == MainScene.State.CountDown)
            {
                if (!countDownFont.Visible)
                {
                    countDownFont.Visible = true;
                }

                countDownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countDownTimer <= 0)
                {
                    readySetGoIndex++;
                    if (readySetGoIndex < 3)
                        countDownTimer = startCountDownTimer;
                    else
                    {
                        readySetGoIndex = 0;
                        countDownTimer = startCountDownTimer;
                        countDownFont.Visible = false;
                        MainScene.CurrentState = MainScene.State.Playing;
                    }
                }

                countDownFont.Text = readySetGo[readySetGoIndex];
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            countDownFont.Draw(gameTime, spriteBatch);
        }

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
