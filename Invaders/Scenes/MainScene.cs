﻿using Engine;
using Invaders.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.Scenes
{
    class MainScene : GameState
    {

        public enum State
        {
            CountDown,
            Start,
            Playing,
            Died,
            Won,
            Lost
        }

        public State CurrentState { get; set; }

        Player player;
        Barrier[] barriers = new Barrier[4];
        Projectile projectile;

        List<Projectile> projectilesForAlien = new List<Projectile>();
        Aliens aliens;

        CountDownTimer countDownTimer;

        public int Score { get; set; } = 0;

        public MainScene(Point windowSize)
        {
            countDownTimer = new CountDownTimer(windowSize);
            gameObjects.AddChild(countDownTimer);

            player = new Player(windowSize);
            gameObjects.AddChild(player);

            for (int i = 0; i < 5; i++)
            {
                projectilesForAlien.Add(new Projectile(windowSize, 390, .033f));
                projectilesForAlien[i].Active = false;
                gameObjects.AddChild(projectilesForAlien[i]);
            }
            
            //Most of the gameplay logic is in the aliens class
            //a little bit more can be seen in the player and barrier class
            aliens = new Aliens(windowSize);
            gameObjects.AddChild(aliens);

            projectile = new Projectile(windowSize, -660, .01f);
            gameObjects.AddChild(projectile);

            for (int i = 0; i < 4; i++)
            {
                barriers[i] = new Barrier(i);
                barriers[i].LocalPosition = new Vector2(87 + i * 145, windowSize.Y - 120);
                gameObjects.AddChild(barriers[i]);
            }

        }




        public Projectile Projectile => projectile;

        public List<Projectile> ProjectilesForAlien => projectilesForAlien;

        public Aliens Aliens => aliens;

        public Player Player => player;

        public Barrier[] Barriers => barriers;
    }
}
