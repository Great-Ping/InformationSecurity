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
}