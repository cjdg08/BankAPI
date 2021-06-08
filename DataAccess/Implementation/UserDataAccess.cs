using DataAccess.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Dapper;

namespace DataAccess.Implementation
{
    public class UserDataAccess : IUserDataAccess
    {
        public List<UserDTO> GetUsers()
        {
            List<UserDTO> user = new List<UserDTO>();
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                user = SqlMapper.Query<UserDTO>(con, "GetUsers", commandType: CommandType.StoredProcedure).ToList();
                con.Close();
            }
            return user;
        }

        public UserDTO GetUsersByBankAccountNumber(string BankAccountNumber)
        {
            UserDTO user;
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@BankAccountNumber", BankAccountNumber);
                user = SqlMapper.Query<UserDTO>(con, "GetUsersByBankAccountNumber", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return user;
        }

        public UserDTO GetUsersByBankAccountNumberAndPIN(string BankAccountNumber, string PIN)
        {
            UserDTO user;
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@BankAccountNumber", BankAccountNumber);
                param.Add("@PIN", PIN);
                user = SqlMapper.Query<UserDTO>(con, "GetUsersByBankAccountNumberAndPIN", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return user;
        }

        public UserDTO GetUsersByCardNumber(string CardNumber)
        {
            UserDTO user;
            using (IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("@CardNumber", CardNumber);
                user = SqlMapper.Query<UserDTO>(con, "GetUsersByCardNumber", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return user;
        }

        public int InsertUser(UserDTO user)
        {
            using(IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@BankAccountNumber", user.BankAccountNumber);
                        param.Add("@CardNumber", user.CardNumber);
                        param.Add("@ExpiryDate", user.ExpiryDate);
                        param.Add("@FirstName", user.FirstName);
                        param.Add("@LastName", user.LastName);
                        param.Add("@MiddleName", user.MiddleName);
                        param.Add("@PIN", user.PIN);
                        param.Add("@Balance", user.Balance);
                        param.Add("@IsActive", user.IsActive);

                        user.ID = con.ExecuteScalar<int>("InsertUser", param, tran, commandType: CommandType.StoredProcedure);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
                con.Open();

                return user.ID;
            }
        }
    }
}
