namespace InformationSecurity.Cryptography.Gamma;

public record NumbersGeneratorOptions(
    int ConstantA,  // A
    int ConstantC,  // С 
    int InitialT,   // T 0
    int WordLen     // B - Max Char
)
{
    public static NumbersGeneratorOptions Default { get; }

    static NumbersGeneratorOptions()
    {
        Default = new NumbersGeneratorOptions(
            13,
            43,
            37,
            Char.MaxValue
        );
    }
}