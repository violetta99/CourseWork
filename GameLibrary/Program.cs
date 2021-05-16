using System;
using System.Windows.Forms;
using EngineLibrary.Game;
using SharpDX.Direct3D;
using Device11 = SharpDX.Direct3D11.Device;
using GameLibrary.Scenes;

namespace FuckGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            if (Device11.GetSupportedFeatureLevel() != FeatureLevel.Level_11_0)
            {
                MessageBox.Show("DirectX11 not Supported");
                return;
            }

            Game game = new Game(new MenuScene());
            game.Run();
            game.Dispose();
        }
    }
}
