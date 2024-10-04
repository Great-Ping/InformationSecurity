using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography.Permutation;

public class PermutationOptions(int[] permutations)
{
    public static PermutationOptions Default { get; }
    public int[] Permutations { get; } = permutations;

    [JsonIgnore]
    public int[] Reverse { get; } = PermutationHelper.GenerateReversedPermutations(permutations);

    [JsonIgnore] public char[] SpaceBuffer { get; } = new char[permutations.Length];

    static PermutationOptions()
    {
        int[] permutations = Enumerable.Range(0, 5).ToArray();
        Random.Shared.Shuffle(permutations);
        
        Default = new PermutationOptions(
            permutations
        );
    }
};