using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceBreaker
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;

        private List<GameObject> gameObjects;
        private List<GameObject> gameObjectsToInstantiate;
        private List<GameObject> gameObjectsToDestroy;

        private float enemyTimer;

        private Random random;

        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }

                return instance;
            }
        }

        private GameWorld()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            random = new Random();

            gameObjects = new List<GameObject>();
            gameObjectsToInstantiate = new List<GameObject>();
            gameObjectsToDestroy = new List<GameObject>();

            enemyTimer = 5;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Viewport viewport = graphics.GraphicsDevice.Viewport;

            string[] playerSprites = new string[3] { "Player1", "Player2", "Player3" };

            player = new Player(playerSprites, new Vector2(viewport.Width / 2, viewport.Height / 2), 0.25f, 6, "laser");
            Instantiate(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Time.Update(gameTime);


            if (random.Next(0, 100) < 1)
            {
                string tex = (random.Next(0, 2) < 1) ? "SmallMeteor" : "BigMeteor";

                Instantiate(new Meteor(tex, new Vector2(random.Next(0, 1201), 0), (float)random.Next(20, 30) / 100f, random.Next(1, 7), (float)random.Next(-4, 5) / 100f));
            }


            if (enemyTimer < 0)
            {
                Instantiate(new Enemy("GreenEnemy", new Vector2(random.Next(80, 1121), 0), 0.2f));
                enemyTimer = 5;
            }
            else
            {
                enemyTimer -= Time.DeltaTime;
            }


            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);

                if (gameObject.Collision)
                {
                    foreach (GameObject gameObject1 in gameObjects)
                    {
                        gameObject.CheckColliosion(gameObject1);
                    }
                }
            }

            CallInstantiate();
            CallDestroy();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Instantiate(GameObject gameObject)
        {
            gameObjectsToInstantiate.Add(gameObject);
        }

        public void Destroy(GameObject gameObject)
        {
            gameObjectsToDestroy.Add(gameObject);
        }

        private void CallInstantiate()
        {
            if (gameObjectsToInstantiate.Count > 0)
            {
                foreach (GameObject g in gameObjectsToInstantiate)
                {
                    g.Awake();
                    gameObjects.Add(g);
                }
                gameObjectsToInstantiate.Clear();
            }
        }

        private void CallDestroy()
        {
            if (gameObjectsToDestroy.Count > 0)
            {
                foreach (GameObject g in gameObjectsToDestroy)
                {
                    g.OnDestroy();
                    gameObjects.Remove(g);
                }
                gameObjectsToDestroy.Clear();
            }
        }
    }
}
