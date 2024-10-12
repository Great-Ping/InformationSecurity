namespace InformationSecurity.Cryptography;

public abstract class BaseCryptographer<T> : ICryptographer<T>
{
    public event Action<T>? OptionsChanged;

    public BaseCryptographer(T options)
    {
        Options = options;
    }

    public T Options { get; private set; }
    
    public virtual void UpdateOptions(T options)
    {
        Options = options;
        OptionsChanged?.Invoke(Options);
    }

    public abstract char[] Encrypt(ReadOnlySpan<char> message);
    public abstract char[] Decrypt(ReadOnlySpan<char> encrypted);
}