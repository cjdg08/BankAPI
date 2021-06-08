using BusinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankAPI.Controllers
{
    public class TransactionsController : ApiController
    {
        private ITransactionsBusiness iTransactionsBusiness;
        private IUserBusiness iUserBusiness;
        public TransactionsController(ITransactionsBusiness iTransactionsBusiness, IUserBusiness iUserBusiness)
        {
            this.iTransactionsBusiness = iTransactionsBusiness;
            this.iUserBusiness = iUserBusiness;
        }

        [HttpGet]
        [Route("api/BalanceInquiry")]
        public IHttpActionResult BalanceInquiry(string BankAccountNumber, string PIN)
        {
            try
            {
                // CALL METHOD TO GET DATA
                var user = iTransactionsBusiness.BalanceInquiry(BankAccountNumber, PIN);
                if(user == null) // CHECK IF NO RETURN DATA
                {
                    HttpStatusCode errCode = (HttpStatusCode)400;
                    return Content(errCode, "Invalid Account");
                }
                else
                {
                    if(user.IsActive) // CHECK ACCOUNT IS ACTIVE
                    {
                        responseBalInqData resData = new responseBalInqData();
                        resData.BankAccountNumber = user.BankAccountNumber;
                        resData.FullName = user.LastName + ", " + user.FirstName + " " + user.MiddleName;
                        resData.Balance = user.Balance;
                        return Ok(resData);
                    }
                    else
                    {
                        return Ok("Account is inactive");
                    }
                }
            }
            catch(Exception ex)
            {
                HttpStatusCode errCode = (HttpStatusCode)500;
                return Content(errCode, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Withdrawal")]
        public IHttpActionResult Withdrawal(string BankAccountNumber, string PIN, double WithdrawalAmount)
        {
            try
            {
                // CREATE REFERENCE NUMBER
                string refNumber = "Ref_" + DateTime.Now.ToString("yyyyMMddHHmmssfffff");

                if (WithdrawalAmount <= 0) // VALIDATE PROVIDED WITHDRAWAL AMOUNT IF LESS THAN OR EQUAL TO ZERO THEN RETURN "Invalid withdrawal amount" MESSAGE
                {
                    return Ok("Invalid withdrawal amount");
                }

                // CALL METHOD TO GET USER INFORMATION USING BANK ACCOUNT NUMBER AND PIN
                var user = iUserBusiness.GetUsersByBankAccountNumberAndPIN(BankAccountNumber, PIN);
                if(user == null) // CHECK IF NO RETURN DATA, THEN RETURN "Account is invalid" MESSAGE
                {
                    return Ok("Account is invalid");
                }
                else
                {
                    if(user.IsActive) // VALIDATE IF ACCOUNT INFORMATION IS ACTIVE
                    {
                        if(user.Balance >= WithdrawalAmount) // VALIDATE IF ACCOUNT BALANCE IF NOT INSUFFICIENT
                        {
                            // CALL METHOD TO PROCESS WITHDRAWAL
                            string result = iTransactionsBusiness.Withdrawal(refNumber, user.ID, user.BankAccountNumber, WithdrawalAmount);
                            if (result == "Success") // IF METHOD RETURN SUCCESS, RETURN SUCCESS WITH RESPONSE DATA
                            {
                                responseWithdrawal resWithdraw = new responseWithdrawal();
                                resWithdraw.ReferenceNumber = refNumber;
                                resWithdraw.BankAccountNumber = user.BankAccountNumber;
                                resWithdraw.FullName = user.LastName + ", " + user.FirstName + " " + user.MiddleName;
                                resWithdraw.WithdrawalAmount = WithdrawalAmount;
                                resWithdraw.RemainingBalance = user.Balance - WithdrawalAmount;

                                return Ok(resWithdraw);
                            }
                            else // IF METHOD RETURN ERROR, RETURN HTTP 500 STATUS CODE
                            {
                                HttpStatusCode errCode = (HttpStatusCode)500;
                                return Content(errCode, "Error processing withdrawal");
                            }
                        }
                        else // IF ACCOUNT BALANCE IS LESS THAN WITHDRAWAL AMOUNT, RETURN "Insufficient fund" MESSAGE
                        {
                            return Ok("Insufficient fund");
                        }
                    }
                    else // IF ACCOUNT INFORMATION IS INACTIVE, RETURN "Account is inactive." MESSAGE
                    {
                        return Ok("Account is inactive.");
                    }
                }
            }
            catch (Exception ex)
            {
                HttpStatusCode errCode = (HttpStatusCode)500;
                return Content(errCode, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/Deposit")]
        public IHttpActionResult Deposit(string BankAccountNumber, double Amount)
        {
            try
            {
                // CREATE REFERENCE NUMBER
                string refNumber = "Ref_" + DateTime.Now.ToString("yyyyMMddHHmmssfffff");

                if (Amount <= 0) // VALIDATE PROVIDED AMOUNT IF LESS THAN OR EQUAL TO ZERO, RETURN "Invalid amount" MESSAGE
                {
                    return Ok("Invalid amount");
                }

                // GET USER INFORMATION USING BANK ACCOUNT NUMBER
                var user = iUserBusiness.GetUsersByBankAccountNumber(BankAccountNumber);
                if(user == null) // CHECK IF NO RETURN DATA, THEN RETURN "Account is invalid" MESSAGE
                {
                    return Ok("Account is invalid");
                }
                else
                {
                    if (user.IsActive) // VALIDATE ACCOUNT INFORMATION IS ACTIVE
                    {
                        // CALL METHOD TO PROCESS WITHDRAWAL
                        string result = iTransactionsBusiness.Deposit(refNumber, user.ID, user.BankAccountNumber, Amount);
                        if (result == "Success") // IF METHOD RETURN SUCCESS, RETURN SUCCESS WITH RESPONSE DATA
                        {
                            responseDeposit resDeposit = new responseDeposit();
                            resDeposit.ReferenceNumber = refNumber;
                            resDeposit.BankAccountNumber = user.BankAccountNumber;
                            resDeposit.FullName = user.LastName + ", " + user.FirstName + " " + user.MiddleName;
                            resDeposit.DepositAmount = Amount;
                            resDeposit.TotalBalance = user.Balance + Amount;

                            return Ok(resDeposit);
                        }
                        else // IF METHOD RETURN ERROR, RETURN HTTP 500 STATUS CODE
                        {
                            HttpStatusCode errCode = (HttpStatusCode)500;
                            return Content(errCode, "Error processing withdrawal");
                        }
                    }
                    else // IF ACCOUNT INFORMATION IS INACTIVE, RETURN "Account is inactive." MESSAGE
                    {
                        return Ok("Account is inactive.");
                    }
                }
            }
            catch (Exception ex)
            {
                HttpStatusCode errCode = (HttpStatusCode)500;
                return Content(errCode, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/TransferMoney")]
        public IHttpActionResult TransferMoney(string SourceBankAccountNumber, string DestinationBankAccountNumber, double Amount)
        {
            try
            {
                // CREATE REFERENCE NUMBER
                string refNumber = "Ref_" + DateTime.Now.ToString("yyyyMMddHHmmssfffff");

                // CALL METHOD TO GET USER DATA BY SOURCE BANK ACCOUNT NUMBER
                var source = iUserBusiness.GetUsersByBankAccountNumber(SourceBankAccountNumber);
                if(source == null) // CHECK IF NO RETURN DATA, RETURN, "Source Account is invalid." MESSAGE
                {
                    return Ok("Source Account is invalid.");
                }
                else
                {
                    if(!source.IsActive) // CHECK IF RETURNED DATA IS INACTIVE, RETURN "Source Account is inactive."MESSAGE
                    {
                        return Ok("Source Account is inactive.");
                    }
                    else
                    {
                        if(source.Balance < Amount) // CHECK IF SOURCE ACCOUNT BALANCE IS LESS THAN AMOUNT, RETURN "Insufficient fund." MESSAGE
                        {
                            return Ok("Insufficient fund.");
                        }
                        else
                        {
                            // IF SOURCE ACCOUNT IS VALID, CALL METHOD TO GET USER DATA BY DESTINATION BANK ACCOUNT NUMBER
                            var destination = iUserBusiness.GetUsersByBankAccountNumber(DestinationBankAccountNumber);
                            if (destination == null) // CHECK IF NO RETURN DATA, RETURN "Destination Account is invalid." MESSAGE
                            {
                                return Ok("Destination Account is invalid.");
                            }
                            else
                            {
                                if (!destination.IsActive) // CHECK IF RETURNED DATA IS INACTIVE, RETURN "Destination Account is inactive." MESSAGE
                                {
                                    return Ok("Destination Account is inactive.");
                                }
                                else
                                {
                                    // CALL METHOD TO PROCESS TRANSFER MONEY
                                    string result = iTransactionsBusiness.TransferMoney(refNumber, source.ID, source.BankAccountNumber, destination.ID, destination.BankAccountNumber, Amount);
                                    if(result == "Success") // IF METHOD RETURN SUCCESS, RETURN SUCCESS WITH RESPONSE DATA
                                    {
                                        responseTransfer resTransfer = new responseTransfer();
                                        resTransfer.ReferenceNumber = refNumber;
                                        resTransfer.SourceBankAccountNumber = source.BankAccountNumber;
                                        resTransfer.SourceName = source.LastName + ", " + source.FirstName + " " + source.MiddleName;
                                        resTransfer.DestinationBankAccountNumber = destination.BankAccountNumber;
                                        resTransfer.DestinationName = destination.LastName + ", " + destination.FirstName + " " + destination.MiddleName;
                                        resTransfer.TransferedAmount = Amount;

                                        return Ok(resTransfer);
                                    }
                                    else // IF METHOD RETURN ERROR, RETURN HTTP 500 STATUS CODE
                                    {
                                        HttpStatusCode errCode = (HttpStatusCode)500;
                                        return Content(errCode, "Error processing transfer money");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                HttpStatusCode errCode = (HttpStatusCode)500;
                return Content(errCode, ex.Message);
            }
        }


        #region response data
        public class responseBalInqData
        {
            public string BankAccountNumber { get; set; }
            public string FullName { get; set; }
            public double Balance { get; set; }
        }

        public class responseWithdrawal
        {
            public string ReferenceNumber { get; set; }
            public string BankAccountNumber { get; set; }
            public string FullName { get; set; }
            public double WithdrawalAmount { get; set; }
            public double RemainingBalance { get; set; }
        }

        public class responseDeposit
        {
            public string ReferenceNumber { get; set; }
            public string BankAccountNumber { get; set; }
            public string FullName { get; set; }
            public double DepositAmount { get; set; }
            public double TotalBalance { get; set; }
        }

        public class responseTransfer
        {
            public string ReferenceNumber { get; set; }
            public string SourceBankAccountNumber { get; set; }
            public string SourceName { get; set; }
            public string DestinationBankAccountNumber { get; set; }
            public string DestinationName { get; set; }
            public double TransferedAmount { get; set; }
        }
        #endregion
    }
}
