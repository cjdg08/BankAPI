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
    public class UserBusiness : IUserBusiness
    {
        private IUserDataAccess iUserDataAccess;
        public UserBusiness(IUserDataAccess iUserDataAccess)
        {
            this.iUserDataAccess = iUserDataAccess;
        }

        public List<UserVM> GetUsers()
        {
            // CALL DATA ACCESS
            var DTO = iUserDataAccess.GetUsers();

            // MAPP VIEW MODEL TO DTO
            Mapper.CreateMap<UserDTO, UserVM>();
            return Mapper.Map<List<UserVM>>(DTO);
        }

        public UserVM GetUsersByBankAccountNumber(string BankAccountNumber)
        {
            // CALL DATA ACCESS
            var DTO = iUserDataAccess.GetUsersByBankAccountNumber(BankAccountNumber);

            // MAPP VIEW MODEL TO DTO
            Mapper.CreateMap<UserDTO, UserVM>();
            return Mapper.Map<UserVM>(DTO);
        }

        public UserVM GetUsersByBankAccountNumberAndPIN(string BankAccountNumber, string PIN)
        {
            // CALL DATA ACCESS
            var DTO = iUserDataAccess.GetUsersByBankAccountNumberAndPIN(BankAccountNumber, PIN);

            // MAPP VIEW MODEL TO DTO
            Mapper.CreateMap<UserDTO, UserVM>();
            return Mapper.Map<UserVM>(DTO);
        }

        public UserVM GetUsersByCardNumber(string CardNumber)
        {
            // CALL DATA ACCESS
            var DTO = iUserDataAccess.GetUsersByCardNumber(CardNumber);

            // MAPP VIEW MODEL TO DTO
            Mapper.CreateMap<UserDTO, UserVM>();
            return Mapper.Map<UserVM>(DTO);
        }

        public int InsertUser(UserVM user)
        {
            // MAPP VIEW MODEL TO DTO
            Mapper.CreateMap<UserVM, UserDTO>();
            var DTO = Mapper.Map<UserDTO>(user);

            // CALL DATA ACCESS
            return iUserDataAccess.InsertUser(DTO);
        }
    }
}
