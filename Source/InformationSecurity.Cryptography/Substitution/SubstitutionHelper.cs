using System.Buffers;

namespace InformationSecurity.Cryptography;

public static class SubstitutionHelper
{    
    /// <summary>
    /// Задумка в информатике 8 класс
    /// </summary>
    public static string[] GenerateDeciphers(ReadOnlySpan<char> alphabet, int blockSize, string[] ciphers)
    {
        int ciphersCount = PowInt(alphabet.Length, blockSize);
        string[] deciphers = new string[ciphersCount];

        int i = 0;
        foreach (string cipher in ciphers)
        {
            int cipherValue = FromAlphabet(alphabet, blockSize, cipher.AsSpan());
            string decipher = ToAlphabet(alphabet, blockSize, i);
            deciphers[cipherValue] = decipher;
            i++;
        }

        return deciphers;
    }
    public static string[] GenerateCiphers(ReadOnlySpan<char> alphabet, int blockSize)
    {   
        int ciphersCount = PowInt(alphabet.Length, blockSize);
        string[] ciphers = new string[ciphersCount]; 
        
        for (int i = 0; i < ciphersCount; i++)
        {
            ciphers[i] = ToAlphabet(alphabet, blockSize, i);
        }
        
        Random.Shared.Shuffle(ciphers);
        return ciphers;
    }

    public static int FromAlphabet(ReadOnlySpan<char> alphabet, int blockSize, ReadOnlySpan<char> block)
    {
        int result = 0;
        for (int i = 0; i < blockSize; i++)
        {
            int index = alphabet.IndexOf(block[i]);
            if (index == -1)
            {
                throw new ArgumentException($"Symbol '{block[i]}' does not belong to alphabet");
            }
            
            result += index * PowInt(alphabet.Length,  i);
        }
        return result;
    }

    public static string ToAlphabet(ReadOnlySpan<char> alphabet, int blockSize, int blockValue)
    {
        char[] pool = ArrayPool<char>.Shared.Rent(blockSize);
        Span<char> convertedBlock = pool.AsSpan(0, blockSize);
        try
        {
            for (int i = 0 ; i < blockSize; i++)
            {
                int charIndex = blockValue % alphabet.Length;
                convertedBlock[i] = alphabet[charIndex];
                
                blockValue /= alphabet.Length;
            }

            if (blockValue > 0)
            {
                throw new ArgumentException("Invalid block value");
            }
            
            return new string(convertedBlock);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(pool);
        }
    }


    private static int PowInt(int value, int exponent)
    {
        int result = 1;
        for (int i = 0; i < exponent; i++) {
            result *= value;
        }
        return result;
    }

}