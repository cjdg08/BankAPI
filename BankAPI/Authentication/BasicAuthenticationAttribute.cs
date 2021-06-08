using BusinessLayer.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BankAPI.Authentication
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public BasicAuthenticationAttribute()
        {
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;

                // decoding authToken we get decode value in 'Username:Password' format  
                var decodeauthToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                // spliting decodeauthToken using ':'   
                var arrUserNameandPassword = decodeauthToken.Split(':');

                // at 0th postion of array we get username and at 1st we get password  
                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    // setting current principle  
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }


        // In this method we can handle our database logic here... 
        public bool IsAuthorizedUser(string Username, string Password)
        {
 
            try
            {
                var data = AuthenticationBusiness.AuthenticateCredential(Username);

                if (data != null)
                {
                    if (data.IsActive)
                    {
                        // DECRYPT PASSWORD USING BCRYPT
                        string decryptedPass = BCrypt.Net.BCrypt.HashPassword(Password, data.Salt);

                        // VERIFY IF PROVIDED PASSWORD IS EQUAL TO PASSWORD IN THE TABLE
                        return decryptedPass == data.Password;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}