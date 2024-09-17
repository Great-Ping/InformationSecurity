namespace InformationSecurity.Cryptography;

public interface ICryptographer
{
    ReadOnlySpan<char> Encrypt(ReadOnlySpan<char> message);
    ReadOnlySpan<char> Decrypt(ReadOnlySpan<char> encrypted);
}