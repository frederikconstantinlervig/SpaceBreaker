using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceBreaker
{
    class Meteor : GameObject
    {
        private float speed;
    private float rotSpeed;
    private float rotation;


    public Meteor(string spriteName, Vector2 position, float size, float speed, float rotSpeed) : base(spriteName, position, size)
    {
        this.speed = speed * 100;
        this.rotSpeed = rotSpeed * 100;
    }

    public override void Update(GameTime gameTime)
    {
        rotation += rotSpeed * Time.DeltaTime;
        position.Y += speed * Time.DeltaTime;

        if (position.Y > 950)
        {
            Destroy(this);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, size, SpriteEffects.None, 0);
    }

    public override void Collide(GameObject gameObject)
    {
        if (gameObject is Player || gameObject is Laser)
        {
            Destroy(this);
        }
    }
}
}
