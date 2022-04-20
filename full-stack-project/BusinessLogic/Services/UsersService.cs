using BusinessLogic.Contracts;
using BusinessLogic.Parameters;
using EFCore.EFContext;
using EFCore.UOF;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Helpers;
using BusinessLogic.Responses;

namespace BusinessLogic.Services
{
    /// <summary>
    /// This service manages all users actions
    /// </summary>
    public class UsersService : IUsers
    {
        private readonly EfDataContext _dbContext;
        private readonly IUnitOfWork _iUnitOfContext;
        public UsersService(EfDataContext dbContext, IUnitOfWork iUnitOfContext)
        {
            this._dbContext = dbContext;
            this._iUnitOfContext = iUnitOfContext;
            
        }

        public async Task<Users> GetUserByEmailOrPhone(string UserEmail, string UserPhoneNo)
        {
            return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(c => c.EmailAddress == UserEmail || c.PhoneNumber == UserPhoneNo);
        }
        public async Task<ResponseClass> AddUser(Users user, string Password)
        {
            var userExist = await GetUserByEmailOrPhone(user.EmailAddress, user.PhoneNumber);
            if (userExist != null) return ErrorResponses.GetMessageById(2);

            byte[] passwordHash, passwordSalt;
            PasswordSecurity.CreatePasswordHash(Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash; user.PasswordSalt = passwordSalt;

            await _dbContext.AddAsync(user);
            var res = await _iUnitOfContext.Save();
            if(res > 0) return  CreatedResponseMessages.GetMessageById(1);
            return ErrorCreatedResMessages.GetMessageById(1);

        }

        public async Task<Users> GetUsersByEmail(string UserEmail)
        {
           return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(c=> c.EmailAddress == UserEmail);
        }

        public async Task<Users> GetUsersByPhoneNo(string UserPhoneNo)
        {
            return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(c => c.PhoneNumber == UserPhoneNo);
        }

        public async Task<int> UpdateUser(Users user)
        {
            _dbContext.Update(user);
            return await _iUnitOfContext.Save();
        }

        //user should be able to login using his email or phone number that is why I validated the user with his email or  phone no 
        public async Task<ResponseClass> ValidateUsers(ValidateUsersParam ValidateUsersParam)
        {
            var user = await GetUserByEmailOrPhone(ValidateUsersParam.UserDetail, ValidateUsersParam.UserDetail);

            if (user == null) return ErrorResponses.GetMessageById(1);
            
            if (!PasswordSecurity.VerifyPasswordHash(ValidateUsersParam.Password, user.PasswordHash, user.PasswordSalt)) return ErrorResponses.GetMessageById(1);
            
            if (PasswordSecurity.VerifyPasswordHash(ValidateUsersParam.Password, user.PasswordHash, user.PasswordSalt))  return SuccessResponses.GetMessageById(1);
            
            return ErrorResponses.GetMessageById(1);
        }

        /// <summary>
        /// This function generate user passwordHash and passwordSalt
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Users GenerateUserpasswordHash(string Password)
        {
            byte[] passwordHash, passwordSalt; 
            PasswordSecurity.CreatePasswordHash(Password, out passwordHash, out passwordSalt);

            return new Users
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }

        public async Task<Users> GetUsersByUserId(long UserId)
        {
            return await _dbContext.Users.AsNoTracking().SingleOrDefaultAsync(c => c.Id == UserId);
        }
    }
}
