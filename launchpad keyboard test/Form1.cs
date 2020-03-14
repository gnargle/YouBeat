using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LaunchpadNET;
using Midi;
using static LaunchpadNET.Interface;

namespace launchpad_keyboard_test {    
    public partial class Form1 : Form {
        public Interface interf;
        public static void testKeyEvent(object sender, LaunchpadKeyEventArgs e) {
            int x, y;
            IntPtr calculatorHandle = FindWindow("CalcFrame", "Calculator");

            // Verify that Calculator is a running process.
            if (calculatorHandle == IntPtr.Zero) {
                MessageBox.Show("Calculator is not running.");
                return;
            }

            //x = e.GetX(); y = e.GetY();
            //if (x == 0 && y == 0) {
            SendKeys.SendWait("w");
            //}
        }

        // Get a handle to an application window.
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);
        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public Form1() {
            InitializeComponent();
            interf = new Interface();
            Random r = new Random();
            var connected = interf.getConnectedLaunchpads();
            if (connected.Count() > 0) {
                if (interf.connect(connected[0])) {
                    interf.OnLaunchpadKeyPressed += new LaunchpadKeyEventHandler(testKeyEvent);
                    var note = interf.ledToMidiNote(2, 2);
                    /* while (true) {
                         for (var x = 0; x < 8; x++) {
                             for (var y = 0; y < 8; y++) {
                                 interf.setLED(x, y, r.Next(0, 128));
                             }
                         }
                     }*/
                }
            }
        }
    }
}
