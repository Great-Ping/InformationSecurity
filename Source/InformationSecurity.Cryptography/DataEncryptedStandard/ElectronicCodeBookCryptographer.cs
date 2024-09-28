namespace InformationSecurity.Cryptography.DataEncryptedStandard;

public class ElectronicCodeBookCryptographer: ICryptographer<object>
{
    public object Options { get; }
    public void UpdateOptions(object options)
    {
        throw new NotImplementedException();
    }
    public char[] Encrypt(ReadOnlySpan<char> message)
    {
        throw new NotImplementedException();
    }

    public char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        throw new NotImplementedException();
    }

}