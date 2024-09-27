namespace InformationSecurity.Cryptography.Permutation;


//Лаба 2, перестановками
public class PermutationCryptographer 
    : ICryptographer, IConfigurable<PermutationOptions>
{
    public PermutationOptions Options { get; private set; } = PermutationOptions.Default;
    public void UpdateOptions(PermutationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        Options = options;
    }
    public char[] Encrypt(ReadOnlySpan<char> message)
    {
        return Process(message, Options.Permutations);
    }

    public char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(encrypted, Options.Reverse);
    }


    private char[] Process(ReadOnlySpan<char>  message, int[] permutations)
    {
        int blockSize = permutations.Length;

        if (message.Length % blockSize != 0)
        {
            throw new ArgumentException("Invalid message length.");
        }

        char[] result = new char[message.Length];
        int blocksCount = message.Length / blockSize;

        for (int i = 0; i < blocksCount; i++)
        {
            Span<char> newBlock = new(result, i * blockSize, blockSize);
            ReadOnlySpan<char> originalBlock = message.Slice(i * blockSize, blockSize);

            for (int j = 0; j < blockSize; j++)
            {
                int newIndex = permutations[j];
                newBlock[j] = originalBlock[newIndex];
            }
        }

        return result;
    }

    public override string ToString()
    {
        return $"PermutationCryptographer {{ Options = {Options} }}";
    }

}