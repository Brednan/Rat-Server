using System;

namespace Rat_Server.Model.DataConverter;

public class ShellCodeConverter
{
    public static byte[] ToByteArray(string shellcode)
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
}
