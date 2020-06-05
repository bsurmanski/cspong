using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pong {
    public class Ball {
        static Texture2D TEXTURE;

        Rectangle _screen_bounds;
        Vector2 _position;
        Vector2 _velocity;

        public Ball(Rectangle screen_bounds) {
            _screen_bounds = screen_bounds;
            _position = screen_bounds.Center.ToVector2();
        }

        public Rectangle GetBounds() {
            return new Rectangle(TEXTURE.Bounds.X + (int) _position.X,
                                 TEXTURE.Bounds.Y + (int) _position.Y,
                                 TEXTURE.Bounds.Width,
                                 TEXTURE.Bounds.Height);
        }

        public static void SetTexture(Texture2D texture) {
            TEXTURE = texture;
        }

        public static Texture2D GetTexture() {
            return TEXTURE;
        }

        public Vector2 GetVelocity() {
            return _velocity;
        }

        public void SetVelocity(Vector2 velocity) {
            _velocity = velocity;
        }

        public void SetPosition(Vector2 position) {
            _position = position;
        }

        public void Update(GameTime gameTime) {
            _position += _velocity * gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if(GetBounds().Top < _screen_bounds.Top) {
                _position.Y = _screen_bounds.Top;
                _velocity.Y *= -1.0f;
            } else if(GetBounds().Bottom > _screen_bounds.Bottom) {
                _position.Y = _screen_bounds.Bottom - TEXTURE.Height;
                _velocity.Y *= -1.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(TEXTURE, _position, Color.White);
        }
    }
}