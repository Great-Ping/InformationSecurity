using System.Security.Cryptography;

namespace InformationSecurity.Cryptography.Gamma;

public class PseudorandomNumbersGenerator(    
    NumbersGeneratorOptions options
) { 
    public NumbersGeneratorOptions Options { get; } = options;
    
    //
    // private int[]? _randomNumbers = null;
    // public ReadOnlySpan<int> Genereate(int messageLength)
    // {
    //     if (_randomNumbers != null && _randomNumbers.Length >= messageLength)
    //     {
    //         return _randomNumbers.AsSpan(0, messageLength);
    //     }
    //
    //     int[] randomNumbers = new int[messageLength];
    //     randomNumbers[0] = Options.InitialT;
    //
    //     for (int i = 1; i < messageLength; i++)
    //     {
    //         randomNumbers[i] =
    //             (Options.ConstantA * randomNumbers[i - 1] + Options.ConstantC) % Options.WordLen;
    //     }   
    //     
    //     _randomNumbers = randomNumbers;
    //     return randomNumbers.AsSpan(0, messageLength);
    // }


    private int CalculateNextNumber(int previousNumber)
    {
        return  (Options.ConstantA * previousNumber + Options.ConstantC) % Options.WordLen;
    }

    public IEnumerable<int> Generate(int messageLength, int initialNumber)
    {
        int previousNumber = initialNumber;
        
        for (int i = messageLength; i > 0; i--)
        {
            int nextNumber = CalculateNextNumber(previousNumber);
            previousNumber = nextNumber;
            yield return nextNumber;
        }
    }
}