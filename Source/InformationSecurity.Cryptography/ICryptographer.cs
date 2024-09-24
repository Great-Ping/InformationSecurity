namespace InformationSecurity.Cryptography;

public interface ICryptographer
{
    Span<char> Encrypt(ReadOnlySpan<char> message);
    Span<char> Decrypt(ReadOnlySpan<char> encrypted);
}