namespace InformationSecurity.Cryptography.Permutation;


//Лаба 2, перестановками
public class PermutationCryptographer 
    : ICryptographer<PermutationOptions>
{
    
    private readonly char _separator;
    public PermutationCryptographer(char separator = ' ')
    {
        Options = PermutationOptions.Default;
        _separator = separator;
    }

    public PermutationOptions Options { get; private set; }
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


    private char[] Process(ReadOnlySpan<char> message, int[] permutations)
    {
        if (permutations.Length == 0)
            return Array.Empty<char>();
        
        int blockSize = permutations.Length;
        
        int wholeBlocksCount = message.Length / blockSize;
        int remainder = message.Length % blockSize;

        char[] result = (remainder > 0)
            ? new char[(wholeBlocksCount + 1) * blockSize] 
            : new char[message.Length];

        Span<char> newBlock;
        
        for (int i = 0; i < wholeBlocksCount; i++)
        {
            int start = i * blockSize;
            newBlock = new(result, start, blockSize);
            ReadOnlySpan<char> originalBlock = message.Slice(start, blockSize);

            PermutationHelper.ShuffleBlock(
                originalBlock,
                newBlock,
                permutations
            );
        }

        if (remainder <= 0)
            return result;
        
        int remainderStart = message.Length - remainder;
        ReadOnlySpan<char> remainingBlock = message.Slice(remainderStart, remainder);
        newBlock = new(result, remainderStart, blockSize);
        
        PermutationHelper.Fill(newBlock, _separator);
        
        PermutationHelper.ShuffleBlock(
            remainingBlock,
            newBlock,
            permutations
        );
        
        return result;
    }
    public override string ToString()
    {
        return $"PermutationCryptographer {{ Options = {Options} }}";
    }

}