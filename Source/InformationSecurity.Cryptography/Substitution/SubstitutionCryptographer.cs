using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace InformationSecurity.Cryptography;

/// <summary>
/// Лаба 1 V = {0,1,2,3,4} m = 2
/// </summary>
public class SubstitutionCryptographer 
    : ICryptographer, IConfigurable<SubstitutionOptions>
{
    public SubstitutionOptions Options { get; private set; } = SubstitutionOptions.Default;
    public void UpdateOptions(SubstitutionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        Options = options;
    }

    public char[] Encrypt(ReadOnlySpan<char> message)
    {
        return Process(message, Options.Alphabet, Options.BlockSize, Options.Chipers);
    }


    public char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(encrypted, Options.Alphabet, Options.BlockSize, Options.Deciphers);
    }

    private static char[] Process(ReadOnlySpan<char> message, string alphabet, int blockSize, string[] substitution)
    {
        if (message.Length % blockSize != 0)
        {
            throw new ArgumentException("Invalid message length");
        }

        char[] encryptedMessage = new char[message.Length];

        int blocksCount = message.Length / blockSize;
        int messageIndex = 0;

        for (int i = 0; i < blocksCount; i++)
        {
            ReadOnlySpan<char> block = message.Slice(messageIndex, blockSize);
            int blockValue = SubstitutionHelper.FromAlphabet(
                alphabet,
                blockSize,
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


        return encryptedMessage;
    }

    public override string ToString()
    {
        return $"SubstitutionCryptographer {{ Options = {Options} }}";
    }

    public void UpdateConfig(SubstitutionCryptographer config)
    {
        throw new NotImplementedException();
    }
}