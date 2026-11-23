using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snowman.Scripts
{
    public class Sprite
    {
        internal Rectangle bounds { get; private set; }
        public Vector2 position;
        internal Vector2 origin { get;private set;  }
        private float layer;
        public Sprite(Rectangle bounds, Vector2 position,float layer,bool centered = false)
        {
            this.bounds = bounds;
            this.position = position;
            this.layer = layer;

            if (centered)
            {
                origin = new Vector2(bounds.Width / 2f, bounds.Height / 2f);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.snowman, position, bounds, Color.White, 0, origin, 1, SpriteEffects.None, layer);
        }
    }
}
