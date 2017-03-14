using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using TRCAttributes;

namespace TRCUtilities {

  /// <summary>
  /// This class is used to decrypt the config file values that are encrypted.
  /// The following variable values must not be altered or the decryption will fail:
  ///     encryptionKey = 097D19DDC6D101C2E0530A0A3FD801C2
  ///     salt = { 10, 12, 133, 55, 61, 11, 186, 52 }
  ///     iterations = 1000
  /// </summary>
  [Author("Tom Kraft", version = 1.0)]
  [Author("Timothy Tosh", version = 1.1)]
  public class EncryptionUtil {

    private const string EncryptionKey = "097D19DDC6D101C2E0530A0A3FD801C2"; //DO NOT ALTER THE VALUE

    private static Rijndael GetRijndael() {
      var derived = KeyFromString(EncryptionKey);
      var rijndael = Rijndael.Create();
      rijndael.Key = derived.GetBytes(256/8);
      rijndael.IV = derived.GetBytes(rijndael.BlockSize/8);
      return rijndael;
    }

    private static Rfc2898DeriveBytes KeyFromString(string password) {
      var salt = new byte[] {10, 12, 133, 55, 61, 11, 186, 52}; //DO NOT ALTER THE VALUES
      const int iterations = 1000; //DO NOT ALTER THE VALUE
      return new Rfc2898DeriveBytes(password, salt, iterations);
    }

    /// <exception cref="NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> 
    /// associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  
    /// For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Write" />  with an underlying stream that is read only.  </exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref>
    ///     <name>offset</name>
    ///   </paramref>
    ///   parameter is less than zero.-or- The <paramref>
    ///     <name>count</name>
    ///   </paramref>
    ///   parameter is less than zero. </exception>
    /// <exception cref="ArgumentException">The sum of the <paramref>
    ///     <name>count</name>
    ///   </paramref>
    ///   and <paramref>
    ///     <name>offset</name>
    ///   </paramref>
    ///   parameters is longer than the length of the buffer. </exception>
    /// <exception cref="CryptographicException">The key is corrupt which can cause invalid padding to the stream. </exception>
    /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>buffer</name>
    ///   </paramref>
    ///   is null. </exception>
    public static string Encrypt(string valueToEncrypt) {
      var encoding = new UTF8Encoding();
      var bytes = encoding.GetBytes(valueToEncrypt);
      using (var rijndael = GetRijndael())
      using (var encryptor = rijndael.CreateEncryptor())
      using (var stream = new MemoryStream())
      using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write)) {
        crypto.Write(bytes, 0, bytes.Length);
        crypto.FlushFinalBlock();
        stream.Position = 0;
        var encrypted = new byte[stream.Length];
        stream.Read(encrypted, 0, encrypted.Length);
        return Convert.ToBase64String(encrypted);
      }
    }

    /// <exception cref="NotSupportedException">The <see cref="T:System.Security.Cryptography.CryptoStreamMode" /> 
    /// associated with current <see cref="T:System.Security.Cryptography.CryptoStream" /> object does not match the underlying stream.  
    /// For example, this exception is thrown when using <see cref="F:System.Security.Cryptography.CryptoStreamMode.Write" />  with an underlying stream that is read only.  </exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref>
    ///     <name>offset</name>
    ///   </paramref>
    ///   parameter is less than zero.-or- The <paramref>
    ///     <name>count</name>
    ///   </paramref>
    ///   parameter is less than zero. </exception>
    /// <exception cref="ArgumentException">The sum of the <paramref>
    ///     <name>count</name>
    ///   </paramref>
    ///   and <paramref>
    ///     <name>offset</name>
    ///   </paramref>
    ///   parameters is longer than the length of the buffer. </exception>
    /// <exception cref="CryptographicException">The key is corrupt which can cause invalid padding to the stream. </exception>
    /// <exception cref="ObjectDisposedException">The stream is closed. </exception>
    /// <exception cref="ArgumentNullException"><paramref>
    ///     <name>buffer</name>
    ///   </paramref>
    ///   is null. </exception>
    /// <exception cref="DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)
    /// -and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
    public static string Decrypt(string encrypted) {
      if (!IsBase64String(encrypted))
        return string.Empty;
      var encryptedValue = Convert.FromBase64String(encrypted);

      var encoding = new UTF8Encoding();
      using (var rijndael = GetRijndael())
      using (var decryptor = rijndael.CreateDecryptor())
      using (var stream = new MemoryStream())
      using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write)) {
        crypto.Write(encryptedValue, 0, encryptedValue.Length);
        crypto.FlushFinalBlock();
        stream.Position = 0;
        var decryptedBytes = new Byte[stream.Length];
        stream.Read(decryptedBytes, 0, decryptedBytes.Length);
        return encoding.GetString(decryptedBytes);
      }
    }

    private static bool IsBase64String(string s) {
      s = s.Trim();
      return (s.Length%4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
    }
  }
}