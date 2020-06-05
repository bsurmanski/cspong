using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pong {
    public class Paddle {
        const float SPEED = 200.0f; // units: pixels / second.
        static Texture2D TEXTURE;

        // This whole cpu/human paddle thing could also be solved using
        // a class hierarchy. We could create a Paddle base class, and a
        // CpuPaddle/HumanPaddle subclasses. But I decided not to get into
        // inheritence for now. Any inheritence problem can be reframed as
        // a class with an enum (or in the case of two subclasses, a bool).
        bool _cpu;
        Rectangle _screen_bounds;
        Vector2 _position;
        int _score;

        // The up/down key only exists if we are a human player.
        // Otherwise it is 'null'. That ? signifies that the type is 'nullable'.
        // Normally, 'Keys' is an enum, and requires a value.
        Keys? _up_key;
        Keys? _down_key;

        public Paddle(Rectangle screen_bounds, Vector2 position, bool cpu, Keys? up, Keys? down) {
            _screen_bounds = screen_bounds;
            _position = position;
            _up_key = up;
            _down_key = down;
            _cpu = cpu;
        }

        public static Paddle CreateCpuPaddle(Rectangle screen_bounds, Vector2 position) {
            return new Paddle(screen_bounds, position, true, null, null);
        }

        public static Paddle CreateHumanPaddle(Rectangle screen_bounds, Vector2 position, Keys up, Keys down) {
            return new Paddle(screen_bounds, position, false, up, down);
        }

        public Rectangle GetBounds() {
            return new Rectangle(TEXTURE.Bounds.X + (int) _position.X,
                                 TEXTURE.Bounds.Y + (int) _position.Y,
                                 TEXTURE.Bounds.Width,
                                 TEXTURE.Bounds.Height);
        }

        // 'Setter' method for TEXTURE. I could make TEXTURE public, but
        // this class is the only spot it should be used, so using this
        // method instead makes it 'write only'.
        public static void SetTexture(Texture2D texture) {
            TEXTURE = texture;
        }

        public int GetScore() {
            return _score;
        }

        public void IncrementScore() {
            _score++;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, Ball ball) {
            // this paddle is computer controlled.
            if (_cpu) {
                Vector2 center = _position + TEXTURE.Bounds.Center.ToVector2();
                Vector2 ball_center = ball.GetBounds().Center.ToVector2();
                if(center.Y < ball_center.Y) {
                    _position += new Vector2(0, Math.Min(SPEED * gameTime.ElapsedGameTime.Milliseconds / 1000.0f,
                                                         ball_center.Y - center.Y));
                } else if(center.Y > ball_center.Y) {
                    _position += new Vector2(0, -Math.Min(SPEED * gameTime.ElapsedGameTime.Milliseconds / 1000.0f,
                                                          center.Y - ball_center.Y));
                }
            } else {
                if(_up_key.HasValue && keyboardState.IsKeyDown(_up_key.Value)) {
                    _position += new Vector2(0, -SPEED * gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                }
                if(_down_key.HasValue && keyboardState.IsKeyDown(_down_key.Value)) {
                    _position += new Vector2(0, SPEED * gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
                }
            }

            // check for ball collision, and respond accordingly. It's a bit weird
            // for the paddles to do this, but since there is only two of them and
            // the response is well defined and restricted, its fineeeee.
            if (GetBounds().Intersects(ball.GetBounds())) {
                Vector2 ballv = ball.GetVelocity();
                ball.SetVelocity(new Vector2(-ballv.X, ballv.Y));

                // This is a weird way to check if we are a 'left' or 'right' paddle.
                // We want to send the ball towards the middle.
                // This isn't really a good physical response, but the game is 
                // pretty constrained, so it's fineeeee.
                if(GetBounds().Center.X < _screen_bounds.Center.X) {
                    ball.SetPosition(new Vector2(GetBounds().Right, ball.GetBounds().Top));
                } else {
                    ball.SetPosition(new Vector2(
                        GetBounds().Left - Ball.GetTexture().Width, 
                        ball.GetBounds().Top));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(TEXTURE, _position, Color.White);
        }
    }
}