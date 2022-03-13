using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Project_3
{
    public class Game1 : Game, IParticleEmitter
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState _priorMouse;
        KeyboardState _priorKeyboard;

        FireworkParticleSystem _fireworks;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Circles circle;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            StarsParticleSystem rain = new StarsParticleSystem(this, new Rectangle(-10, -100, 800, 500));
            Components.Add(rain);
            _fireworks = new FireworkParticleSystem(this, 20);
            Components.Add(_fireworks);
            PixieParticleSystem pixie = new PixieParticleSystem(this, this);
            Components.Add(pixie);


            circle = new Circles(this);
            Components.Add(circle);
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState currentMouse = Mouse.GetState();
            KeyboardState currentKeyboard = Keyboard.GetState();
            Vector2 mousePosition = new Vector2(currentMouse.X, currentMouse.Y);
            var mouserect = new Rectangle(mousePosition.ToPoint(), mousePosition.ToPoint());

            if (((currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released) || (currentKeyboard.IsKeyDown(Keys.Space) && _priorKeyboard.IsKeyUp(Keys.Space))) && mouserect.Intersects(circle._rectange))
            {
                _fireworks.PlaceFirework(mousePosition);
                circle.updatePosition();
            }

            Velocity = mousePosition - Position;
            Position = mousePosition;

            _priorKeyboard = currentKeyboard;
            _priorMouse = currentMouse;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
