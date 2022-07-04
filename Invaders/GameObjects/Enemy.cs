using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Invaders.GameObjects
{
    class Enemy : SpriteGameObject
    {
        public int Index { get; set; }
        public Enemy(int index) : base("Sprites/Enemy", .5f)
        {
            Index = index;
            SetOriginToCenter();
        }
    }
}
