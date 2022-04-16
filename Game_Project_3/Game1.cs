using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Game_Project_3
{
    public class Game1 : Game, IParticleEmitter
    {

        SoundEffect fireworkExplosionSound;

        Song backgroundMusic;

        SpriteFont purposeFont;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D _mouseTexture;

        MouseState _priorMouse;
        KeyboardState _priorKeyboard;

        FireworkParticleSystem _fireworks;

        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }

        public Circles circle;

        public int score;

        public Cube moon;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic her

            StarsParticleSystem rain = new StarsParticleSystem(this, new Rectangle(-10, -100, 800, 500));
            Components.Add(rain);
            _fireworks = new FireworkParticleSystem(this, 20);
            Components.Add(_fireworks);
            PixieParticleSystem pixie = new PixieParticleSystem(this, this);
            Components.Add(pixie);


            circle = new Circles(this);
            Components.Add(circle);

            score = 0;

            // Create the cube
            moon = new Cube(this, Matrix.Identity);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            fireworkExplosionSound = this.Content.Load<SoundEffect>("fireworkExplosion");
            backgroundMusic = this.Content.Load<Song>("renewal");
            MediaPlayer.Play(backgroundMusic);
            purposeFont = Content.Load<SpriteFont>("purposeFont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mouseTexture = this.Content.Load<Texture2D>("mouse");
            Mouse.SetCursor(MouseCursor.FromTexture2D(_mouseTexture, _mouseTexture.Width/2, _mouseTexture.Height/2));
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

            // update the cube 
            //moon.Update(gameTime);

            if (mouserect.Intersects(circle._rectange))
            {
                circle.collision = true;
            }
            else
            {
                circle.collision = false;
                if (((currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released) || (currentKeyboard.IsKeyDown(Keys.Space))))
                {
                    score = 0;
                }
            }


            if (((currentMouse.LeftButton == ButtonState.Pressed && _priorMouse.LeftButton == ButtonState.Released) || (currentKeyboard.IsKeyDown(Keys.Space) && _priorKeyboard.IsKeyUp(Keys.Space))) && mouserect.Intersects(circle._rectange))
            {

                for(int i = 0; i < 5; i++)
                {
                    _fireworks.PlaceFirework(mousePosition);
                }
                circle.updatePosition();
                fireworkExplosionSound.Play(0.3f,0,0);
                score++;
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
            moon.Draw();
            _spriteBatch.Begin();
            _spriteBatch.DrawString(purposeFont, "Score: " + score.ToString(), new Vector2(10,10), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
