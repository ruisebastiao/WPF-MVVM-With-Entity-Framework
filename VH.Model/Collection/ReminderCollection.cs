using System.Collections.Generic;

namespace VH.Model
{
    public class ReminderCollection : VHEntityList<Reminder>
    {
          public ReminderCollection()
        {
            
        }

          public ReminderCollection(List<Reminder> list)
              : base(list)
        {

        }
    }
}