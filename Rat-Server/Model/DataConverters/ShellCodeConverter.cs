using System;
using System.Text;

namespace Rat_Server.Model.DataConverter;

public class ShellCodeConverter
{
    public static byte[] ToShellCodeByteArray(string shellcode)
    {
        if (shellcode.Length % 2 != 0)
        {
            throw new ArgumentException("The shellcode string length must be even.");
        }

        int length = shellcode.Length / 2;
        byte[] byteArray = new byte[length];

        for (int i = 0; i < length; i++)
        {
            string byteValue = shellcode.Substring(i * 2, 2);
            byteArray[i] = Convert.ToByte(byteValue, 16);
        }

        return byteArray;
    }

    public static string ToShellCodeString(byte[] byteArray)
    {
        StringBuilder shellcodeString = new StringBuilder(byteArray.Length * 2);
        foreach (byte b in byteArray)
        {
            shellcodeString.Append(b.ToString("x2"));
        }
        return shellcodeString.ToString();
    }
}
