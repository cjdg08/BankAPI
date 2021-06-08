using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        List<UserVM> GetUsers();
        UserVM GetUsersByBankAccountNumber(string BankAccountNumber);
        UserVM GetUsersByBankAccountNumberAndPIN(string BankAccountNumber, string PIN);
        UserVM GetUsersByCardNumber(string CardNumber);
        int InsertUser(UserVM user);
    }
}
