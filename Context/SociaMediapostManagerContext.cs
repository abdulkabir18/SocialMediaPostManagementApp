using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaPostManager.Context
{
    public class SociaMediapostManagerContext
    {
        public static string connectionString = "server=localhost;database=socialMediaPostManagerdb;User=root;Password=Pa$$word";
        public MySqlConnection OpenConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static string HashPassword(string password)
        {
            #region MD5
            var hashSalt = "userclhhashsalthashingpassword";
            var hashPassword = MD5.HashData(Encoding.UTF8.GetBytes(password + hashSalt));
            string hashPasswordResult = Convert.ToBase64String(hashPassword);
            #endregion
            return hashPasswordResult;
        }
    }
}