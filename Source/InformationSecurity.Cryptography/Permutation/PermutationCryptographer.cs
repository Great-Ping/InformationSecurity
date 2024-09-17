namespace InformationSecurity.Cryptography.Permutation;


//Лаба 2, перестановками
public class PermutationCryptographer : ICryptographer
{
    private readonly int[] _permutations;
    private readonly int[] _reversedPermutations;

    public PermutationCryptographer(int[] permutations)
    {
        _permutations = permutations;
        _reversedPermutations = PermutationHelper.GenerateReversedPermutations(_permutations);
    }

    public ReadOnlySpan<char> Encrypt(ReadOnlySpan<char> message)
    {
        return Process(message, _permutations);
    }

    public ReadOnlySpan<char> Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(encrypted, _reversedPermutations);
    }


    private ReadOnlySpan<char> Process(ReadOnlySpan<char> message, int[] permutations)
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
                int newIndex = permutations[i * blockSize + j];
                newBlock[j] = originalBlock[newIndex];
            }
        }

        return result.AsSpan();
    }

    public override string ToString()
    {
        return $"PermutationCryptographer(permutations: [{String.Join(',', _permutations)}])";
    }
}