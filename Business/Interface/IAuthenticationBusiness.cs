using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Business.Interface
{
    public interface IAuthenticationBusiness
    {
        ApiCredentialVM AuthenticateCredential(string UserName, string Password);
    }
}
