using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography.Gamma;

public class GammaCryptographerOptions
{
    public GammaCryptographerOptions(
        NumbersGeneratorOptions numbersGeneratorOptions
    ) : this(
        numbersGeneratorOptions,
        numbersGeneratorOptions.InitialT,
        numbersGeneratorOptions.InitialT
    ) { }

    [JsonConstructor]
    public GammaCryptographerOptions(
        NumbersGeneratorOptions numbersGeneratorOptions,
        int initialEncrypt, 
        int initialDecrypt
    ) {
        NumbersGeneratorOptions = numbersGeneratorOptions;
        InitialEncrypt = initialEncrypt;
        InitialDecrypt = initialDecrypt;
    }

    public static GammaCryptographerOptions Default => new GammaCryptographerOptions(
        NumbersGeneratorOptions.Default
    );

    public NumbersGeneratorOptions NumbersGeneratorOptions { get; }
    public int InitialEncrypt { get; }
    public int InitialDecrypt { get; }
}