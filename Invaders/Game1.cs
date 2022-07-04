using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Invaders
{
    public class Game1 : ExtendedGame
    {
        public const string SCENE_MAIN = "Main Scene";
        public const string SCENE_START = "Starting Screen";
        public Game1()
        {
            IsMouseVisible = true;

            windowSize = new Point(610, 675);
            worldSize = new Point(610, 675);
        }

        protected override void LoadContent()
        {
            base.LoadContent();


            GameStateManager.AddGameState(SCENE_START, new StartScreen(windowSize));
            GameStateManager.AddGameState(SCENE_MAIN, new MainScene(windowSize));
            GameStateManager.SwitchTo(SCENE_START);
            // TODO: use this.Content to load your game content here
        }


    }
}
