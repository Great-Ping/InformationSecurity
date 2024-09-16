namespace InformationSecurity.Cryptography;

public interface ICrypto
{
    public abstract void Encrypt(string message);
    public abstract void Decrypt(string encryptedMessage);
}