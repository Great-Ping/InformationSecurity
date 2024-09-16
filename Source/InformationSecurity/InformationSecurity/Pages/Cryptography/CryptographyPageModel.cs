using InformationSecurity.Shared;
using InformationSecurity.Widgets.Cryptography;

namespace InformationSecurity.Pages.Cryptography;

public class CryptographyPageModel(
        CryptographyWidgetModel widget
    ) : ViewModelBase {
    public CryptographyWidgetModel Widget { get; } = widget;
}