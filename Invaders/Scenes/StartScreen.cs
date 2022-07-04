using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.Scenes
{
    class StartScreen : GameState
    {
        TextGameObject title = new TextGameObject("Fonts/Title", 1, Color.White, TextGameObject.Alignment.Center);

        public StartScreen(Point windowSize)
        {
            title.LocalPosition = new Vector2(windowSize.X / 2, 125);
            title.Text = "Invaders";
            gameObjects.AddChild(title);
        }


    }
}
