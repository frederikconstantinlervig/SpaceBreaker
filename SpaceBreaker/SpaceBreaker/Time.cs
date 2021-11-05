using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceBreaker
{
    static class Time
    {
        private static float deltaTime;

        public static float DeltaTime { get { return deltaTime; } }

        public static void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
