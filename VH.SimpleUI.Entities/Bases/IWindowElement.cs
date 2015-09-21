using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VH.SimpleUI.Entities;

namespace VH.Bases
{
    public interface IWindowElement
    {
        string Title { get; }
        double DialogStartupSizePercentage { get; }
        double DialogStartupCustomHeight { get; }
        double DialogStartupCustomWidth { get; }
        bool ShowMinimizeButton { get; }
        bool ShowMaximizeButton { get; }
        bool ShowCloseButton { get; }
        string StatusMessage { get; set; }
        string NotificationMessage { get; set; }
        bool IsActive { get; set; }
        ViewModeType ViewMode { get; set; }
        DialogType DialogType { get; }
        

    }
}
