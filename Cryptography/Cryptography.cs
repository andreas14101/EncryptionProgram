using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Cryptography;

public class Ase
{
    private Boolean stop = false;
    
    /**
     * method that keeps the program running when not exited
     */
     public void aseEncrypt(byte[] key)
    {
        while (!stop)
        {
                    Console.WriteLine("1: Encrypt");
                    Console.WriteLine("2: Decrypt");
                    Console.WriteLine("0: Quit");
                    Console.WriteLine("-----------");
                    string? what = Console.ReadLine();
                    Console.WriteLine("-----------");
                  
                    
                    
                    
            
                    if (what.Equals("1"))
                    {
                        Console.WriteLine("please enter text:");
                        string? enteredText = Console.ReadLine();
                        Console.WriteLine("-----------");
                        EncryptWithNet(enteredText,key);
                        Console.WriteLine("-----------");
                    }
                    else if (what.Equals("2"))
                    {
                        DecryptWithNet(key);
                        Console.WriteLine("-----------");
                    }
                    else if(what.Equals("0"))
                    {
                        stop = true;
                    }
        }
       
    }

    /**
     * Decrypts the the encrypted text from a file
     */
    private void DecryptWithNet(byte[] key)
    {
        using (var aes = new AesGcm(key))
        {

            try
            {
                Byte[] iv = new byte[] {};
                Byte[] tag = new byte[]{};
                Byte[] ciphertxt = new byte[]{};
                try
                {
                    string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    // Open the text file using a stream reader.
                    using (var sr = new StreamReader(docPath+"/WriteLines.txt"))
                    {
                        iv = Convert.FromBase64String(sr.ReadLine());
                        tag = Convert.FromBase64String(sr.ReadLine());
                        ciphertxt = Convert.FromBase64String(sr.ReadLine());
                    }
                }
                catch (IOException e)
                {
                    throw new Exception("failed to read file", e);
                }
                
                
                var plaintextBytes = new byte[ciphertxt.Length];

                aes.Decrypt(iv, ciphertxt, tag, plaintextBytes);

                writedcoded(plaintextBytes);
                readdecoded();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        } 
    }

    
    /**
     * Encrypts the text that is given by the user
     */
    private void  EncryptWithNet(string plaintext, byte[] key)
    {
        using (var aes = new AesGcm(key))
        {
            
            var iv = new byte[AesGcm.NonceByteSizes.MaxSize];
            RandomNumberGenerator.Fill(iv);
            
            var tag = new byte[AesGcm.TagByteSizes.MaxSize];
            
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var ciphertext = new byte[plaintextBytes.Length];
           
            
          
            
            aes.Encrypt(iv,plaintextBytes,ciphertext,tag);
            
            writeDocument(ciphertext, iv, tag);
            readDocument();
        }
    }

    
    /**
     * Writes the encrypted text in a file
     */
    public void writeDocument(byte[] ciphertxt, byte[] iv, byte[] tag)
    {
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
        {
         outputFile.WriteLine(Convert.ToBase64String(iv));
         outputFile.WriteLine(Convert.ToBase64String(tag));
         outputFile.WriteLine(Convert.ToBase64String(ciphertxt));
        }
    }

    /**
     * writes the decrypted text in a file
     */
    public void writedcoded(byte[] plaintext)
    {
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Decode.txt"), true))
        {
            outputFile.WriteLine(Encoding.UTF8.GetString(plaintext));
        }
    }

    /**
     * Reads the text from the with the decrypted text
     */
    public void readdecoded()
    {
        try
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(docPath+"/Decode.txt"))
            { 
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        catch (IOException e)
        {
            throw new Exception("failed to read file", e);
        }
    }

    /**
     * reads the file with the encrypted text
     */
    public void readDocument()
    {
        try
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Open the text file using a stream reader.
            using (var sr = new StreamReader(docPath+"/WriteLines.txt"))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }
        catch (IOException e)
        {
            throw new Exception("failed to read file", e);
        }
    }


    /**
     * hashes the password that the user types in
     */
    public byte[] hashPw(string password, byte[] salt)
    {
      return   KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 600000,
            numBytesRequested: 256 / 8);
    }
}