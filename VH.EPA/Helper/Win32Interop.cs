using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace VH.View
{
   /// <summary>
    /// Win32 Interop Functions
    /// </summary>
    /// <remarks>
    /// http://blogs.msdn.com/llobo/archive/2006/08/01/Maximizing-window-_2800_with-WindowStyle_3D00_None_2900_-considering-Taskbar.aspx
    /// http://social.msdn.microsoft.com/forums/en-US/wpf/thread/e77c3b58-41f6-4534-8a92-be0f8287b734/
    /// </remarks>
    public static class Win32Interop
    {
        #region · Consts ·

        /// <summary>
        /// Sets a new extended window style
        /// </summary>
        public static readonly Int32 GWL_EXSTYLE                = -20;
        
        /// <summary>
        /// Layered Windows
        /// </summary>
        public static readonly Int32 WS_EX_LAYERED              = 0x00080000;
        
        /// <summary>
        /// Transparent window
        /// </summary>
        public static readonly Int32 WS_EX_TRANSPARENT          = 0x00000020;
        
        private static readonly Int32 MONITOR_DEFAULTTONEAREST  = 0x00000002;

        /// <summary>
        /// Stop flashing. The system restores the window to its original state.
        /// </summary>
        public const UInt32 FLASHW_STOP = 0;

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        public const UInt32 FLASHW_CAPTION = 1;

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        public const UInt32 FLASHW_TRAY = 2;

        /// <summary>
        /// Flash both the window caption and taskbar button. 
        /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        public const UInt32 FLASHW_ALL = 3;

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        public const UInt32 FLASHW_TIMER = 4;

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        public const UInt32 FLASHW_TIMERNOFG = 12;

        #endregion

        #region · Inner Types ·

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        // Struct we'll need to pass to the function
        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public UInt32 cbSize;
            public UInt32 dwTime;
        }

        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            #region · Fields ·

            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int X;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int Y;

            #endregion

            #region · Constructors ·

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            #endregion
        }

        /// <summary>
        /// The MINMAXINFO structure contains information about a window's maximized size and 
        /// position and its minimum and maximum tracking size.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            /// <summary>
            /// Reserved; do not use.
            /// </summary>
            public POINT ptReserved;

            /// <summary>
            /// Specifies the maximized width (POINT.x) and the maximized height (POINT.y) of the window. 
            /// For top-level windows, this value is based on the width of the primary monitor.
            /// </summary>
            public POINT ptMaxSize;

            /// <summary>
            /// Specifies the position of the left side of the maximized window (POINT.x) and 
            /// the position of the top of the maximized window (POINT.y). 
            /// For top-level windows, this value is based on the position of the primary monitor.
            /// </summary>
            public POINT ptMaxPosition;

            /// <summary>
            /// Specifies the minimum tracking width (POINT.x) and the minimum tracking height (POINT.y) of the window. 
            /// This value can be obtained programmatically from the system metrics SM_CXMINTRACK and SM_CYMINTRACK.
            /// </summary>
            public POINT ptMinTrackSize;

            /// <summary>
            /// Specifies the maximum tracking width (POINT.x) and the maximum tracking height (POINT.y) of the window. 
            /// This value is based on the size of the virtual screen and can be obtained programmatically from the 
            /// system metrics SM_CXMAXTRACK and SM_CYMAXTRACK.
            /// </summary>
            public POINT ptMaxTrackSize;
        };

        /// <summary>
        /// The MONITORINFO structure contains information about a display monitor.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// The size of the structure, in bytes.
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// A RECT structure that specifies the display monitor rectangle, expressed in virtual-screen coordinates. 
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// A RECT structure that specifies the work area rectangle of the display monitor, expressed in virtual-screen coordinates. 
            /// Note that if the monitor is not the primary display monitor, some of the rectangle's coordinates may be negative values.
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// A set of flags that represent attributes of the display monitor.
            /// </summary>            
            public int dwFlags = 0;
        }

        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            #region · Operators ·

            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }

            #endregion

            #region · Static Members ·

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            #endregion

            #region · Public Fields ·

            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            #endregion

            #region · Properties ·

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }

            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }

            #endregion

            #region · Constructors ·

            /// <summary>
            /// Win32
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="top">The top.</param>
            /// <param name="right">The right.</param>
            /// <param name="bottom">The bottom.</param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary>
            /// Win32
            /// </summary>
            /// <param name="rcSrc">The rc SRC.</param>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            #endregion

            #region · Methods ·

            /// <summary>
            /// Return a user friendly representation of this struct
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                if (this == RECT.Empty)
                {
                    return "RECT {Empty}";
                }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary>
            /// Determine if 2 RECT are equal (deep compare)
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect))
                {
                    return false;
                }

                return (this == (RECT)obj);
            }

            /// <summary>
            /// Return the HashCode for this struct (not garanteed to be unique)
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }

            #endregion
        }

        #endregion

        #region · Static Methods ·

        public static void FlashWindow(IntPtr handle)
        {
            IntPtr      hWnd    = handle;
            FLASHWINFO  fInfo   = new FLASHWINFO();

            fInfo.cbSize    = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd      = hWnd;
            fInfo.dwFlags   = Win32Interop.FLASHW_ALL |
                              Win32Interop.FLASHW_TIMERNOFG |
                              Win32Interop.FLASHW_CAPTION;

            fInfo.uCount    = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            FlashWindowEx(ref fInfo);
        }

        /// <summary>
        /// Gets the application idle time
        /// </summary>
        /// <remarks>
        /// http://www.geekpedia.com/tutorial210_Retrieving-the-Operating-System-Idle-Time-Uptime-and-Last-Input-Time.html
        /// </remarks>
        /// <returns></returns>
        public static int GetIdleTime()
        {
            // Get the system uptime		
            int systemUptime = Environment.TickCount;
            // The tick at which the last input was recorded		
            int lastInputTicks = 0;
            // The number of ticks that passed since last input
            int idleTicks = 0;

            // Set the struct		
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            // If we have a value from the function		
            if (GetLastInputInfo(ref lastInputInfo))
            {
                // Get the number of ticks at the point when the last activity was seen		
                lastInputTicks = (int)lastInputInfo.dwTime;
                // Number of idle ticks = system uptime ticks - number of ticks at last input		
                idleTicks = systemUptime - lastInputTicks;
            }

            return (idleTicks / 1000);
        }

        /// <summary>
        /// Window Proc
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <param name="handled">if set to <c>true</c> [handled].</param>
        /// <returns></returns>
        public static System.IntPtr WindowProc(
              System.IntPtr hwnd,
              int msg,
              System.IntPtr wParam,
              System.IntPtr lParam,
              ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (System.IntPtr)0;
        }

        /// <summary>
        /// Get the min max size of a window
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="lParam">The l param.</param>
        public static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            Win32Interop.MINMAXINFO mmi = (Win32Interop.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Win32Interop.MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            System.IntPtr monitor = Win32Interop.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                Win32Interop.MONITORINFO monitorInfo = new Win32Interop.MONITORINFO();
                Win32Interop.GetMonitorInfo(monitor, monitorInfo);
                Win32Interop.RECT rcWorkArea = monitorInfo.rcWork;
                Win32Interop.RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.X = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        #endregion

        #region · P/Invoke Functions ·

        [DllImport("user32.dll")]
        static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Gets information about a display monitor.
        /// </summary>
        /// <param name="hMonitor">The h monitor.</param>
        /// <param name="lpmi">The lpmi.</param>
        /// <returns></returns>
        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        /// <summary>
        /// Gets a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified windo
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        /// <summary>
        /// Gets the cursor's position, in screen coordinates.
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);

        /// <summary>
        /// Gets information about the specified window. 
        /// The function also retrieves the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        /// <summary>
        /// Changes an attribute of the specified window.
        /// The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="newVal"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 newVal);

        #endregion
    }
}
