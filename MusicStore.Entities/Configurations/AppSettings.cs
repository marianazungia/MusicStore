using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities.Configurations
{
    public class AppSettings
    {
        public StorageConfiguration StorageConfiguration { get; set; }

        public Jwt Jwt { get; set; }
    }

    public class Jwt
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string SigningKey { get; set; } = null!;
    }
    public class StorageConfiguration
    {
        public string Path { get; set; } = null!;
        public string PublicUrl { get; set; } = null!;
    }

   
}
