using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
    public class Constants
    {
        public const string DefaultRoute = "api/[controller]";
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "HH:mm:ss";

        public const string RoleAdministrator = "Administrator";
        public const string RoleCustomer = "Customer";

        public const string UserDoesntExists = "El usuario no existe";
        public const string InvalidPassword = "Clave Incorrecta";
    }
}
