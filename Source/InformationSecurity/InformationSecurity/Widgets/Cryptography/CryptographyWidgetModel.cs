using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InformationSecurity.Shared;
using ReactiveUI;

namespace InformationSecurity.Widgets.Cryptography;

public class CryptographyWidgetModel: ViewModelBase
{
    private string _userInput;
    
    public CryptographyWidgetModel() {
        HeaderText = "Let's encrypt";
        InputWatermark= "Введите сообщение";
        _userInput = String.Empty;
        
        EncryptCommand = ReactiveCommand.Create(OnEncrypt); 
        DecryptCommand = ReactiveCommand.Create(OnDecrypt);
    }

    public string HeaderText { get; } 
    public string InputWatermark { get; }

    public string UserInput
    {
        get => _userInput; 
        set => this.RaiseAndSetIfChanged(ref _userInput, value);
    }
    
    public ICommand EncryptCommand { get; }
    public ICommand DecryptCommand { get; }
    
    private Task OnEncrypt()
    {
        UserInput = "Encrypted";
        return Task.CompletedTask;
    }
    
    private Task OnDecrypt()
    {
        UserInput = "Decrypted";
        return Task.CompletedTask;
    }
}