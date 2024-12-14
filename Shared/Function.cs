using System.Text;
using System.IO.Hashing;
using tenant_service.Core.Entities.Landlord;
using tenant_service.Core.Entities;
using tenant_service.Shared;

namespace tenant_service.Shared
{
    public class Function
    {
        /// <summary>
        /// Generate string by length input
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Generated string</returns>
        public static string GenerateRandomString(int length = 32)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                var index = random.Next(chars.Length);
                result.Append(chars[index]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Generate connection string base on Tenant info, config database
        /// </summary>
        /// <param name="databaseInfo"></param>
        /// <param name="tenant"></param>
        /// <returns>Connection string</returns>
        public static string GenerateConnectionStringBaseOnTenant(DatabaseInfo databaseInfo, Tenant tenant)
        {
            return $"server={databaseInfo.Server};port={databaseInfo.Port};user={databaseInfo.User};password={databaseInfo.Password};database={tenant.TenantId};";
        }

        /// <summary>
        /// Get connection string server only
        /// </summary>
        /// <param name="databaseInfo"></param>
        /// <returns>Connection string</returns>
        public static string GetConnectionStringServerOnly(DatabaseInfo databaseInfo)
        {
            return $"server={databaseInfo.Server};port={databaseInfo.Port};user={databaseInfo.User};password={databaseInfo.Password};";
        }

        /// <summary>
        /// HashString using CRC32 algorithm
        /// </summary>
        /// <param name="str"></param>
        /// <returns>uint hashValue</returns>
        public static uint HashStringCRC32(string str)
        {
            // Convert it into bytes array
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);

            // Using Crc32 to hash recent array
            byte[] hashBytes = Crc32.Hash(inputBytes);

            // convert it into unit
            uint hashValue = BitConverter.ToUInt32(hashBytes, 0) % ConfigGlobal.MaxAllowUser;

            return hashValue;
        }
    }
}