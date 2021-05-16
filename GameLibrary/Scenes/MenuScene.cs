using UILibrary;
using UILibrary.Drawing;
using EngineLibrary.Game;
using SharpDX;
using UILibrary.Containers;
using UILibrary.Backgrounds;
using SharpDX.Mathematics.Interop;
using UILibrary.Elements;
using System.Windows.Forms;

namespace GameLibrary.Scenes
{
    public class MenuScene : Scene
    {
        private UISequentialContainer _menu;
        private UISequentialContainer _endGamePanel;
        private UIText _endGameText;

        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            context.NewBitmap("Background", loader.LoadBitmapFromFile(@"Resources\UI\menu.png"));
            context.NewNinePartsBitmap("Panel", loader.LoadBitmapFromFile(@"Resources\UI\panel.png"), 25, 103, 25, 103);
            context.NewNinePartsBitmap("ButtonPressed", loader.LoadBitmapFromFile(@"Resources\UI\buttonPressed.png"), 30, 98, 30, 98);
            context.NewNinePartsBitmap("ButtonRealesed", loader.LoadBitmapFromFile(@"Resources\UI\buttonRealesed.png"), 30, 98, 30, 98);

            context.NewSolidBrush("TextLight", new RawColor4(238.0f / 255.0f, 201.0f / 255.0f, 159.0f / 255.0f, 1.0f));
            context.NewSolidBrush("TextDark", new RawColor4(120.0f / 255.0f, 58.0f / 255.0f, 41.0f / 255.0f, 1.0f));
            context.NewTextFormat("Text", fontSize: 35.0f, textAlignment: SharpDX.DirectWrite.TextAlignment.Center, 
                paragraphAlignment: SharpDX.DirectWrite.ParagraphAlignment.Center);

            var ui = new UIMultiElementsContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                Background = new TextureBackground("Background")
            };

            DrawMenu(screenWidth, screenHeight);
            DrawEndPanel(screenWidth, screenHeight);

            ui.OnResized += () => _menu.Size = ui.Size;
            ui.OnResized += () => _endGamePanel.Size = ui.Size;

            ui.Add(_menu);          
            ui.Add(_endGamePanel);          

            return ui;
        }

        private void DrawMenu(int screenWidth, int screenHeight)
        {
            _menu = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
            };

            var container = new UISequentialContainer(Vector2.Zero, new Vector2(400.0f, 400.0f))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
                Background = new NinePartsTextureBackground("Panel")
            };

            var text = new UIText("Охота на волков", new Vector2(300.0f, 50.0f), "Text", "TextLight");
            container.Add(new UIMarginContainer(text, 0.0f, 20.0f));

            text = new UIText("Новая игра", new Vector2(250.0f, 70.0f), "Text", "TextDark");
            var button = new UIButton(new Vector2(250.0f, 70.0f), text)
            {
                ReleasedBackground = new NinePartsTextureBackground("ButtonRealesed"),
                PressedBackground = new NinePartsTextureBackground("ButtonPressed")
            };
            button.OnClicked += () => Game.ChangeScene(new GameScene());
            container.Add(new UIMarginContainer(button, 0.0f, 20.0f));

            text = new UIText("Выход", new Vector2(250.0f, 70.0f), "Text", "TextDark");
            button = new UIButton(new Vector2(250.0f, 70.0f), text)
            {
                ReleasedBackground = new NinePartsTextureBackground("ButtonRealesed"),
                PressedBackground = new NinePartsTextureBackground("ButtonPressed")
            };
            button.OnClicked += () => Game.CloseProgramm();
            container.Add(new UIMarginContainer(button, 0.0f, 20.0f));
            _menu.Add(container);
        }
        
        private void DrawEndPanel(int screenWidth, int screenHeight)
        {
            _endGamePanel = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
            };
            var container = new UISequentialContainer(Vector2.Zero, new Vector2(400.0f, 400.0f))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
                Background = new NinePartsTextureBackground("Panel")
            };

            _endGameText = new UIText("Вы выиграли", new Vector2(300.0f, 50.0f), "Text", "TextLight");
            container.Add(new UIMarginContainer(_endGameText, 0.0f, 20.0f));

            var text = new UIText("Ок", new Vector2(250.0f, 70.0f), "Text", "TextDark");
            var button = new UIButton(new Vector2(250.0f, 70.0f), text)
            {
                ReleasedBackground = new NinePartsTextureBackground("ButtonRealesed"),
                PressedBackground = new NinePartsTextureBackground("ButtonPressed")
            };
            button.OnClicked += () =>
            {
                _endGamePanel.IsVisible = false;
                _menu.IsVisible = true;
            };
            container.Add(new UIMarginContainer(button, 0.0f, 20.0f));
            _endGamePanel.Add(container);
            _endGamePanel.IsVisible = false;
        }

        public void UpdateMenu(bool isLose)
        {
            Cursor.Show();
            if (isLose)
            {
                _endGameText.Text = "Вы проиграли";
            }
            else
            {
                _endGameText.Text = "Вы выиграли";
            }
            _menu.IsVisible = false;
            _endGamePanel.IsVisible = true;
        }
    }
}
