using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class Extensions
    {
        public static Roles StringToRole(this string enumRaw)
        {
            if (Enum.TryParse(enumRaw, out Roles output))
            {
                return output;
            }
            throw new Exception("Unable to parse enum");
        }
    }
}
