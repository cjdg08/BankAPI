using AutoMapper;
using DataAccess.Static;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer.Static
{
    public static class AuthenticationBusiness
    {

        public static ApiCredentialVM AuthenticateCredential(string UserName)
        {
            // CALL DATA ACCESS
            var DTO = AuthenticationDataAccess.AuthenticateCredential(UserName);

            // MAP DTO TO VIEWMODEL
            Mapper.CreateMap<ApiCredentialDTO, ApiCredentialVM>();
            return Mapper.Map<ApiCredentialVM>(DTO);
        }
    }
}
