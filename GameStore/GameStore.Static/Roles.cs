using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Static
{
    public static class Roles
    {
        public static String Admin
        {
            get { return "Admin"; }
        }

        public static String Manager
        {
            get { return "Manager"; }
        }

        public static String Moderator
        {
            get { return "Moderator"; }
        }

        public static String User
        {
            get { return "User"; }
        }
    }
}
