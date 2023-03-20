using System;

class ShiftCipher
{
    static void Main()
    {
        Console.Write("Enter message to encrypt: ");
        string message = Console.ReadLine();

        Console.Write("Enter shift value: ");
        int shift = int.Parse(Console.ReadLine());

        string encryptedMessage = Encrypt(message, shift);

        Console.WriteLine($"Encrypted message: {encryptedMessage}");
    }

    static string Encrypt(string message, int shift)
    {
        string encrypted = "";
        foreach (char c in message)
        {
            if (char.IsLetter(c) && c >= 65 && c <= 90)
            {
                char shifted = (char)((char.ToUpper(c) - 65 + shift) % 26 + 65);
                encrypted += shifted;
            }
            else if(char.IsLetter(c))
            {
                char shifted = (char)((char.ToUpper(c) - 65 + shift) % 26 + 65);
                encrypted += char.ToLower(shifted);
            }
            else
            {
                encrypted += c;
            }
        }
        return encrypted;
    }
}
