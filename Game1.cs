using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle _screen_bounds;
        Ball _ball;
        Paddle _left_paddle;
        Paddle _right_paddle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _screen_bounds = _graphics.GraphicsDevice.Viewport.Bounds;
           
            _ball = new Ball(_screen_bounds);
            _left_paddle = Paddle.CreateHumanPaddle(_screen_bounds, 
                                                    new Vector2(_screen_bounds.Left, 
                                                                _screen_bounds.Center.Y),
                                                    Keys.Up, Keys.Down);
            _right_paddle = Paddle.CreateCpuPaddle(_screen_bounds, 
                                                   new Vector2(_screen_bounds.Right-16/*paddle width*/, 
                                                               _screen_bounds.Center.Y));
            _ball.SetVelocity(new Vector2(0.71f, 0.71f) * 200.0f);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Ball.SetTexture(Content.Load<Texture2D>("ball"));
            Paddle.SetTexture(Content.Load<Texture2D>("paddle"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            _ball.Update(gameTime);
            _left_paddle.Update(gameTime, keyboardState, _ball);
            _right_paddle.Update(gameTime, keyboardState, _ball);

            if (_ball.GetBounds().Left < _screen_bounds.Left) {
                // LEFT GOAL!
            } else if (_ball.GetBounds().Right > _screen_bounds.Right) {
                // RIGHT GOAL!
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _ball.Draw(_spriteBatch);
            _left_paddle.Draw(_spriteBatch);
            _right_paddle.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
