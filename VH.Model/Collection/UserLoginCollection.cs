using System.Collections.Generic;

namespace VH.Model
{
    public class UserLoginCollection : VHEntityList<UserLogin>
    {
        public UserLoginCollection()
        {
            
        }

        public UserLoginCollection(List<UserLogin> list)
            : base(list)
        {

        }
         
    }
}