using System.Security.Cryptography;
using System.Text;
using Cryptography;

public class CryptographyConsoleApp
{

    public static void Main()
    {
        var ase = new Ase();
        string pass = "";
        byte[] salt = new byte[] { };
        Console.WriteLine("passcode");
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);
        Console.WriteLine("\n"+"-----------");
        
        ase.aseEncrypt(ase.hashPw(pass, salt));
    }
}