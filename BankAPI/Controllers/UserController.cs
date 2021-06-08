using BankAPI.Authentication;
using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using ViewModel;

namespace BankAPI.Controllers
{
    [BasicAuthentication]
    public class UserController : ApiController
    {
        private IUserBusiness iUserBusiness;
        public UserController(IUserBusiness iUserBusiness)
        {
            this.iUserBusiness = iUserBusiness;
        }

        [HttpPost]
        [Route("api/AddUser")]
        public IHttpActionResult AddUser([FromBody] UserVM user)
        {
            try
            {
                // VALIDATE REQUEST BODY HERE
                HttpStatusCode errCode;
                if (user == null)
                {
                    errCode = (HttpStatusCode)400;
                    return Content(errCode, "Request body not found.");
                }
                if (String.IsNullOrEmpty(user.BankAccountNumber))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "Bank Account Number is required.");
                }
                else
                {
                    if(Regex.Matches(user.BankAccountNumber, @"^[0-9]+$").Count <= 0)
                    {
                        errCode = (HttpStatusCode)422;
                        return Content(errCode, "Bank Account Number is invalid.");
                    }
                }
                if (String.IsNullOrEmpty(user.CardNumber))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "Card Number is required.");
                }
                else
                {
                    if (Regex.Matches(user.CardNumber, @"^[0-9]+$").Count <= 0)
                    {
                        errCode = (HttpStatusCode)422;
                        return Content(errCode, "Card Number is invalid.");
                    }
                }
                if (user.ExpiryDate == null || user.ExpiryDate == DateTime.Parse("01/01/0001"))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "Expiry Date is required.");
                }
                if (String.IsNullOrEmpty(user.FirstName))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "First Name is required.");
                }
                if (String.IsNullOrEmpty(user.LastName))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "Last Name is required.");
                }
                if (String.IsNullOrEmpty(user.MiddleName))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "Middle Name is required.");
                }
                if (String.IsNullOrEmpty(user.PIN))
                {
                    errCode = (HttpStatusCode)422;
                    return Content(errCode, "PIN is required.");
                }
                else
                {
                    if (Regex.Matches(user.PIN, @"^[0-9]+$").Count <= 0)
                    {
                        errCode = (HttpStatusCode)422;
                        return Content(errCode, "PIN is invalid.");
                    }
                }

                // CALL METHOD TO GET USERS DATA AND VALIDATE IF EXISTING
                var users = iUserBusiness.GetUsersByBankAccountNumber(user.BankAccountNumber);
                if(users != null)
                {
                    errCode = (HttpStatusCode)400;
                    return Content(errCode, "Bank Account Number already exist.");
                }

                users = iUserBusiness.GetUsersByCardNumber(user.CardNumber);
                if (users!= null)
                {
                    errCode = (HttpStatusCode)400;
                    return Content(errCode, "Card Number already exist.");
                }

                // CALL METHOD TO INSERT DATA TO TABLE
                user.ID = iUserBusiness.InsertUser(user);

                if (user.ID > 0)
                {
                    return Ok("User successfully saved!");
                }
                else
                {
                    errCode = (HttpStatusCode)500;
                    return Content(errCode, "Error saving information");
                }
            }
            catch (Exception ex)
            {
                HttpStatusCode errCode = (HttpStatusCode)500;
                return Content(errCode, ex.Message);
            }
        }
    }
}
