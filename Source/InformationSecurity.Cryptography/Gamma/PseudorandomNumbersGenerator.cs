using System.Security.Cryptography;

namespace InformationSecurity.Cryptography.Gamma;

public class PseudorandomNumbersGenerator(    
    NumbersGeneratorConfig config
) { 
    private int[]? _randomNumbers = null;
    public NumbersGeneratorConfig Config { get; } = config;

    public ReadOnlySpan<int> Genereate(int messageLength)
    {
        if (_randomNumbers != null && _randomNumbers.Length >= messageLength)
        {
            return _randomNumbers.AsSpan(0, messageLength);
        }

        int[] randomNumbers = new int[messageLength];
        randomNumbers[0] = Config.InitialT;

        for (int i = 1; i < messageLength; i++)
        {
            randomNumbers[i] =
                (Config.ConstantA * randomNumbers[i - 1] + Config.ConstantC) % Config.WordLen;
        }   
        
        _randomNumbers = randomNumbers;
        return randomNumbers.AsSpan(0, messageLength);
    }
}