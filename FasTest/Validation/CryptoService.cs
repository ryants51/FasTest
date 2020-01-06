using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FasTest.Validation
{
    public static class CryptoService
    {

        public static string GenerateSalt()
        {

            // Define min and max salt sizes.
            int minSaltSize = 6;
            int maxSaltSize = 10;

            // Generate a random number for the size of the salt.
            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            byte[] saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(saltBytes);

            return hashValue;
        }

        public static string ComputePasswordHash(string plainText)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            SHA256Managed hash = new SHA256Managed();
            
            // Compute hash value of the plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
        
            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashBytes);

            hash.Dispose();

            return hashValue;
        }

        public static string ComputeHash(string plainText,
                                  byte[] saltBytes)
        {
            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            
             SHA256Managed hash = new SHA256Managed();
           
            // Compute hash value of the plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            hash.Dispose();

            // Return the result.
            return hashValue;
        }

        public static bool ValidateHash(string hashedPassword, string plainText, string saltBytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Clear();
            string actualPassword = stringBuilder.Append(hashedPassword + saltBytes).ToString();
            stringBuilder.Clear();
            string attemptedPassword = stringBuilder.Append(ComputePasswordHash(plainText) + saltBytes).ToString();
            stringBuilder.Clear();
            
            return actualPassword == attemptedPassword;
        }

            public static bool VerifyHash(string plainText, string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;
            
            // Size of the hash
            hashSizeInBits = 256;
            
            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =  ComputeHash(plainText, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }
    }
    
}
