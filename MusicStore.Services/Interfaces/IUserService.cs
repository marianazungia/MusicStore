using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponseGeneric<string>> RegisterAsync(DtoRegisterUser request);

        Task<DtoLoginResponse> LoginAsync(DtoLogin request);
         Task<BaseResponseGeneric<string>> SendTokenToResetPasswordAsync(DtoResetPassword request);
         Task<BaseResponseGeneric<string>> ResetPassword(DtoConfirmReset request);

        Task<BaseResponse> ChangePassword(DtoChangePassword request);
    }
}
