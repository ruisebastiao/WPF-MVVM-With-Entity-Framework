using System;

namespace VH.SimpleUI.Entities
{
    [Serializable]
    public enum DialogResult
    {
        /// <summary>
        /// The message box returns no result.
        /// </summary>
        None = 0,
        /// <summary>
        /// The result value of the message box is OK.
        /// </summary>
        Ok = 1,
        /// <summary>
        /// The result value of the message box is Cancel.
        /// </summary>
        Cancel = 2,
        /// <summary>
        /// The result value of the message box is Yes.
        /// </summary>
        Yes = 6,
        /// <summary>
        /// The result value of the message box is No.
        /// </summary>
        No = 7,
    }

    [Serializable]
    public enum DialogButton
    {
        /// <summary>
        /// The message box displays an OK button.
        /// </summary>
        Ok = 0,
        /// <summary>
        /// The message box displays OK and Cancel buttons.
        /// </summary>
        OkCancel = 1,
        /// <summary>
        /// The message box displays Yes and No buttons.
        /// </summary>
        YesNo = 4,
    }

    [Serializable]
    public enum ViewModeType
    {
        /// <summary>
        /// Default mode
        /// </summary>
        Default,
        /// <summary>
        /// Adding new
        /// </summary>
        Add,
        /// <summary>
        /// Edit mode
        /// </summary>
        Edit,
        /// <summary>
        /// View only mode
        /// </summary>
        ViewOnly,
        /// <summary>
        /// Busy mode
        /// </summary>
        Busy
    }

    public enum DialogType
    {
        ByPercentage,
        BySizeInPixel
    }

    public enum MaskType
    {
        Any,
        Integer,
        Decimal
    }
}