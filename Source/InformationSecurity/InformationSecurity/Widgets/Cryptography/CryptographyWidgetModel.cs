using System;
using System.Threading.Tasks;
using System.Windows.Input;
using InformationSecurity.Cryptography;
using InformationSecurity.Shared;
using ReactiveUI;

namespace InformationSecurity.Widgets.Cryptography;

public class CryptographyWidgetModel: ViewModelBase
{
    private string _userInput;
    private ICryptographer _cryptographer;
    
    public CryptographyWidgetModel() {
        HeaderText = "Let's encrypt";
        InputWatermark= "Введите сообщение\nV={0,1,2,3,4}\nm=2 |V|=5 |V|^m=25";
        
        _userInput = String.Empty;  
        _cryptographer = new SubstitutionCryptographer("01234", 2);
            
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
        ReadOnlySpan<char> encrypted = _cryptographer.Encrypt(UserInput);
        UserInput = new string(encrypted);
        return Task.CompletedTask;
    }
    
    private Task OnDecrypt()
    {
        ReadOnlySpan<char> encrypted = _cryptographer.Decrypt(UserInput);
        UserInput = new string(encrypted);
        return Task.CompletedTask;
    }
}