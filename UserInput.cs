using System.Windows.Forms;

namespace SolarSystemSimulator
{
    public static class UserInput
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);
        public static bool isKeyPressed(Keys keyCode)
        {
            return GetKeyState((int)keyCode) < 0;
        }

        public static bool isMouseButtonClicked(MouseButtons mouseButton)
        {
            return (Control.MouseButtons & mouseButton) == mouseButton;
        }
    }
}
