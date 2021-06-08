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

        /*
         * 
         */

        [HttpPost]
        [Route("api/AddUser")]
        public IHttpActionResult AddUser([FromBody] UserVM user)
        {
            try
            {
                // VALIDATE REQUEST BODY
                HttpStatusCode errCode;
                if (user == null) // IF NO REQUEST BODY PROVIDED, RETURN 400 BAD REQUEST STATUS CODE WITH "Request body not found." MESSAGE
                {
                    errCode = (HttpStatusCode)400;
                    return Content(errCode, "Request body not found.");
                }
                if (String.IsNullOrEmpty(user.BankAccountNumber))
                {
                    return Ok("Bank Account Number is required.");
                }
                else
                {
                    if(Regex.Matches(user.BankAccountNumber, @"^[0-9]+$").Count <= 0)
                    {
                        return Ok("Bank Account Number is invalid.");
                    }
                }
                if (String.IsNullOrEmpty(user.CardNumber))
                {
                    return Ok("Card Number is required.");
                }
                else
                {
                    if (Regex.Matches(user.CardNumber, @"^[0-9]+$").Count <= 0)
                    {
                        return Ok("Card Number is invalid.");
                    }
                }
                if (user.ExpiryDate == null || user.ExpiryDate == DateTime.Parse("01/01/0001"))
                {
                    return Ok("Expiry Date is required.");
                }
                if (String.IsNullOrEmpty(user.FirstName))
                {
                    return Ok("First Name is required.");
                }
                if (String.IsNullOrEmpty(user.LastName))
                {
                    return Ok("Last Name is required.");
                }
                if (String.IsNullOrEmpty(user.MiddleName))
                {
                    return Ok("Middle Name is required.");
                }
                if (String.IsNullOrEmpty(user.PIN))
                {
                    return Ok("PIN is required.");
                }
                else
                {
                    if (Regex.Matches(user.PIN, @"^[0-9]+$").Count <= 0)
                    {
                        return Ok("PIN is invalid.");
                    }
                }

                // CALL METHOD TO GET USER DATA BY BANK ACCOUNT NUMBER
                var users = iUserBusiness.GetUsersByBankAccountNumber(user.BankAccountNumber);
                if(users != null) // CHECK IF NO RETURN DATA, RETURN "Bank Account Number already exist." MESSAGE
                {
                    return Ok("Bank Account Number already exist.");
                }

                // CALL METHOD TO GET USER DATA BY CARD NUMBER
                users = iUserBusiness.GetUsersByCardNumber(user.CardNumber);
                if (users != null) // CHECK IF NO RETURN DATA, RETURN "Card Number already exist." MESSAGE
                {
                    return Ok("Card Number already exist.");
                }

                // CALL METHOD TO INSERT DATA TO TABLE AND RETURN CREATED ID
                user.ID = iUserBusiness.InsertUser(user);

                if (user.ID > 0) // IF CREATED ID IS GREATER THAN ZERO, RETURN "User successfully saved!" MESSAGE
                {
                    return Ok("User successfully saved!");
                }
                else // IF NO CREATED ID, RETURN 500 STATUS CODE WITH "Error saving information" MESSAGE
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
