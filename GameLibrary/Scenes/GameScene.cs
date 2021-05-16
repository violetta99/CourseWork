using EngineLibrary.Game;
using EngineLibrary.Graphics;
using SoundLibrary;
using SharpDX;
using GameLibrary.Components;
using GameLibrary.Scripts.Utils;
using GameLibrary.Scripts.Game;
using System.Collections.Generic;
using System;
using EngineLibrary.Scripts;
using EngineLibrary.Utils;
using EngineLibrary.Components;
using EngineLibrary.Collisions;
using System.Drawing;
using UILibrary;
using UILibrary.Drawing;
using UILibrary.Containers;
using UILibrary.Backgrounds;
using UILibrary.Elements;
using SharpDX.Mathematics.Interop;
using GameLibrary.Scripts.Bonuses;
using System.Windows.Forms;
using GameLibrary.Scripts.Arrows;

namespace GameLibrary.Scenes
{
    public class GameScene : Scene
    {
        private SharpAudioDevice _audioDevice;

        private UIProgressBar _healthProgressBar;
        private UIText _arrowStandartText;
        private UIText _arrowPoisonText;
        private UIText _wolfCounter;
        private Game3DObject _player;

        private int _currentEnemyCount;

        protected override void InitializeObjects(Loader loader, SharpAudioDevice audioDevice)
        {
            _audioDevice = audioDevice;
            CreatePlayerLoader(loader);
            CreateLevel(loader);
            _wolfCounter.Text = _currentEnemyCount.ToString();
        }

        #region LevelGeneration

