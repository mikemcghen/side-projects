using System;

class ShiftCipher
{
    static void Main()
    {
        string selection;
        do
        {
            Console.WriteLine("Would you like to:");
            Console.WriteLine("1) Enter a message to encrypt");
            Console.WriteLine("2) Enter a message to decrypt");
            Console.WriteLine("3) Exit");
            selection = Console.ReadLine();
            Console.WriteLine();
            if(selection == "1")
            {
                Console.Write("Enter message to encrypt: ");
                string message = Console.ReadLine();

                Console.Write("Enter shift value: ");
                int shift = int.Parse(Console.ReadLine());
                string encryptedMessage = Encrypt(message, shift);
                Console.WriteLine($"Encrypted message: {encryptedMessage}");
                Console.WriteLine();
            }
            else if (selection == "2")
            {
                Console.Write("Enter message to decrypt: ");
                string message = Console.ReadLine();

                Console.Write("Enter shift value: ");
                int shift = int.Parse(Console.ReadLine());
                string decryptedMessage = Decrypt(message, shift);
                Console.WriteLine($"Decrypted message: {decryptedMessage}");
                Console.WriteLine();
            }
            else if (selection == "3")
            {
                Console.WriteLine("Thank you, have a nice day!");
            }
            else
            {
                Console.WriteLine("Please select a valid input.");
            }
        }
        while (selection != "3");   
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
    static string Decrypt(string message, int shift)
    {
        string decrypted = "";
        foreach (char c in message)
        {
            if (char.IsLetter(c) && c >= 65 && c <= 90)
            {
                char shifted = (char)((char.ToUpper(c) - 65 - shift) % 26 + 65);
                decrypted += shifted;
            }
            else if (char.IsLetter(c))
            {
                char shifted = (char)((char.ToUpper(c) - 65 - shift) % 26 + 65);
                decrypted += char.ToLower(shifted);
            }
            else
            {
                decrypted += c;
            }
        }
        return decrypted;
    }
}
