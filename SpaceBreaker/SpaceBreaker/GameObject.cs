using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceBreaker
{
    abstract class GameObject
    {
        private GameWorld gameWorld;
        protected string[] spriteNames;
        protected Texture2D sprite;
        protected Texture2D[] sprites;
        protected Vector2 position;
        protected float size;
        private bool collision;
        protected Vector2 origin;
        protected float fps;
        private float timeElapsed;
        private int currentIndex;


        public bool Collision { get { return collision; } }

        public GameObject(string spriteNames, Vector2 position, float size, bool collision = true, float fps = 12)
        {
            this.spriteNames = new string[1] { spriteNames };
            sprites = new Texture2D[spriteNames.Length];
            this.position = position;
            this.size = size;
            this.collision = collision;
            this.fps = fps;

            gameWorld = GameWorld.Instance;
        }
        public GameObject(string[] spriteNames, Vector2 position, float size, bool collision = true, float fps = 12)
        {
            this.spriteNames = spriteNames;
            sprites = new Texture2D[spriteNames.Length];
            this.position = position;
            this.size = size;
            this.collision = collision;
            this.fps = fps;

            gameWorld = GameWorld.Instance;
        }

        public virtual void Awake()
        {
            for (int i = 0; i < spriteNames.Length; i++)
            {
                sprites[i] = gameWorld.Content.Load<Texture2D>(spriteNames[i]);
            }
            sprite = sprites[0];

            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, size, SpriteEffects.None, 0);
        }

        public virtual void OnDestroy()
        {

        }

        public void Instantiate(GameObject gameObject)
        {
            gameWorld.Instantiate(gameObject);
        }
        public void Destroy(GameObject gameObject)
        {
            gameWorld.Destroy(gameObject);
        }

        public virtual void CheckColliosion(GameObject obj)
        {
            if (obj == this) { return; }

            float distance = Vector2.Distance(position, obj.position);

            float collisionDistance = ((sprite.Width * size) / 2 < (sprite.Height * size) / 2) ? (sprite.Width * size) / 2 : (sprite.Height * size) / 2;
            collisionDistance += ((obj.sprite.Width * obj.size) / 2 < (obj.sprite.Height * obj.size) / 2) ? (obj.sprite.Width * obj.size) / 2 : (obj.sprite.Height * obj.size) / 2;

            if (distance < collisionDistance)
            {
                Collide(obj);
                obj.Collide(this);
            }
        }

        public abstract void Collide(GameObject gameObject);

        protected void Animate()
        {
            timeElapsed += Time.DeltaTime;

            currentIndex = (int)(timeElapsed * fps);

            if (currentIndex > sprites.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }

            sprite = sprites[currentIndex];
        }
    }
}
