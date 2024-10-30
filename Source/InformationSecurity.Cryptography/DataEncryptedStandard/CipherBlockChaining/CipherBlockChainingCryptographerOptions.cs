using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography.DataEncryptedStandard.CipherBlockChaining;

public class CipherBlockChainingCryptographerOptions
{

    [JsonConstructor]
    public CipherBlockChainingCryptographerOptions(byte[] key, byte[] initialVector)
    {
        Key = key;
        InitialVector = initialVector;

        DataEncryptedStandard = DataEncryptedStandardOptions.Default;
        FeistelKeys = DesHelper.PrepareFeistelKeys(key, DataEncryptedStandard);
    }

    public static CipherBlockChainingCryptographerOptions Default = new (
        [
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101
        ],
        [
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101,
            0b10101011,
            0b11001101
        ]
    );

    public byte[] Key { get; }
    public byte[] InitialVector { get; }

    [JsonIgnore]
    public IEnumerable<byte[]> FeistelKeys;
    [JsonIgnore]
    public DataEncryptedStandardOptions DataEncryptedStandard { get; }
}