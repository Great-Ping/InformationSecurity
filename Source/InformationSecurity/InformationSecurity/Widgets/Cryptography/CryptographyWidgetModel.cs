using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using InformationSecurity.Cryptography;
using InformationSecurity.Cryptography.Permutation;
using InformationSecurity.Shared;
using ReactiveUI;

namespace InformationSecurity.Widgets.Cryptography;

public class CryptographyWidgetModel: ViewModelBase
{
    private string _userInput;
    private string? _exception;
    private readonly ICryptographer _cryptographer;
    
    //_cryptographer = new SubstitutionCryptographer("01234", 2);
    //_cryptographer = new PermutationCryptographer([0, 5, 2, 3, 4, 1]);
        
    public CryptographyWidgetModel(ICryptographer cryptographer) {
        _cryptographer = cryptographer;
        _userInput = String.Empty;  
        
        InputWatermark= $"Введите сообщение\n{_cryptographer}";
        
            
        EncryptCommand = ReactiveCommand.Create(OnEncrypt); 
        DecryptCommand = ReactiveCommand.Create(OnDecrypt);
    }

    public string InputWatermark { get; }
    public string? Exception
    {
        get => _exception;
        set => this.RaiseAndSetIfChanged(ref _exception, value);
    }

    public string UserInput
    {
        get => _userInput;
        set => this.RaiseAndSetIfChanged(ref _userInput, value);
    }

    public ICommand EncryptCommand { get; }
    public ICommand DecryptCommand { get; }
    
    private Task OnEncrypt()
    {
        try
        {
            ReadOnlySpan<char> encrypted = _cryptographer.Encrypt(UserInput);
            UserInput = new string(encrypted);
            Exception = null;
        }
        catch (Exception ex)
        {
            Exception = ex.Message;   
        }
        return Task.CompletedTask;
    }
    
    private Task OnDecrypt()
    {
        try
        {
            ReadOnlySpan<char> encrypted = _cryptographer.Decrypt(UserInput);
            UserInput = new string(encrypted);
            Exception = null;
        }
        catch (Exception ex)
        {
            Exception = ex.Message;
        }
        return Task.CompletedTask;
    }
}