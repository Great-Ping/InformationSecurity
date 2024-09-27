namespace InformationSecurity.Cryptography;

public record SubstitutionOptions(
    string Alphabet,
    int BlockSize,
    string[] Chipers,
    string[] Deciphers
)
{
    public static SubstitutionOptions Default { get; }

    static SubstitutionOptions()
    {
        string alphabet = "01234";
        int blockSize = 2;
        string[] chipers = SubstitutionHelper.GenerateCiphers(alphabet, blockSize);
        string[] deciphers = SubstitutionHelper.GenerateDeciphers(alphabet, blockSize, chipers);

        Default = new SubstitutionOptions(
            alphabet,
            blockSize,
            chipers,
            deciphers
        );
    }
};