using System.Reflection;

namespace InformationSecurity.Cryptography.Gamma;


//Лаба 3.
public class GammaCryptographer
    : ICryptographer, IConfigurable<NumbersGeneratorOptions>
{
    private PseudorandomNumbersGenerator? _generator = null;

    public NumbersGeneratorOptions Options { get; private set; } = NumbersGeneratorOptions.Default;
    public void UpdateOptions(NumbersGeneratorOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        Options = options;
    }

    public char[] Encrypt(ReadOnlySpan<char> message)
    {
        PseudorandomNumbersGenerator currentGenerator = SelectGenerator();
        ReadOnlySpan<int> numbers = currentGenerator.Genereate(message.Length);
        
        char[] result = new char[message.Length];

        for (int i = 0; i < message.Length; i++)
        {
            int newChar = (message[i] + numbers[i]) % Options.WordLen;

            result[i] = Convert.ToChar(newChar);
        }

        return result;
    }

    public char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        PseudorandomNumbersGenerator currentGenerator = SelectGenerator();
        ReadOnlySpan<int> numbers = currentGenerator.Genereate(encrypted.Length);
        
        char[] result = new char[encrypted.Length];

        for (int i = 0; i < encrypted.Length; i++)
        {
            int newChar = (encrypted[i] - numbers[i]) % Options.WordLen;

            if (newChar < 0)
            {
                newChar = newChar + Options.WordLen;
            }
          
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