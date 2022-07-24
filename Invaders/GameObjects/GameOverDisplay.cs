using Engine;
using Engine.UI;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class GameOverDisplay : SpriteGameObject
    {
        Button playAgainButton;
        TextGameObject header, message, buttonFont;
        public GameOverDisplay(Point windowSize) : base("Sprites/GameOverDisplay", .9f)
        {
            SetOriginToCenter();

            LocalPosition = new Vector2(windowSize.X / 2, windowSize.Y / 2);

            header = new TextGameObject("Fonts/Title", 1f, Color.Black, TextGameObject.Alignment.Center);
            header.Parent = this;
            header.Text = "Game Over";
            header.LocalPosition = new Vector2(0, -225);

            playAgainButton = new Button("Sprites/PlayButton", .95f);
            playAgainButton.Parent = this;
            playAgainButton.SetOriginToCenter();
            playAgainButton.LocalPosition = new Vector2(0, 100);

            buttonFont = new TextGameObject("Fonts/Play", 1f, Color.GreenYellow, TextGameObject.Alignment.Center);
            buttonFont.Text = "PLAY";
            buttonFont.Parent = playAgainButton;
            buttonFont.LocalPosition = new Vector2(0, -15);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainScene.CurrentState == MainScene.State.Lost)
            {
                Visible = true;
                header.Visible = true;
                playAgainButton.Visible = true;
                buttonFont.Visible = true;
            }
            else
            {
                Visible = false;
                header.Visible = false;
                playAgainButton.Visible = false;
                buttonFont.Visible = false;
            }


        }

        public override void HandleInput(InputHelper inputHelper)
        {

            if (playAgainButton.BoundingBox.Contains(inputHelper.MousePositionWorld) &&
                inputHelper.MouseLeftButtonPressed() && playAgainButton.Visible)
                MainScene.Reset();
                
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            playAgainButton.Draw(gameTime, spriteBatch);
            header.Draw(gameTime, spriteBatch);
            buttonFont.Draw(gameTime, spriteBatch);
        }

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
