using Widget.Base;

namespace Widget.Clock
{
    public class ClockSettings : XmlSerializable
    {
        public ClockSettings()
        {
            Autolock = true;
            AutolockTime = 300; //seconds
        }

        public bool Autolock { get; set; }
        public int AutolockTime { get; set; }
        public string LockScreenBg { get; set; }
    }
}
