using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace snowman.Scripts
{
    public class Particle
    {
        public Vector2 position { get; set; }
        public float scale { get;set;  }
        public float rotation { get; set; }

        private float moveSpeed;
        private Color color;
        private float offset;

        public Particle(Vector2 position,float scale,float moveSpeed,Color color)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = 0f;
            this.moveSpeed = moveSpeed;
            this.color = color;
            offset = (float)Game1.random.NextDouble();
        }
        public void MoveDown(GameTime gameTime)
        {
            position += new Vector2(0,(float)gameTime.ElapsedGameTime.TotalSeconds * moveSpeed);
            
            if (position.Y >= 130)
            {
                ResetPos();
            }
        }
        private void ResetPos() => position = new Vector2(Game1.random.Next(0, 97), -1);
        private float Wave(float amplitude, float phase) =>(amplitude*MathF.Sin((Game1.timer+offset) *phase));
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.pointTexture, position+new Vector2(Wave(.25f,6),0), new Rectangle(0, 0, 1, 1), color, Wave(.25f,4), Vector2.Zero, scale, SpriteEffects.None, .9f);
        }



    }
}
