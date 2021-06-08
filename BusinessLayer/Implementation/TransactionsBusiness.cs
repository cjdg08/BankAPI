using AutoMapper;
using BusinessLayer.Interface;
using DataAccess.Interface;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer.Implementation
{
    public class TransactionsBusiness : ITransactionsBusiness
    {
        private ITransactionsDataAccess iTransactionsDataAccess;
        public TransactionsBusiness(ITransactionsDataAccess iTransactionsDataAccess)
        {
            this.iTransactionsDataAccess = iTransactionsDataAccess;
        }
        public UserVM BalanceInquiry(string BankAccountNumber, string PIN)
        {
            // CALL DATA ACCESS
            var DTO = iTransactionsDataAccess.BalanceInquiry(BankAccountNumber, PIN);

            // MAP DTO TO VIEW MODEL
            Mapper.CreateMap<UserDTO, UserVM>();
            return Mapper.Map<UserVM>(DTO);
        }

        public string Deposit(string ReferenceNumber, int UserID, string BankAccountNumber, double Amount)
        {
            return iTransactionsDataAccess.Deposit(ReferenceNumber, UserID, BankAccountNumber, Amount);
        }

        public string TransferMoney(string ReferenceNumber, int SourceUserID, string SourceBankAccountNumber, int DestinationUserID, string DestinationBankAccountNumber, double TransferedAmount)
        {
            return iTransactionsDataAccess.TransferMoney(ReferenceNumber, SourceUserID, SourceBankAccountNumber, DestinationUserID, DestinationBankAccountNumber, TransferedAmount);
        }

        public string Withdrawal(string ReferenceNumber, int UserID, string BankAccountNumber, double WithdrawalAmount)
        {
            return iTransactionsDataAccess.Withdrawal(ReferenceNumber, UserID, BankAccountNumber, WithdrawalAmount);
        }
    }
}
