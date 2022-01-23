using System.Runtime.InteropServices;

namespace CsgoSDK {

    public class Overlay {

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom) {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) {
            }
        }

        public Form OverlayForm { get; private set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private IntPtr GameWindowHandle { get; set; }

        public Overlay(int width, int heigth, IntPtr gameWindowHandle) {
            this.OverlayForm = new Form();

            this.Width = width;
            this.Height = heigth;
            this.GameWindowHandle = gameWindowHandle;

            this.OverlayForm.BackColor = Color.Wheat;
            this.OverlayForm.TransparencyKey = Color.Wheat;
            this.OverlayForm.FormBorderStyle = FormBorderStyle.None;
            this.OverlayForm.ShowIcon = false;
            this.OverlayForm.ShowInTaskbar = false;

            int initalStyle = GetWindowLong(this.OverlayForm.Handle, -20);
            _ = SetWindowLong(this.OverlayForm.Handle, -20, initalStyle | 0x80000 | 0x20);

            this.OverlayForm.Paint += (object? sender, PaintEventArgs args) => {
                this.Init();
            };

            this.OverlayForm.Visible = true;
        }

        public Graphics GetGraphics() {
            return this.OverlayForm.CreateGraphics();
        }

        public void Toggle() {
            this.OverlayForm.Visible = !this.OverlayForm.Visible;
        }

        private void Init() {
            Application.DoEvents();

            this.OverlayForm.Size = new Size(this.Width, this.Height);

            GetWindowRect(this.GameWindowHandle, out RECT GameRect);
            this.OverlayForm.Top = GameRect.Top;
            this.OverlayForm.Left = GameRect.Left;

            this.OverlayForm.Update();
            this.OverlayForm.TopMost = true;
        }
    }
}