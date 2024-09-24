namespace InformationSecurity.Cryptography;

public interface ICryptographer
{
    char[] Encrypt(ReadOnlySpan<char> message);
    char[] Decrypt(ReadOnlySpan<char> encrypted);
}