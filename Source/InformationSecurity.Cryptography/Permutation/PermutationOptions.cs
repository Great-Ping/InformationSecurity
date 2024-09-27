namespace InformationSecurity.Cryptography.Permutation;

public record PermutationOptions(
    int[] Permutations,
    int[] Reverse
)
{
    public static PermutationOptions Default { get; }

    static PermutationOptions()
    {
        int[] permutations = [1,2,3,4,0];
        int[] reversedPermutations = PermutationHelper.GenerateReversedPermutations(permutations);

        Default = new PermutationOptions(
            permutations,
            reversedPermutations
        );
    }
};