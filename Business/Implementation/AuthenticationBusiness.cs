using AutoMapper;
using Business.Interface;
using DataAccess.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.Implementation
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private IAuthenticationDataAccess iAuthenticationDataAccess;
        public AuthenticationBusiness(IAuthenticationDataAccess iAuthenticationDataAccess)
        {
            this.iAuthenticationDataAccess = iAuthenticationDataAccess;
        }

        public ApiCredentialVM AuthenticateCredential(string UserName, string Password)
        {
            var DTO = iAuthenticationDataAccess.AuthenticateCredential(UserName, Password);

            // MAP DTO TO VIEWMODEL
            //Mapper.CreateMap<ApiCredentialDTO, ApiCredentialVM>();
            //return Mapper.Map<ApiCredentialVM>(DTO);

            return new ApiCredentialVM();
        }
    }
}
