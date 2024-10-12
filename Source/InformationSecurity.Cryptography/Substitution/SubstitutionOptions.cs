using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography;

public class SubstitutionOptions
{
    
    [JsonConstructor]
    public SubstitutionOptions(string alphabet, int blockSize, string[]? chipers = null)
    {
        Alphabet = alphabet;
        BlockSize = blockSize;
        Chipers = chipers ?? SubstitutionHelper.GenerateCiphers(alphabet, blockSize);
        Deciphers = SubstitutionHelper.GenerateDeciphers(alphabet, blockSize, Chipers);
    }

    public string Alphabet { get;  }
    public int BlockSize { get; }
    public string[] Chipers { get; }
    [JsonIgnore]
    public string[] Deciphers { get; }
    public static SubstitutionOptions Default { get; }

    static SubstitutionOptions()
    {
        string alphabet = "01234";
        int blockSize = 2;

        Default = new SubstitutionOptions(
            alphabet,
            blockSize
        );
    }
};