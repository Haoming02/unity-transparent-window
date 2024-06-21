using System;
using System.Runtime.InteropServices;

public static class TransparentUtils
{
    private static IntPtr window;

    private static bool clickThrough;
    private static bool noIcon;

    public static void Init(bool transparent = false, bool clickthrough = false, bool alwayonstop = false, bool noicon = false)
    {
        window = GetActiveWindow();

        if (transparent)
            EnableTransparency();
        if (alwayonstop)
            EnableAlwaysOnTop();

        clickThrough = clickthrough;
        noIcon = noicon;

        setWindowLong();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    private const int GWL_EXSTYLE = -20;
    private const uint WS_EX_LAYERED = 0x00080000;
    private const uint WS_EX_TRANSPARENT = 0x00000020;
    private const uint WS_EX_TOOLWINDOW = 0x00000080;

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref Margins margins);

    private static void setWindowLong()
    {
        uint style = WS_EX_LAYERED;
        if (clickThrough)
            style |= WS_EX_TRANSPARENT;
        if (noIcon)
            style |= WS_EX_TOOLWINDOW;

        SetWindowLong(window, GWL_EXSTYLE, style);
    }

    public static void EnableClickThrough()
    {
        clickThrough = true;
        setWindowLong();
    }

    public static void DisableClickThrough()
    {
        clickThrough = false;
        setWindowLong();
    }

    public static void ShowIcon()
    {
        noIcon = false;
        setWindowLong();
    }

    public static void HideIcon()
    {
        noIcon = true;
        setWindowLong();
    }

    public static void EnableAlwaysOnTop()
    {
        SetWindowPos(window, HWND_TOPMOST, 0, 0, 0, 0, 0);
    }

    public static void DisableAlwaysOnTop()
    {
        SetWindowPos(window, HWND_NOTOPMOST, 0, 0, 0, 0, 0);
    }

    public static void EnableTransparency()
    {
        Margins transparent = new Margins(-1);
        DwmExtendFrameIntoClientArea(window, ref transparent);
    }

    public static void DisableTransparency()
    {
        Margins solid = new Margins(0);
        DwmExtendFrameIntoClientArea(window, ref solid);
    }

    private struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopWidth;
        public int cxBottomWidth;

        public Margins(int v)
        {
            this.cxLeftWidth = v;
            this.cxRightWidth = v;
            this.cxTopWidth = v;
            this.cxBottomWidth = v;
        }
    }
}
