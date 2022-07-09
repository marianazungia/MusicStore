using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Response
{
    public class DtoLoginResponse : BaseResponse
    {
        public string Token { get; set; }

        public string FullName { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
