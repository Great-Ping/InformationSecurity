using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography;

public class SubstitutionOptions
{
    
    
    public SubstitutionOptions(string alphabet, int blockSize)
        : this(
            alphabet,
            blockSize,
            SubstitutionHelper.GenerateCiphers(alphabet, blockSize)
        )
    { }

    [JsonConstructor]
    public SubstitutionOptions(string alphabet, int blockSize, string[] chipers)
    {
        Alphabet = alphabet;
        BlockSize = blockSize;
        Chipers = chipers;
        Deciphers = SubstitutionHelper.GenerateDeciphers(alphabet, blockSize, chipers);
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