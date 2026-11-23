using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace snowman.Scripts
{
    enum BallState
    {
        Waiting,
        Falling,
        Returning

    }
    public class Snowball : Sprite
    {
        public EventHandler OnDroppingEndedEvent { get; set; }
        public Rectangle boundingBox { get; private set; }
        float velocity = 0;
        float speed = 2f;
        int jumps = 2;
        int fallTowardsY;
        float timer;
        BallState state;
        public int name { get; private set; }
        private Dictionary<string, Vector2> accessoryAtachmentPoints;

        public Snowball(Rectangle bounds,int fallTowardsY,int name,float layer,Dictionary<string,Vector2> accessoryAtachmentPoints) 
            : base(bounds, new Vector2(48,-11), layer, true)
        {
            this.fallTowardsY = fallTowardsY;
            this.name = name;
            this.accessoryAtachmentPoints = accessoryAtachmentPoints;
            
        }

        public void Drop() => state = BallState.Falling;
        public bool CanAccessoryBePlaced(string name,out Vector2 position)
        {
            if(accessoryAtachmentPoints is null)
            {
                position = Vector2.Zero;
                return false;
            }
            if (accessoryAtachmentPoints.ContainsKey(name)) { position = accessoryAtachmentPoints[name]; return true; }
            position = Vector2.Zero;
            return false;
        }
        public void Update(GameTime gameTime)
        {
            if(state == BallState.Waiting) { return; }
            velocity += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (state)
            {
                case BallState.Falling:
                    position += new Vector2(0, velocity);
                    break;
                case BallState.Returning:
                    position -= new Vector2(0, velocity);
                    break;
                default:
                    break;
            }

            if (timer <= 0 && state == BallState.Returning)
            {
                velocity = .05f;
                speed *= 2f;
                state = BallState.Falling;
            }

            if (position.Y>= fallTowardsY)
            {
                if(jumps >= 1)
                {
                    velocity = 1;
                    speed /= 2f;
                    state = BallState.Returning;
                    timer = jumps/20f;
                    jumps -= 1;
                }
                else
                {
                    OnDroppingEndedEvent?.Invoke(this, EventArgs.Empty);
                    boundingBox = new Rectangle((int)position.X-(int)origin.X, (int)position.Y-(int)origin.Y, bounds.Width, bounds.Height);
                    state = BallState.Waiting;
                }
            }
            
        }
    }
}
