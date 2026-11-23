using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using snowman.Scripts;
using System;
using System.Collections.Generic;

namespace snowman
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D backdrop;
        public static int scale = 5;
        private Matrix scaleMatrix;

        public static Texture2D pointTexture;
        public static Texture2D snowman;
        private List<Particle> bigSnowParticles = new List<Particle>();
        private const int snowParticleCount = 80;
        public static Random random = new Random();
        public static float timer;
        private static List<Sprite> drawableSprites = new List<Sprite>();

        private Accessory buttons;
        private Accessory eyes;
        private Accessory scarf;
        private Accessory nose;
        private Accessory handL;
        private Accessory handR;

        public static EventHandler OnMouseClickEvent { get; set;  }

        private MouseState previosMouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 96*scale;
            _graphics.PreferredBackBufferHeight = 128*scale;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            pointTexture = new Texture2D(GraphicsDevice, 1, 1);
            pointTexture.SetData<Color>(new Color[] {Color.White});

            for (int i = 0; i < snowParticleCount; i++)
            {
                bigSnowParticles.Add(new Particle(new Vector2(random.Next(0, 97), random.Next(0, 120)),Math.Max( 1.5f * (float)random.NextDouble(),.5f), random.Next(5, 10 + 1), Color.White));
            }

            drawableSprites.Add(new Snowball(new Rectangle(0, 0, 22, 22), 90,0,.1f,default));
            drawableSprites.Add(new Snowball(new Rectangle(22, 0, 16, 16), 75,1,.11f,new Dictionary<string, Vector2>() {
                { "buttons",new Vector2(7,4)},
                { "scarf",new Vector2(3,0)},
                { "handR",new Vector2(-9,1)},
                { "handL",new Vector2(16,1)}
            }));
            drawableSprites.Add(new Snowball(new Rectangle(38, 0, 12, 12), 64,2,.111f,new Dictionary<string, Vector2>()
            {
                                {"eyes",new Vector2(2,3) },
                {"nose",new Vector2(4,5) }
            }));

            (drawableSprites[0] as Snowball).OnDroppingEndedEvent += OnDrop;
            (drawableSprites[1] as Snowball).OnDroppingEndedEvent += OnDrop;
            ((Snowball)drawableSprites[0]).Drop();

            buttons = new Accessory(new Rectangle(0, 22, 2, 10), new Vector2(10, 110),"buttons");
            eyes = new Accessory(new Rectangle(0, 34, 7, 2), new Vector2(17, 110),"eyes");
            nose = new Accessory(new Rectangle(2, 22, 6, 3), new Vector2(30, 110),"nose");
            scarf = new Accessory(new Rectangle(8, 22, 10, 5), new Vector2(40, 110),"scarf");
            handR = new Accessory(new Rectangle(2,27, 9, 7), new Vector2(55, 110),"handR");
            handL = new Accessory(new Rectangle(11, 27, 9, 7), new Vector2(70, 110),"handL");

            drawableSprites.AddRange(new[] {buttons, eyes, nose, scarf, handR, handL});

            base.Initialize();

        }
        private void OnDrop(Object o,EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("dropped");
            if((o as Snowball).name == 0)
            {
                (drawableSprites[1] as Snowball).Drop();
            }
            else if((o as Snowball).name == 1)
            {
                (drawableSprites[2] as Snowball).Drop();
            }
        }
        public static Rectangle GetMouseBoundingBox() => new Rectangle(Mouse.GetState().Position.X / scale, Mouse.GetState().Position.Y / scale, 2, 2);
        public static bool GetSnowballOverCursor(out Snowball snowball)
        {
            foreach (var item in drawableSprites)
            {
                if(item is Snowball && (item as Snowball).boundingBox.Intersects(GetMouseBoundingBox()))
                {
                    snowball = item as Snowball;
                    return true;
                }
            }
            snowball = null;
            return false;
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backdrop = Content.Load<Texture2D>("Art/backdrop_1");
            snowman = Content.Load<Texture2D>("Art/snowman");

        }
        protected override void Update(GameTime gameTime)
        {
            if(Mouse.GetState().LeftButton == ButtonState.Pressed && previosMouseState.LeftButton == ButtonState.Released)
            {
                OnMouseClickEvent?.Invoke(this,EventArgs.Empty);
            }
            scaleMatrix = Matrix.CreateScale(scale, scale, 1);

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer = Math.Min(timer, 10000);

            bigSnowParticles.ForEach(p => p.MoveDown(gameTime));
            foreach (var item in drawableSprites)
            {
                if(item is Snowball)
                {
                    ((Snowball)item).Update(gameTime);
                }
                if(item is Accessory)
                {
                    (item as Accessory).Update(gameTime);
                }
            }

            previosMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp,sortMode:SpriteSortMode.FrontToBack,transformMatrix:scaleMatrix);
            _spriteBatch.Draw(backdrop, Vector2.Zero,new Rectangle(0,0,96,128),Color.White,0,Vector2.Zero,1,SpriteEffects.None,0);
            bigSnowParticles.ForEach(p => p.Draw(_spriteBatch));
            drawableSprites.ForEach(p => p.Draw(_spriteBatch));
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
