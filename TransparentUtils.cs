using System;
using System.Runtime.InteropServices;

namespace Utils
{
    /// <summary>
    /// <b>Important:</b> <br/>
    /// - Disable the <b>Flip Model Swapchain</b> player setting <br/>
    /// - Only the <b>built-in</b> render pipeline is supported <br/>
    /// <b>Credits:</b> <br/>
    /// - Reference: <see href="https://youtu.be/RqgsGaMPZTw?feature=shared">CodeMonkey</see> <br/>
    /// - Resource: <see href="https://www.pinvoke.net/">PINVOKE.NET</see>
    /// </summary>
    public static class TransparentUtils
    {
        public enum Mode
        {
            /// <summary>If you just want to hide the Taskbar icon</summary>
            None,
            /// <summary>Turns areas with specified the color click-through</summary>
            Chroma,
            /// <summary>Manually control when window becomes click-through</summary>
            Manual
        }

        private static bool _init = false;
        private static Mode _mode;
        private static IntPtr _window;

        private static bool __editor = false;

        public static void Init(Mode mode, bool alwaysOnTop = false, bool hideTaskbar = false)
        {
#if !UNITY_STANDALONE_WIN
            throw new NotImplementedException("These features are only avaliable on Windows...");
#endif

#if UNITY_EDITOR
            // Click-Through can make the Editor unusable...
            __editor = true;
#endif

            if (__editor)
                return;

            if (_init)
                throw new SystemException("Init can only be called once...");

            _window = WinAPI.GetActiveWindow();
            if (_window == IntPtr.Zero)
                throw new NullReferenceException("Failed to get Active Window...");

            _mode = mode;
            switch (_mode)
            {
                default:
                    _init = true;
                    break;
                case Mode.Chroma:
                    SetupChroma();
                    break;
                case Mode.Manual:
                    SetupManual();
                    break;
            }

            SetTaskbar(hideTaskbar);
            if (_mode != Mode.None)
                SetAlwaysOnTop(alwaysOnTop);
        }

        private static void SetupChroma()
        {
            var style = (uint)WinAPI.GetWindowLong(_window, Flags.GWL_EXSTYLE);
            style |= Flags.WS_EX_LAYERED;
            WinAPI.SetWindowLongPtr(_window, Flags.GWL_EXSTYLE, (IntPtr)style);

            uint black = 0;
            WinAPI.SetLayeredWindowAttributes(_window, black, 0, Flags.LWA_COLORKEY);

            _init = true;
        }

        private static void SetupManual()
        {
            var transparent = new Margins(-1);
            WinAPI.DwmExtendFrameIntoClientArea(_window, ref transparent);

            var style = (uint)WinAPI.GetWindowLong(_window, Flags.GWL_EXSTYLE);
            style |= Flags.WS_EX_LAYERED;
            style |= Flags.WS_EX_TRANSPARENT;
            WinAPI.SetWindowLongPtr(_window, Flags.GWL_EXSTYLE, (IntPtr)style);

            _init = true;
        }

        public static void SetChromaKey(byte r, byte g, byte b)
        {
            if (!_init) return;
            if (_mode != Mode.Chroma)
                throw new SystemException("Chroma Mode is required...");

            var key = (uint)((b << 16) | (g << 8) | (r));
            WinAPI.SetLayeredWindowAttributes(_window, key, 0, Flags.LWA_COLORKEY);
        }

        public static void SetClickThrough(bool allow)
        {
            if (!_init) return;
            if (_mode != Mode.Manual)
                throw new SystemException("Manual Mode is required...");

            var style = (uint)WinAPI.GetWindowLong(_window, Flags.GWL_EXSTYLE);

            if (allow)
                style |= Flags.WS_EX_TRANSPARENT;
            else
                style &= ~Flags.WS_EX_TRANSPARENT;

            WinAPI.SetWindowLongPtr(_window, Flags.GWL_EXSTYLE, (IntPtr)style);
        }

        public static void SetAlwaysOnTop(bool enable)
        {
            if (!_init) return;

            if (enable)
                WinAPI.SetWindowPos(_window, Flags.HWND_TOPMOST, 0, 0, 0, 0, 0);
            else
                WinAPI.SetWindowPos(_window, Flags.HWND_NOTOPMOST, 0, 0, 0, 0, 0);
        }

        public static void SetTaskbar(bool hideIcon)
        {
            if (!_init) return;

            var style = (uint)WinAPI.GetWindowLong(_window, Flags.GWL_EXSTYLE);

            if (hideIcon)
                style |= Flags.WS_EX_TOOLWINDOW;
            else
                style &= ~Flags.WS_EX_TOOLWINDOW;

            WinAPI.SetWindowLongPtr(_window, Flags.GWL_EXSTYLE, (IntPtr)style);
        }

        private static class Flags
        {
            public const int GWL_EXSTYLE = -20;

            public const uint WS_EX_LAYERED = 0x00080000;
            public const uint WS_EX_TRANSPARENT = 0x00000020;
            public const uint WS_EX_TOOLWINDOW = 0x00000080;

            public const uint LWA_COLORKEY = 0x00000001;

            public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        }

        private static class WinAPI
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetActiveWindow();

            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
            public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll")]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            [DllImport("user32.dll")]
            public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

            [DllImport("dwmapi.dll")]
            public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margins);
        }

        private struct Margins
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cxTopHeight;
            public int cxBottomHeight;

            public Margins(int margin)
            {
                cxLeftWidth = margin;
                cxRightWidth = margin;
                cxTopHeight = margin;
                cxBottomHeight = margin;
            }
        }
    }
}