        private void CreateLevel(Loader loader)
        {
            // загрузка земли
            string file = @"Resources\Models\ground.fbx";
            var ground = loader.LoadGameObjectFromFile(file, new Vector3(0.0f, -0.1f, 0.0f), Vector3.Zero);
            ground.Tag = "ground";
            ground.Collision = new BoxCollision(200.0f, 0.1f);
            ground.AddComponent(new ColliderComponent());
            ground.AddScript(new ColliderScript());
            AddGameObject(ground);

            CreateBorders(loader);

            Bitmap bitmap = new Bitmap(@"Resources\map.bmp");
            Random random = new Random();
            _currentEnemyCount = 0;

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    float angle = 2 * MathUtil.Pi / random.Next(1, 19);
                    System.Drawing.Color color = bitmap.GetPixel(j, i);

                    if(color.R == 0 && color.G == 0 && color.B == 0)
                    {
                        CreateEnemy(loader, new Vector3(i - 100, 0.25f, j - 100));
                    }
                    else if(color.R == 255 && color.G == 255 && color.B == 0)
                    {
                        CreateStandartArrowsBonus(loader, new Vector3(i - 100, -0.15f, j - 100));
                    }
                    else if(color.R == 0 && color.G == 255 && color.B == 255)
                    {
                        CreatePoisonArrowsBonus(loader, new Vector3(i - 100, -0.15f, j - 100));
                    }
                    else if (color.R == 255 && color.G == 0 && color.B == 255)
                    {
                        CreateHealthBonus(loader, new Vector3(i - 100, -0.15f, j - 100));
                    }
                    else if (color.R == 0 && color.G != 0 && color.B == 0)
                    {
                        CreateTrees(loader, (color.G / 100) + 1, new Vector3(i - 100, -0.5f, j - 100), new Vector3(angle, 0.0f, 0.0f));
                    }
                    else if(color.R != 0 && color.G == 0 && color.B == 0)
                    {
                        CreateStones(loader, (color.R / 100) + 1, new Vector3(i - 100, -0.5f, j - 100), new Vector3(angle, 0.0f, 0.0f));
                    }
                    else if (color.R == 0 && color.G == 0 && color.B != 0)
                    {
                        CreatePlatforms(loader, (color.B / 100) + 1, new Vector3(i - 100, 0.0f, j - 100));
                    }
                }
            }
        }

        private void CreateStandartArrowsBonus(Loader loader, Vector3 position)
        {
            var file = @"Resources\Models\stump.fbx";
            var stump = loader.LoadGameObjectFromFile(file, position, Vector3.Zero);
            file = @"Resources\Models\standartArrows.fbx";
            var bonus = loader.LoadGameObjectFromFile(file, new Vector3(0.0f, 0.2f, 0.0f), Vector3.Zero);
            bonus.Collision = new BoxCollision(0.4f, 0.8f);
            bonus.AddComponent(new ColliderComponent());
            bonus.AddScript(new ColliderScript());
            var voice = new SharpAudioVoice(_audioDevice, @"Resources\Audio\Стрелы.wav");
            bonus.AddScript(new StandartArrowBonus(voice, ArrowType.Standart ,10));

            stump.AddChild(bonus);
            AddGameObject(stump);
        }

        private void CreatePoisonArrowsBonus(Loader loader, Vector3 position)
        {
            var file = @"Resources\Models\stump.fbx";
            var stump = loader.LoadGameObjectFromFile(file, position, Vector3.Zero);
            file = @"Resources\Models\poisonArrows.fbx";
            var bonus = loader.LoadGameObjectFromFile(file, new Vector3(0.0f, 0.2f, 0.0f), Vector3.Zero);
            bonus.Collision = new BoxCollision(0.4f, 0.8f);
            bonus.AddComponent(new ColliderComponent());
            bonus.AddScript(new ColliderScript());
            var voice = new SharpAudioVoice(_audioDevice, @"Resources\Audio\Стрелы.wav");
            bonus.AddScript(new StandartArrowBonus(voice, ArrowType.Poison, 3));

            stump.AddChild(bonus);
            AddGameObject(stump);
        }

        private void CreateHealthBonus(Loader loader, Vector3 position)
        {
            var file = @"Resources\Models\stump.fbx";
            var stump = loader.LoadGameObjectFromFile(file, position, Vector3.Zero);
            file = @"Resources\Models\healthPack.fbx";
            var bonus = loader.LoadGameObjectFromFile(file, new Vector3(0.0f, 0.2f, 0.0f), Vector3.Zero);
            bonus.Collision = new BoxCollision(0.4f, 0.8f);
            bonus.AddComponent(new ColliderComponent());
            bonus.AddScript(new ColliderScript());
            var voice = new SharpAudioVoice(_audioDevice, @"Resources\Audio\Аптечка.wav");
            bonus.AddScript(new HealthBonus(voice, 25));

            stump.AddChild(bonus);
            AddGameObject(stump);
        }

        private void CreateBorders(Loader loader)
        {
            var file = @"Resources\Models\border.fbx";

            var obstacle = loader.LoadGameObjectFromFile(file, new Vector3(-2.0f, -0.2f, 98.0f), Vector3.Zero);
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(200.0f, 20.0f, 8.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);

            obstacle = loader.LoadGameObjectFromFile(file, new Vector3(2.0f, -0.2f, -98.0f), new Vector3(0.0f, 0.0f, MathUtil.Pi));
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(200.0f, 20.0f, 8.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);

            obstacle = loader.LoadGameObjectFromFile(file, new Vector3(-98.0f, -0.2f, -2.0f), new Vector3(0.0f, 0.0f, MathUtil.PiOverTwo));
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(8.0f, 20.0f, 200.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);

            obstacle = loader.LoadGameObjectFromFile(file, new Vector3(98.0f, -0.2f, 2.0f), new Vector3(0.0f, 0.0f, MathUtil.PiOverTwo * 3));
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(8.0f, 20.0f, 200.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);
        }

        private void CreateTrees(Loader loader, int index, Vector3 position, Vector3 rotation)
        {
            var file = @"Resources\Models\tree" + index.ToString() + ".fbx";
            var obstacle = loader.LoadGameObjectFromFile(file, position, rotation);
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(1.5f, 8.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);
        }

        private void CreateStones(Loader loader, int index, Vector3 position, Vector3 rotation)
        {
            var file = @"Resources\Models\stone" + index.ToString() + ".fbx";
            var obstacle = loader.LoadGameObjectFromFile(file, position, rotation);
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(2.7f, 5.0f);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            AddGameObject(obstacle);
        }

        private void CreatePlatforms(Loader loader, int index, Vector3 position)
        {
            var file = @"Resources\Models\platform" + index.ToString() + ".fbx";
            var obstacle = loader.LoadGameObjectFromFile(file, position, Vector3.Zero);
            obstacle.Tag = "obstacle";
            obstacle.Collision = new BoxCollision(2.2f, 0.8f * index);
            obstacle.AddComponent(new ColliderComponent());
            obstacle.AddScript(new ColliderScript());
            var platform = new Game3DObject(new Vector3(0.0f, 1.5f * index, 0.0f), Vector3.Zero);
            platform.Tag = "ground";
            platform.Collision = new BoxCollision(1.8f, 0.8f);
            platform.AddComponent(new ColliderComponent());
            platform.AddScript(new ColliderScript());
            obstacle.AddChild(platform);
            AddGameObject(obstacle);
        }

        private void CreateEnemy(Loader loader, Vector3 position)
        {
            _currentEnemyCount++;
            var file = @"Resources\Models\wolf.fbx";
            var wolf = loader.LoadGameObjectFromFile(file, position, Vector3.Zero);
            wolf.Tag = "enemy";
            wolf.Collision = new BoxCollision(1.5f, 0.7f);
            wolf.AddComponent(new ColliderComponent());
            wolf.AddScript(new ColliderScript());
            var voice_1 = new SharpAudioVoice(_audioDevice, @"Resources\Audio\Лай волков.wav");
            var voice_2 = new SharpAudioVoice(_audioDevice, @"Resources\Audio\Серия укусов.wav");
            wolf.AddComponent(new ReloadComponent(2.5f));
            wolf.AddScript(new ReloadScript());
            var wolfComponent = new EnemyComponent(3, 8.0f);
            wolfComponent.OnEnemyDeath += UpdateWolfCount;
            wolf.AddComponent(wolfComponent);
            wolf.AddScript(new Enemy(voice_1, voice_2, _player, 20.0f, 10));
            file = @"Resources\Models\wolfHead.fbx";
            var head = loader.LoadGameObjectFromFile(file, new Vector3(0.0f, 0.23f, -0.48f), Vector3.Zero);
            wolf.AddChild(head);
            AddGameObject(wolf);
        }

        #endregion

        #region Player
        private void CreatePlayerLoader(Loader loader)
        {
            // создание игрока
            _player = new Game3DObject(new Vector3(0.0f, 10.0f, -5.0f), Vector3.Zero);
            float deltaAngle = Camera.FOVY / Game.RenderForm.ClientSize.Height;
            _player.Tag = "player";
            _player.Collision = new BoxCollision(1.0f, 1.7f);
            _player.AddComponent(new PhysicsComponent());
            _player.AddScript(new PhysicsScript());
            _player.AddComponent(new ColliderComponent());
            _player.AddScript(new ColliderScript());
            var playerComponent = new PlayerComponent();
            playerComponent.OnArrowsCountChanged += UpdateArrowsCountUI;
            playerComponent.OnHealthChanged += (count) => _healthProgressBar.Value = count;
            playerComponent.AddArrows(ArrowType.Standart, 5);
            playerComponent.AddArrows(ArrowType.Poison, 2);
            _player.AddComponent(playerComponent);
            _player.AddScript(new PlayerMovement(deltaAngle, 4f, 0.5f, 2.0f));
            _player.AddChild(Camera);
            // доабвление игроку лука
            string file = @"Resources\Models\bow.fbx";
            var bow = loader.LoadGameObjectFromFile(file, new Vector3(-0.017f, 0.94f, 0.08f), Vector3.Zero);
            _player.AddChild(bow);
            //добавление точки появления стрел игроку
            var arrowParrent = new Game3DObject(new Vector3(-0.052f, 0.95f, 0.0f), Vector3.Zero);
            arrowParrent.AddComponent(new VisibleComponent());
            arrowParrent.AddScript(new VisibleScript());
            // скрипт стрельбы
            var bowShootScript = new BowShoot(2.0f, 1.0f);
            //создание копируемых стрел для игрока
            //обычная стрела
            file = @"Resources\Models\arrow.fbx";
            var arrow = loader.LoadGameObjectFromFile(file, Vector3.Zero, Vector3.Zero);
            arrow.Collision = new BoxCollision(0.3f, 0.2f, 0.3f);
            var arrowComponents = new List<Func<ObjectComponent>>
            {
                () => new ArrowComponent(),
                () => new PhysicsComponent(0.05f),
                () => new ColliderComponent()
            };
            var arrowScripts = new List<Func<Script>>{
                () => new Arrow(arrowParrent, 20.0f, 5.0f),
                () => new PhysicsScript(),
                () => new ColliderScript()
            };
            var arrowTemplate = new CopyableGameObject(arrow, arrowScripts, arrowComponents);
            bowShootScript.AddArrowTemplate(ArrowType.Standart, arrowTemplate);
            //стрела с ядом
            file = @"Resources\Models\arrowPoison.fbx";
            arrow = loader.LoadGameObjectFromFile(file, Vector3.Zero, Vector3.Zero);
            arrow.Collision = new BoxCollision(0.3f, 0.2f, 0.3f);
            arrowComponents = new List<Func<ObjectComponent>>
            {
                () => new ArrowComponent(),
                () => new PhysicsComponent(0.05f),
                () => new ColliderComponent()
            };
            arrowScripts = new List<Func<Script>>{
                () => new PoisonArrow(arrowParrent, 20.0f, 5.0f, 5.0f),
                () => new PhysicsScript(),
                () => new ColliderScript()
            };
            arrowTemplate = new CopyableGameObject(arrow, arrowScripts, arrowComponents);
            bowShootScript.AddArrowTemplate(ArrowType.Poison, arrowTemplate);
            arrowParrent.AddScript(bowShootScript);
            //добавление стрел к точке появления
            file = @"Resources\Models\arrow.fbx";
            arrow = loader.LoadGameObjectFromFile(file, Vector3.Zero, Vector3.Zero);
            arrowParrent.AddChild(arrow);
            file = @"Resources\Models\arrowPoison.fbx";
            arrow = loader.LoadGameObjectFromFile(file, Vector3.Zero, Vector3.Zero);
            arrow.IsHidden = true;
            arrowParrent.AddChild(arrow);
            _player.AddChild(arrowParrent);

            AddGameObject(_player);
        }

        protected override Camera CreateCamera()
        {
            Camera camera = new Camera(new Vector3(0.0f, 1.0f, 0.0f));

            return camera;
        }
        #endregion

        #region UI

        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            Cursor.Hide();        
            context.NewNinePartsBitmap("Panel", loader.LoadBitmapFromFile(@"Resources\UI\panel.png"), 30, 98, 30, 98);
            context.NewBitmap("Heart", loader.LoadBitmapFromFile(@"Resources\UI\heart.png"));
            context.NewBitmap("ArrowStandart", loader.LoadBitmapFromFile(@"Resources\UI\arrowStandart.png"));
            context.NewBitmap("ArrowPoison", loader.LoadBitmapFromFile(@"Resources\UI\arrowPoison.png"));
            context.NewBitmap("WolfIcon", loader.LoadBitmapFromFile(@"Resources\UI\wolfIcon.png"));

            var header1 = new UISequentialContainer(new Vector2(0.0f, 0.0f), new Vector2(screenWidth, 100.0f), false);
            // здоровье
            var counter = new UISequentialContainer(Vector2.Zero, new Vector2(300.0f, 100.0f), false)
            {
                Background = new NinePartsTextureBackground("Panel"),
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            var image = new UIPanel(Vector2.Zero, new Vector2(80.0f, 80.0f))
            {
                Background = new TextureBackground("Heart")
            };
            context.NewSolidBrush("HealthBarBack", new RawColor4(144.0f / 255.0f, 31.0f / 255.0f, 61.0f / 255.0f, 1.0f));
            context.NewSolidBrush("HealthBar", new RawColor4(187.0f / 255.0f, 48.0f / 255.0f, 48.0f / 255.0f, 1.0f));
            _healthProgressBar = new UIProgressBar(Vector2.Zero, new Vector2(180.0f, 20.0f), "HealthBar")
            {
                Background = new ColorBackground("HealthBarBack"),
                MaxValue = 100.0f,
                Value = 50.0f
            };

            counter.Add(image);
            counter.Add(_healthProgressBar);
            header1.Add(new UIMarginContainer(counter, 5.0f, 0.0f));

            // обычные стрелы
            counter = new UISequentialContainer(Vector2.Zero, new Vector2(200.0f, 100.0f), false)
            { 
                Background = new NinePartsTextureBackground("Panel"),
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            image = new UIPanel(Vector2.Zero, new Vector2(80.0f, 80.0f))
            {
                Background = new TextureBackground("ArrowStandart")
            };
            context.NewSolidBrush("Text", new RawColor4(1.0f, 1.0f, 1.0f, 1.0f));
            context.NewTextFormat("Text", fontSize: 40.0f, 
                textAlignment: SharpDX.DirectWrite.TextAlignment.Center);
            _arrowStandartText = new UIText("0", new Vector2(50.0f, 50.0f), "Text", "Text");

            counter.Add(image);
            counter.Add(_arrowStandartText);
            header1.Add(new UIMarginContainer(counter, 5.0f, 0.0f));

            // стрелы яда
            counter = new UISequentialContainer(Vector2.Zero, new Vector2(200.0f, 100.0f), false)
            {
                Background = new NinePartsTextureBackground("Panel"),
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            image = new UIPanel(Vector2.Zero, new Vector2(80.0f, 80.0f))
            {
                Background = new TextureBackground("ArrowPoison")
            };
            _arrowPoisonText = new UIText("0", new Vector2(50.0f, 50.0f), "Text", "Text");

            counter.Add(image);
            counter.Add(_arrowPoisonText);
            header1.Add(new UIMarginContainer(counter, 5.0f, 0.0f));

            var header2 = new UISequentialContainer(new Vector2(5.0f, 110.0f), new Vector2(screenWidth, 100.0f), false);
            // счет врагов
            counter = new UISequentialContainer(Vector2.Zero, new Vector2(200.0f, 100.0f), false)
            {
                Background = new NinePartsTextureBackground("Panel"),
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center
            };
            image = new UIPanel(Vector2.Zero, new Vector2(80.0f, 80.0f))
            {
                Background = new TextureBackground("WolfIcon")
            };
            _wolfCounter = new UIText("0", new Vector2(100.0f, 50.0f), "Text", "Text");
            counter.Add(image);
            counter.Add(_wolfCounter);
            header2.Add(counter);

            var ui = new UIMultiElementsContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight));
            ui.OnResized += () => header1.Size = ui.Size;
            ui.OnResized += () => header2.Size = ui.Size;
            ui.Add(header1);
            ui.Add(header2);
            return ui;
        }

        private void UpdateArrowsCountUI(ArrowType type, int count)
        {
            switch (type)
            {
                case ArrowType.Standart:
                    _arrowStandartText.Text = count.ToString();
                    break;
                case ArrowType.Poison:
                    _arrowPoisonText.Text = count.ToString();
                    break;
            }
        }

        private void UpdateWolfCount()
        {
            _currentEnemyCount--;
            _wolfCounter.Text = _currentEnemyCount.ToString();
            if(_currentEnemyCount <= 0)
            {
                var scene = (MenuScene)PreviousScene;
                scene.UpdateMenu(false);
                Game.ChangeScene(scene);
            }
        }

        #endregion
    }
}
