namespace InformationSecurity.Cryptography.DataEncryptedStandard;

public class ElectronicCodeBookCryptographer
    : BaseCryptographer<object>
{

    public ElectronicCodeBookCryptographer()
        : base(new object())
    {
    }

    public override char[] Encrypt(ReadOnlySpan<char> message)
    {
        throw new NotImplementedException();
    }

    public override char[] Decrypt(ReadOnlySpan<char> encrypted)
    {
        throw new NotImplementedException();
    }

}