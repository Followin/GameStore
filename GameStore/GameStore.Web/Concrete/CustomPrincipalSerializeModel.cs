using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Concrete
{
    public class CustomPrincipalSerializeModel
    {
        public Int32 Id { get; set; }
        public String SessionId { get; set; }
    }
}