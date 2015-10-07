using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MagicTheGathering
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        internal static Dictionary<string, MagicCard> cards = new Dictionary<string, MagicCard>();
        Player player;
        JSONTable table;
        MagicGame host;
        MagicUI ui;
        Input.InputState inputState = new Input.InputState();
        Texture2D cardBack;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Assembly current = Assembly.GetExecutingAssembly();
            table = JSONTable.parse(new StreamReader(current.GetManifestResourceStream("MagicTheGathering.AllCards.json")).ReadToEnd());
            host = new MagicGame();

            base.Initialize();
        }

        internal static MagicCard LoadCard (string card, GraphicsDevice GraphicsDevice, JSONTable table)
        {
            if (cards.ContainsKey(card))
                return cards[card];
            else
            {
                JSONTable cardTable = table.getJSON(card);
                MagicCardType type = (MagicCardType)0;
                foreach (var item in cardTable.getArray("types").toStringArray())
                {
                    type |= (MagicCardType)Enum.Parse(typeof(MagicCardType), item);
                }
                cards.Add(card, new MagicCard(card, GraphicsDevice, type, cardTable.getArray("subtypes").toStringArray(), new Ability[0], new Cost(new List<CostComponent>() { ManaPaymentComponent.Parse(cardTable.getString("manaCost", "0")) })));
                return cards[card];
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            List<MagicCard> list = new List<MagicCard>() {LoadCard("Memnite", GraphicsDevice, table), LoadCard("Catacomb Slug", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Swamp", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Plains", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Swamp", GraphicsDevice, table), LoadCard("Air Elemental", GraphicsDevice, table), LoadCard("Plains", GraphicsDevice, table), LoadCard("Island", GraphicsDevice, table), LoadCard("Mountain", GraphicsDevice, table), LoadCard("Forest", GraphicsDevice, table) };
            list.Shuffle();
            Deck deck = new NormalDeck(list);
            player = new TestPlayer(host, deck);
            ui = new MagicUI(player, Content, GraphicsDevice);
            host.ReadyGameForStart();
            // TODO: use this.Content to load your game content here

            cardBack = Content.Load<Texture2D>("card-back");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            inputState.Update();
            ui.Update(inputState, GraphicsDevice.Viewport.Bounds);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            ui.Draw(spriteBatch);
            host.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
