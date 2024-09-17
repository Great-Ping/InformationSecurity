using System.Collections.Immutable;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace InformationSecurity.Cryptography;


/// <summary>
/// Лаба 1 V = {0,1,2,3,4} m = 2
/// </summary>
public class SubstitutionCryptographer: ICryptographer
{
    private readonly string[] _ciphers;
    private readonly string[] _deciphers;
    private readonly string _alphabet;
    private readonly int _blockSize;
    
    public SubstitutionCryptographer(string alphabet, int blockSize)
    {
        _alphabet = alphabet;
        _blockSize = blockSize;
        _ciphers = SubstitutionHelper.GenerateCiphers(alphabet.AsSpan(), blockSize);
        _deciphers = SubstitutionHelper.GenerateDeciphers(alphabet.AsSpan(), blockSize, _ciphers);
    }

    public ReadOnlySpan<char> Encrypt(ReadOnlySpan<char> message) 
    {
        return Process(message, _ciphers);
    }
   

    public ReadOnlySpan<char> Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(encrypted, _deciphers);
    }

    private ReadOnlySpan<char> Process(ReadOnlySpan<char> message, string[] substitution)
    {
        if (message.Length % _blockSize != 0)
        {
            throw new ArgumentException("Invalid message length");
        }

        char[] encryptedMessage = new char[message.Length];

        int blocksCount = message.Length / _blockSize;
        int messageIndex = 0;

        for (int i = 0; i < blocksCount; i++)
        {
            ReadOnlySpan<char> block = message.Slice(messageIndex, _blockSize);
            int blockValue = SubstitutionHelper.FromAlphabet(
                _alphabet,
                _blockSize,
                block
            );
            string encryptedBlock = substitution[blockValue];
            encryptedBlock.CopyTo(
                0,
                encryptedMessage,
                messageIndex,
                encryptedBlock.Length
            );

            messageIndex += block.Length;
        }


        return encryptedMessage.AsSpan();
    }

    public override string ToString()
    {
        return "SubstitutionCryptographer( V = {0,1,2,3,4} m = 2)";
    }
}