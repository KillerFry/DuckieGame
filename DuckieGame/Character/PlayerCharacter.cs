using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckieGame.Character
{
    class PlayerCharacter
    {
        public Point Position { get; set; }
        public Texture2D Texture { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(Texture,
                new Vector2(
                    20 + Position.X * 10,
                    20 + Position.Y * 10), Color.White);
        }
    }
}
