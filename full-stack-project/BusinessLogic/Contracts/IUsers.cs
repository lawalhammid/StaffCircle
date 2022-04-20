using BusinessLogic.Parameters;
using BusinessLogic.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IUsers
    {
        Task<ResponseClass> AddUser(Users user, string Password);
        Task<int> UpdateUser(Users user);
        Task<Users> GetUsersByEmail(string UserEmail);
        Task<Users> GetUsersByPhoneNo(string UserPhoneNo);
        Task<ResponseClass> ValidateUsers(ValidateUsersParam ValidateUsersParam);
        Users GenerateUserpasswordHash(string Password);
        Task<Users> GetUserByEmailOrPhone(string UserEmail, string UserPhoneNo);
        Task<Users> GetUsersByUserId(long UserId);
    }
}