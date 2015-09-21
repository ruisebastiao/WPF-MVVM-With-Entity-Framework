using System.Collections.Generic;

namespace VH.Model
{
    public class AppointmentCollection : VHEntityList<Appointment>
    {
        public AppointmentCollection()
        {

        }

        public AppointmentCollection(List<Appointment> list)
            : base(list)
        {

        }
    }
}