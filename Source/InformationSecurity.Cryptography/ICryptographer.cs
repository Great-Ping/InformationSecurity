namespace InformationSecurity.Cryptography;

public interface ICryptographer<T> : IConfigurable<T>
{
    event Action<T> OptionsChanged;
    
    char[] Encrypt(ReadOnlySpan<char> message);
    char[] Decrypt(ReadOnlySpan<char> encrypted);
}