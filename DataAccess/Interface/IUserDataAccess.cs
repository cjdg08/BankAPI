using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IUserDataAccess
    {
        List<UserDTO> GetUsers();
        UserDTO GetUsersByBankAccountNumber(string BankAccountNumber);
        UserDTO GetUsersByBankAccountNumberAndPIN(string BankAccountNumber, string PIN);
        UserDTO GetUsersByCardNumber(string CardNumber);
        int InsertUser(UserDTO user);
    }
}
