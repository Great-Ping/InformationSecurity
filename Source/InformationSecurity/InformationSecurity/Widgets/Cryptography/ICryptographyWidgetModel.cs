using System.Windows.Input;

namespace InformationSecurity.Widgets.Cryptography;

public interface ICryptographyWidgetModel
{
    string InputWatermark { get; }
    string OptionsInput { get; set; }
    string? Exception { get; }
    string UserInput { get; set; }
    ICommand EncryptCommand { get; }
    ICommand DecryptCommand { get; }
}