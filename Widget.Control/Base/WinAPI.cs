using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Widget.Base
{
    public static class WinAPI
    {
        public const int GwlExstyle = -20;

        public const int GwlStyle = -16;

        public const uint WM_SETICON = 0x0080;
        public const uint WM_CLOSE = 0x0010;

        public const int WS_EX_DLGMODALFRAME = 0x0001;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_NOACTIVATE = 0x0010;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;

        public const int WsExToolwindow = 0x00000080;

        public const int Flip3D = 8;

        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int cxLeftWidth;      // width of left border that retains its size
            public int cxRightWidth;     // width of right border that retains its size
            public int cyTopHeight;      // height of top border that retains its size
            public int cyBottomHeight;   // height of bottom border that retains its size
        };

        public struct BbStruct //Blur Behind Structure
        {
            public BbFlags Flags;
            public bool Enable;
            public IntPtr Region;
            public bool TransitionOnMaximized;
        }

        [Flags]
        public enum BbFlags : byte //Blur Behind Flags
        {
            DwmBbEnable = 1,
            DwmBbBlurregion = 2,
            DwmBbTransitiononmaximized = 4,
        };

        public enum Flip3DPolicy
        {
            Default = 0,
            ExcludeBelow,
            ExcludeAbove
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public const uint SHGFI_ICON = 0x000000100;     // get icon
        public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
        public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
        public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmEnableBlurBehindWindow(IntPtr hWnd, ref BbStruct blurBehind);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(out bool enabled);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr window, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr window, int index, uint value);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int xradius, int yradius);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateEllipticRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("kernel32")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeConsole();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("shell32")]
        public static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        public static void RemoveWindowIcon(IntPtr hwnd)
        {
            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GwlExstyle);
            SetWindowLong(hwnd, GwlExstyle, (uint)extendedStyle | WS_EX_DLGMODALFRAME);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
                  SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowPlacement")]
        private static extern bool InternalGetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
        public static bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement placement)
        {
            placement = new WindowPlacement();
            placement.Length = Marshal.SizeOf(typeof(WindowPlacement));
            return InternalGetWindowPlacement(hWnd, ref placement);
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string classname, string title);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("dwmapi.dll")]
        public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr source, out IntPtr hthumbnail);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUnregisterThumbnail(IntPtr HThumbnail);

        [DllImport("dwmapi.dll")]
        public static extern int DwmUpdateThumbnailProperties(IntPtr HThumbnail, ref ThumbnailProperties props);

        [DllImport("dwmapi.dll")]
        public static extern int DwmQueryThumbnailSourceSize(IntPtr HThumbnail, out Size size);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetText(IntPtr hWnd)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public enum WindowShowStyle : uint
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        public struct Point
        {
            public int x;
            public int y;
        }

        public struct Size
        {
            public int Width, Height;
        }

        public struct WindowPlacement
        {
            public int Length;
            public int Flags;
            public int ShowCmd;
            public Point MinPosition;
            public Point MaxPosition;
            public Rect NormalPosition;
        }

        public struct ThumbnailProperties
        {
            public ThumbnailFlags Flags;
            public Rect Destination;
            public Rect Source;
            public Byte Opacity;
            public bool Visible;
            public bool SourceClientAreaOnly;
        }

        public struct Rect
        {
            public Rect(int x, int y, int x1, int y1)
            {
                this.Left = x;
                this.Top = y;
                this.Right = x1;
                this.Bottom = y1;
            }

            public int Left, Top, Right, Bottom;
        }

        [Flags]
        public enum ThumbnailFlags : int
        {
            RectDetination = 1,
            RectSource = 2,
            Opacity = 4,
            Visible = 8,
            SourceClientAreaOnly = 16
        }

        public enum GetWindowCmd : uint
        {
            First = 0,
            Last = 1,
            Next = 2,
            Prev = 3,
            Owner = 4,
            Child = 5,
            EnabledPopup = 6
        }

        [Flags]
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,

        }

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
    }
}
