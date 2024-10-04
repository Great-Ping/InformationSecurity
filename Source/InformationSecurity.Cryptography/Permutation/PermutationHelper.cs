namespace InformationSecurity.Cryptography.Permutation;

public static class PermutationHelper
{
    public static int[] GenerateReversedPermutations(int[] permutations)
    {
        int[] reversedPermutations = new int[permutations.Length];
        
        for (int i = 0; i < permutations.Length; i++)
        {
            int permutation = permutations[i];
            reversedPermutations[permutation] = i;
        }

        return reversedPermutations;
    }


    public static void ShuffleBlock(ReadOnlySpan<char> originalBlock, Span<char> newBlock, int[] permutations)
    {
        for (int j = 0; j < originalBlock.Length; j++)
        {
            int newPosition = permutations[j];
            newBlock[newPosition] = originalBlock[j];
        }
    }

    public static void Fill(Span<char> block, char symbol)
    {
        for (int i = 0; i < block.Length; i++)
        {
            block[i] = symbol;
        }
    }
}