using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using YouBeat.Scenes;
using System.Runtime.InteropServices;

namespace YouBeat {
    class Program {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static void Main(string[] args) {
            var handle = GetConsoleWindow();
            // Hide
            ShowWindow(handle, SW_HIDE);
            var game = new Game("YouBeat", 1920, 1080, 60, false);
            //var game = new Game("YouBeat", 1920, 1080, 60, true);
            if (game.Debugger != null) {
                ShowWindow(handle, SW_SHOW);
                game.Debugger.ToggleKey = Key.End;
                game.Debugger.ShowPerformance(5);
            }
            game.Color = Color.Grey;
            game.Start(new TitleScene());            
        }        
    }    
}
