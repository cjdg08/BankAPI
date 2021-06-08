using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer.Interface
{
    public interface ITransactionsBusiness
    {
        UserVM BalanceInquiry(string BankAccountNumber, string PIN);
        string Deposit(string ReferenceNumber, int UserID, string BankAccountNumber, double Amount);
        string TransferMoney(string ReferenceNumber, int SourceUserID, string SourceBankAccountNumber, int DestinationUserID, string DestinationBankAccountNumber, double TransferedAmount);
        string Withdrawal(string ReferenceNumber, int UserID, string BankAccountNumber, double WithdrawalAmount);
    }
}
