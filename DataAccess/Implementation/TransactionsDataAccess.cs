using Dapper;
using DataAccess.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Implementation
{
    public class TransactionsDataAccess : ITransactionsDataAccess
    {
        public UserDTO BalanceInquiry(string BankAccountNumber, string PIN)
        {
            UserDTO user;
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@BankAccountNumber", BankAccountNumber);
                param.Add("@PIN", PIN);
                user = SqlMapper.Query<UserDTO>(con, "BalanceInquiry", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return user;
        }

        public string Deposit(string ReferenceNumber, int UserID, string BankAccountNumber, double Amount)
        {
            string result = "";
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ReferenceNumber", ReferenceNumber);
                        param.Add("@UserID", UserID);
                        param.Add("@BankAccountNumber", BankAccountNumber);
                        param.Add("@Amount", Amount);

                        con.Execute("Deposit", param, tran, commandType: CommandType.StoredProcedure);

                        result = "Success";
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
                con.Close();
            }
            return result;
        }

        public string TransferMoney(string ReferenceNumber, int SourceUserID, string SourceBankAccountNumber, int DestinationUserID, string DestinationBankAccountNumber, double TransferedAmount)
        {
            string result = "";
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ReferenceNumber", ReferenceNumber);
                        param.Add("@SourceUserID", SourceUserID);
                        param.Add("@SourceBankAccountNumber", SourceBankAccountNumber);
                        param.Add("@DestinationUserID", DestinationUserID);
                        param.Add("@DestinationBankAccountNumber", DestinationBankAccountNumber);
                        param.Add("@TransferedAmount", TransferedAmount);

                        con.Execute("TransferMoney", param, tran, commandType: CommandType.StoredProcedure);

                        result = "Success";
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
                con.Close();
            }
            return result;
        }

        public string Withdrawal(string ReferenceNumber, int UserID, string BankAccountNumber, double WithdrawalAmount)
        {
            string result = "";
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ReferenceNumber", ReferenceNumber);
                        param.Add("@UserID", UserID);
                        param.Add("@BankAccountNumber", BankAccountNumber);
                        param.Add("@WithdrawalAmount", WithdrawalAmount);

                        con.Execute("Withdrawal", param, tran, commandType: CommandType.StoredProcedure);

                        result = "Success";
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = "Error";
                        tran.Rollback();
                    }
                }
                con.Close();
            }
            return result;
        }
    }
}
