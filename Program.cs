using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SymmetricEncryption
{
  class Program
  {
    static void Main(string[] args)
    {
      byte[] plainText = Encoding.ASCII.GetBytes("Hello World!");
      byte[] key = Encoding.ASCII.GetBytes("AAECAwQFBgcICQoLDA0ODw==");
      byte[] encryptedText = EncryptAES(plainText, key);
      Console.WriteLine(Encoding.ASCII.GetString(encryptedText));  
    }

    protected static byte[] CreateRandomByteArray(int length)
    {
      var rngService = new RNGCryptoServiceProvider();
      byte[] buffer = new byte[length];
      rngService.GetBytes(buffer);
      return buffer;
    }

    public static byte[] EncryptAES(byte[] plainText, byte[] key)
    {
      byte[] encrypted;
      var iv = CreateRandomByteArray(16);
      using (var rijndael = Rijndael.Create())
      {
        rijndael.Key = key;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.Mode = CipherMode.CBC;
        rijndael.IV = iv;
        var encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
        using (var memStream = new MemoryStream())
        {
          using (var cryptStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter writer = new StreamWriter(cryptStream))
            {
              writer.Write(plainText);
            }
            encrypted = memStream.ToArray();
          }
        }
        return encrypted;
      }
    }
  }
}
