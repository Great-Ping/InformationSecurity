namespace InformationSecurity.Cryptography;

public interface IConfigurable<T>
{
    T Options { get; }
    void UpdateOptions(T options);
}