using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.DataAccess
{
    public class MusicStoreUserIdentity : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public int Age { get; set; }
        public TypeDocument TypeDocument { get; set; }

        [StringLength(20)]
        public string DocumentNumber { get; set; }

        public MusicStoreUserIdentity()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            DocumentNumber = string.Empty;
        }
    }

    public enum TypeDocument
    {
        Dni,
        Pasaporte
    }
}
