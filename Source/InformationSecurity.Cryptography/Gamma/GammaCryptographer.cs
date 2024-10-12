using System.Reflection;

namespace InformationSecurity.Cryptography.Gamma;


//Лаба 3.
public class GammaCryptographer 
    : BaseCryptographer<NumbersGeneratorOptions>
{
    private PseudorandomNumbersGenerator? _generator = null;

    public GammaCryptographer()
        : base(NumbersGeneratorOptions.Default)
    {
    }

    public override char[] Encrypt(ReadOnlySpan<char> message)
    {
        return Process(message);
    }

    public override char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        return Process(encrypted);
    }


    private char[] Process(ReadOnlySpan<char> message)
    {
        PseudorandomNumbersGenerator currentGenerator = SelectGenerator();
        ReadOnlySpan<int> numbers = currentGenerator.Genereate(message.Length);
        
        char[] result = new char[message.Length];

        for (int i = 0; i < message.Length; i++)
        {
            int newChar =  message[i] ^ numbers[i];
            result[i] = Convert.ToChar(newChar);
        }
        
        return result;        
    }

    private PseudorandomNumbersGenerator SelectGenerator()
    {
        if (_generator != null && _generator.Options == Options)
        {
            return _generator;
        }
        
        return new PseudorandomNumbersGenerator(Options);
    }

    public override string ToString()
    {
        return $"GammaCryptographer {{ Options = {Options} }}";
    }
}