using System.Reflection;

namespace InformationSecurity.Cryptography.Gamma;

public class GammaCryptographer(
    NumbersGeneratorConfig config
) : ICryptographer
{
    private readonly NumbersGeneratorConfig _config = config;
    private PseudorandomNumbersGenerator? _generator = null;
    
    public char[] Encrypt(ReadOnlySpan<char> message)
    {
        PseudorandomNumbersGenerator currentGenerator = SelectGenerator();
        ReadOnlySpan<int> numbers = currentGenerator.Genereate(message.Length);
        
        char[] result = new char[message.Length];

        for (int i = 0; i < message.Length; i++)
        {
            int newChar = message[i] - numbers[i];

            if (newChar < 0)
            {
                newChar = newChar + _config.WordLen;
            }

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
            int newChar = (encrypted[i] + numbers[i]) % config.WordLen;
            result[i] = Convert.ToChar(newChar);
        }

        return result;
    }

    private PseudorandomNumbersGenerator SelectGenerator()
    {
        if (_generator != null && _generator.Config == _config)
        {
            return _generator;
        }
        
        return new PseudorandomNumbersGenerator(_config);
    }

    public override string ToString()
    {
        return $"GammaCryptographer(config:{_config})";
    }
}