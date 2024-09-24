namespace InformationSecurity.Cryptography.Gamma;

public readonly record struct NumbersGeneratorConfig(
    int ConstantA,  // A
    int ConstantC,  // С 
    int InitialT,   // T 0
    int WordLen     // B
);