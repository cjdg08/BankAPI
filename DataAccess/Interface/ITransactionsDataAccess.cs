using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface ITransactionsDataAccess
    {
        UserDTO BalanceInquiry(string BankAccountNumber, string PIN);
        string Deposit(string ReferenceNumber, int UserID, string BankAccountNumber, double Amount);
        string TransferMoney(string ReferenceNumber, int SourceUserID, string SourceBankAccountNumber, int DestinationUserID, string DestinationBankAccountNumber, double TransferedAmount);
        string Withdrawal(string ReferenceNumber, int UserID, string BankAccountNumber, double WithdrawalAmount);
    }
}
