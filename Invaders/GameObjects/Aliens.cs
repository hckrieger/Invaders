﻿using Engine;
using Invaders.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invaders.GameObjects
{
    class Aliens : GameObject
    {
        Enemy[,] EnemyGrid;
        const int WIDTH = 11;
        const int HEIGHT = 5;
        float startMovementTime, movementTimer;
        float projectileTimer;
        float startDeathDelay, deathDelay;
        List<Enemy[]> alienArrayColumns = new List<Enemy[]>();
        Point windowSize;
        bool movingDown = false;
        float xDirection = 11, yDirection = 0;
        int counter = 1;
        TextGameObject counterText = new TextGameObject("Fonts/Debug", 1, Color.White, TextGameObject.Alignment.Center);
        TextGameObject scoreFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        TextGameObject livesFont = new TextGameObject("Fonts/ScoreFont", 1, Color.White);
        int activeCount, targetNumber;
        int alienYPosition;
        bool alienBreach = false, earnedExtraLife = false;
        public int Lives { get; set; }

        public Aliens(Point windowSize)
        {
            EnemyGrid = new Enemy[WIDTH, HEIGHT];
            this.windowSize = windowSize;
            counterText.LocalPosition = new Vector2(450, 100);
            scoreFont.LocalPosition = new Vector2(50, 10);
            livesFont.LocalPosition = new Vector2(windowSize.X - 100, 10);

                
            Reset();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            scoreFont.Text = $"Score\n  {MainScene.Score}";
            livesFont.Text = $"Lives\n   {Lives}";

            counterText.Text = alienYPosition.ToString();
           
            //if gameplay is running then set the movement timer for aliens
            //makes it where aliens move in it's desired direction after a short time interval
            if (MainScene.CurrentState == MainScene.State.Playing)
                movementTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Move the aliens to determined direction everytime the timer expires and resets
            if (movementTimer <= 0)
            {
                LocalPosition += new Vector2(xDirection, yDirection);
                movementTimer = startMovementTime;

                if (movingDown)
                {
                    counter++;
                    yDirection = 20;

                    if (startMovementTime >= .025f)
                        startMovementTime -= .0415f;
                    movingDown = false;
                }
                else
                {
                    yDirection = 0;
                }
            }

            if (alienArrayColumns.Count > 0)
            {
                if (alienArrayColumns.First()[0].GlobalPosition.X <= 25 && xDirection < 0)
                {
                    xDirection = Math.Abs(xDirection);

                    movingDown = true;
                }
                else if (alienArrayColumns.Last()[0].GlobalPosition.X >= windowSize.X - 25 && xDirection > 0)
                {
                    xDirection = -(xDirection);
                    movingDown = true;
                }
            }


            bool speedUp = false;
            if (activeCount < targetNumber && (activeCount % 5 == 0 && activeCount >= 5 ||
                activeCount < 5))
                speedUp = true;

            if (speedUp && startMovementTime > .025f)
            {
                if (activeCount >= 5)
                {
                    startMovementTime -= .0415f;
                    targetNumber -= 5;
                } else
                {
                    startMovementTime -= .0666f;
                    targetNumber -= 1;
                }
                    
            }

            if (startMovementTime < .025f)
                startMovementTime = .025f;


            var projectile = MainScene.Projectile;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (CollisionDetection.ShapesIntersect(projectile.BoundingBox, EnemyGrid[x, y].BoundingBox))
                    {
                        var index = EnemyGrid[x, y].Index;
                        switch (index % 5)
                        {
                            case 0:
                                MainScene.Score += 30;
                                break;
                            case 1:
                                MainScene.Score += 20;
                                break;
                            case 2:
                                MainScene.Score += 20;
                                break;
                            case 3:
                                MainScene.Score += 10;
                                break;
                            case 4:
                                MainScene.Score += 10;
                                break;

                        }

                        EnemyGrid[x, y].Active = false;
                        projectile.Active = false;
                        activeCount--;
                    }

                    if (EnemyGrid[x, y].BoundingBox.Bottom >= MainScene.Barriers[0].BoundingBox.Bottom)
                    {
                        alienBreach = true;
                        MainScene.CurrentState = MainScene.State.Died;

                        break;
                    }
                }
            }

            if (alienArrayColumns.Count > 0 && MainScene.CurrentState == MainScene.State.Playing)
                projectileTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (projectileTimer <= 0)
            {
                foreach (Projectile obj in MainScene.ProjectilesForAlien)
                {
                    if (!obj.Active && obj != null && BlastingAlien() != null && activeCount > 0)
                    {
                        obj.Active = true;
                        obj.LocalPosition = BlastingAlien().GlobalPosition;
                        break;
                    }
                }


                projectileTimer = (float)ExtendedGame.Random.NextDouble(.5, .8);
            }

            //If all the aliens in the columns are inactive than remove that array from the List
            for (int i = 0; i < alienArrayColumns.Count; i++)
            {
                if (alienArrayColumns[i].All(m => m.Active == false))
                    alienArrayColumns.RemoveAt(i);
            }


            if (MainScene.CurrentState == MainScene.State.Died && Lives > 0)
            {
                deathDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (deathDelay <= 0)
                {
                    if (alienBreach)
                    {
                        Reset();
                        foreach (Barrier obj in MainScene.Barriers)
                            obj.Reset();
                    }
                        
                    MainScene.CurrentState = MainScene.State.Playing;
                    deathDelay = startDeathDelay;
                }
            }
            else if (MainScene.CurrentState == MainScene.State.Died && Lives == 0)
            {
                MainScene.CurrentState = MainScene.State.Lost;
            }
                

            if (activeCount == 0)
                MainScene.CurrentState = MainScene.State.Won;

            
            if (MainScene.Score >= 1500 && !earnedExtraLife)
            {
                Lives++;
                earnedExtraLife = true;
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (inputHelper.KeyPressed(Keys.Space) && 
                (MainScene.CurrentState != MainScene.State.Playing) && 
                ((MainScene.CurrentState != MainScene.State.Died && !alienBreach) || MainScene.CurrentState == MainScene.State.Died && alienBreach))
            {
                Reset();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            for (int x = 0; x < WIDTH; x++)
                for (int y = 0; y < HEIGHT; y++)
                    EnemyGrid[x, y].Draw(gameTime, spriteBatch);

            //counterText.Draw(gameTime, spriteBatch);
            scoreFont.Draw(gameTime, spriteBatch);
            livesFont.Draw(gameTime, spriteBatch);
        }

        //Randomly selected alien that will shoot projectile
        public Enemy BlastingAlien()
        {
            var column = alienArrayColumns[ExtendedGame.Random.Next(alienArrayColumns.Count)];
            
                Enemy selectedEnemy = null;

            //If the bottom-most enemy in the chosen column is not active...
            //then iterate to each enemy upward from that until you reach one that's active
            for (int i = column.Length - 1; i >= 0; i--)
            {
                if (!column[i].Active)
                    continue;

                //Turn the color of the bottom-most active enemy orange
                selectedEnemy = column[i];

                break;
            }


            return selectedEnemy;
        }


        public override void Reset()
        {
            
            if (MainScene != null)
            {
                if (MainScene.CurrentState == MainScene.State.Lost || 
                    MainScene.CurrentState == MainScene.State.Start ||
                    (MainScene.CurrentState == MainScene.State.Died && alienBreach))
                {
                    
                    if (alienBreach)
                    {
                        Lives--;
                        alienBreach = false;
                        goto SkipAhead;
                    }
                    {
                        
                        Lives = 3;
                        alienYPosition = windowSize.Y - 525;
                        MainScene.Score = 0;
                        earnedExtraLife = false;
                    }

                    
                }
                else if (MainScene.CurrentState == MainScene.State.Won)
                {
                    alienYPosition += 25;
                }
            } else
            {
                alienYPosition = windowSize.Y - 525;
                Lives = 3;
            }
            SkipAhead:
            LocalPosition = new Vector2(25, alienYPosition);

            movementTimer = .95f;
            startMovementTime = movementTimer;

            deathDelay = 2.5f;
            startDeathDelay = deathDelay;

            activeCount = WIDTH * HEIGHT;
            targetNumber = activeCount;

            if (alienArrayColumns.Count > 0)
                alienArrayColumns.Clear();

            var index = 0;

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {

                    EnemyGrid[x, y] = new Enemy(index);

                    if (!EnemyGrid[x, y].Active)
                        EnemyGrid[x, y].Active = true;

                    EnemyGrid[x, y].Color = Color.White;

                    EnemyGrid[x, y].Parent = this;
                    EnemyGrid[x, y].LocalPosition = new Vector2(x * 44, y * 46);

                    //HEIGHT = 6
                    //Make an array for each column of aliens 
                    if (index % HEIGHT == 0)
                        alienArrayColumns.Add(new Enemy[HEIGHT]);

                    alienArrayColumns[index / HEIGHT][index % HEIGHT] = EnemyGrid[x, y];

                    index++;
                }
            }

            if (MainScene != null)
                MainScene.CurrentState = MainScene.State.Playing;
        }

        public bool AlienBreach => alienBreach;

        public MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.SCENE_MAIN);
    }
}
