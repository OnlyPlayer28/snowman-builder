using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace snowman.Scripts
{
    internal class Accessory : Sprite
    {
        Vector2 startingPos;
        bool isDragging = false;
        bool canBePixkedUp = true;

        private string name;

        public Rectangle boundingBox { get; private set; }
        public Accessory(Rectangle bounds, Vector2 position,string name) 
            : base(bounds, position, .2f, false)
        {
            startingPos = position;
            this.name = name;
            Game1.OnMouseClickEvent += OnMouseClicked;
        }

        private void OnMouseClicked(Object o,EventArgs e)
        {
            if (!canBePixkedUp) { return; }
            if(isDragging&&Game1.GetSnowballOverCursor(out Snowball ball))
            {
                if (ball.CanAccessoryBePlaced(this.name,out Vector2 pos))
                {
                    position = pos+ball.position-ball.origin;
                    isDragging = false;
                    canBePixkedUp = false;
                }
                return;
            }
            if (Game1.GetMouseBoundingBox().Intersects(boundingBox))
            {
                if (canBePixkedUp)
                {
                    isDragging = !isDragging;
                    if (!isDragging) { position = startingPos; }
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            if (isDragging) { position = new Vector2(Mouse.GetState().Position.X/Game1.scale, Mouse.GetState().Position.Y / Game1.scale); }
            boundingBox = new Rectangle((int)position.X,(int)position.Y,bounds.Width,bounds.Height);
        }
    }
}
