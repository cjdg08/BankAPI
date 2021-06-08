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

namespace DataAccess.Static
{
    public static class AuthenticationDataAccess
    {
        public static ApiCredentialDTO AuthenticateCredential(string UserName)
        {
            ApiCredentialDTO apiCredential;
            using(IDbConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BankConString"].ToString()))
            {
                con.Open();
                DynamicParameters param = new DynamicParameters();
                param.Add("UserName", UserName);

                apiCredential = SqlMapper.Query<ApiCredentialDTO>(con, "AuthenticateCredential", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
            }
            return apiCredential;
        }
    }
}
