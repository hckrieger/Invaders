using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.Scenes
{
    class StartScreen : GameState
    {

        Button playButton = new Button("Sprites/PlayButton", .9f);

        public StartScreen(Point windowSize)
        {
            var color = Color.ForestGreen;
            TextGameObject title = new TextGameObject("Fonts/Title", 1, color, TextGameObject.Alignment.Center);
            title.LocalPosition = new Vector2(windowSize.X / 2, 80);
            title.Text = "Invaders";
            gameObjects.AddChild(title);

            TextGameObject instructions = new TextGameObject("Fonts/Instructions", 1, color, TextGameObject.Alignment.Center);
            instructions.LocalPosition = new Vector2(windowSize.X / 2, 285);
            instructions.Text = "                Instructions:\n\n" +
                                "-Arrow Keys - Move left and right\n" +
                                "-X Key - Fire at aliens";
            gameObjects.AddChild(instructions);



            playButton.LocalPosition = new Vector2(windowSize.X / 2, windowSize.Y - 150);
            playButton.SetOriginToCenter();
            gameObjects.AddChild(playButton);

            TextGameObject credit = new TextGameObject("Fonts/Credit", 1, color, TextGameObject.Alignment.Center);
            credit.LocalPosition = new Vector2(windowSize.X / 2, windowSize.Y - 40);
            credit.Text = "Programmed by Hunter Krieger";
            gameObjects.AddChild(credit);

            color = Color.GreenYellow;

            TextGameObject play = new TextGameObject("Fonts/Play", 1, color, TextGameObject.Alignment.Center);
            play.LocalPosition = new Vector2(windowSize.X / 2, windowSize.Y - 165);
            play.Text = "PLAY";
            gameObjects.AddChild(play);

            
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (playButton.Pressed)
            {
                ExtendedGame.GameStateManager.SwitchTo(Game1.SCENE_MAIN);
            }
                
        }

    }
}
