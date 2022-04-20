using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Helpers
{
    public static class PasswordSecurity
    {
        public static void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }

        public static bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;

                }
            }
            return true;
        }
    }


}
