using InformationSecurity.Pages.Cryptography;

namespace InformationSecurity.Application;

public class MainViewModel(CryptographyPageModel cryptographyPageModel)
{
    public CryptographyPageModel CryptographyPage { get; }
        = cryptographyPageModel;
}