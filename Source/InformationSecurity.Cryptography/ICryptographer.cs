namespace InformationSecurity.Cryptography;

public interface ICryptographer<T> : IConfigurable<T>
{
    char[] Encrypt(ReadOnlySpan<char> message);
    char[] Decrypt(ReadOnlySpan<char> encrypted);
}