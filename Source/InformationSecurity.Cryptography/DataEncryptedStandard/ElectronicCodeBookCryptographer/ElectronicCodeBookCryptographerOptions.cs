using System.Text.Json.Serialization;

namespace InformationSecurity.Cryptography.DataEncryptedStandard.ElectronicCodeBookCryptographer;

public class ElectronicCodeBookCryptographerOptions
{

    [JsonConstructor]
    public ElectronicCodeBookCryptographerOptions(byte[] key)
    {
        Key = key;
        DataEncryptedStandard = DataEncryptedStandardOptions.Default;
        FeistelKeys = DesHelper.PrepareFeistelKeys(key, DataEncryptedStandard);
    }

    public static ElectronicCodeBookCryptographerOptions Default = new(
        // [0b01, 0b01, 0b01, 0b01, 0b01, 0b01, 0b01, 0b01]
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

    [JsonIgnore]
    public IEnumerable<byte[]> FeistelKeys;
    [JsonIgnore]
    public DataEncryptedStandardOptions DataEncryptedStandard { get; }
}