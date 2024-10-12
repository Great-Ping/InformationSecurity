using System.Reflection;

namespace InformationSecurity.Cryptography.Gamma;



//Лаба 3.
public class GammaCryptographer 
    : BaseCryptographer<GammaCryptographerOptions>
{
    private readonly record struct ProcessResult(
        char[] Message,
        int LastNumber
    );
    
    private PseudorandomNumbersGenerator _generator;
    
    public GammaCryptographer()
        : base(GammaCryptographerOptions.Default)
    {
        _generator = new PseudorandomNumbersGenerator(Options.NumbersGeneratorOptions);
    }

    public override void UpdateOptions(GammaCryptographerOptions options)
    {
        _generator = new PseudorandomNumbersGenerator(options.NumbersGeneratorOptions);
        base.UpdateOptions(options);
    }


    public override char[] Encrypt(ReadOnlySpan<char> message)
    {
        ProcessResult result = Process(message, Options.InitialEncrypt);
        // int initialDecrypt = Options.InitialEncrypt;
        // int initialEncrypt = result.LastNumber;
        //
        // UpdateOptions(new GammaCryptographerOptions(
        //     Options.NumbersGeneratorOptions,
        //     initialEncrypt,
        //     initialDecrypt
        // ));
        
        return result.Message;
    }

    
    public override char[] Decrypt(ReadOnlySpan<char> message)
    {
        ProcessResult result = Process(message, Options.InitialDecrypt);
        return result.Message;
    }


    private ProcessResult Process(ReadOnlySpan<char> message, int initialNumber)
    {
        PseudorandomNumbersGenerator currentGenerator = _generator;
        IEnumerable<int> numbers = currentGenerator.Generate(message.Length, initialNumber).ToList();
        int lastNumber = initialNumber;
        
        char[] result = new char[message.Length];

        int i = 0;
        foreach (int number in numbers)
        {
            int newChar =  message[i] ^ number;
            result[i] = Convert.ToChar(newChar);
            
            lastNumber = number;
            i++;
        }
        
        return new ProcessResult(result, lastNumber);        
    }

    public override string ToString()
    {
        return $"GammaCryptographer {{ Options = {Options} }}";
    }
}