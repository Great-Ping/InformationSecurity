using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using InformationSecurity.Cryptography;
using InformationSecurity.Shared;
using ReactiveUI;

namespace InformationSecurity.Widgets.Cryptography;

public class CryptographyWidgetModel<T>: ViewModelBase, ICryptographyWidgetModel
{
    private string _userInput;
    private string _optionsInput;
    private string? _exception;
    private bool _hasChanges;
    private readonly ICryptographer<T> _cryptographer;
    private readonly JsonSerializerOptions _serializerOptions;
    
    //_cryptographer = new SubstitutionCryptographer("01234", 2);
    //_cryptographer = new PermutationCryptographer([0, 5, 2, 3, 4, 1]);
        
    public CryptographyWidgetModel(ICryptographer<T> cryptographer) {
        _cryptographer = cryptographer;
        _userInput = String.Empty; 
        _hasChanges = false;
        _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,  
        };
        _optionsInput = JsonSerializer.Serialize(cryptographer.Options, _serializerOptions);
        
        InputWatermark= $"Введите сообщение\n";
        EncryptCommand = ReactiveCommand.Create(OnEncrypt); 
        DecryptCommand = ReactiveCommand.Create(OnDecrypt);

        _cryptographer.OptionsChanged += (T newOptions) =>
        {
            OptionsInput = JsonSerializer.Serialize(newOptions, _serializerOptions);
            _hasChanges = false;
        };
    }

    public string InputWatermark { get; }

    public string OptionsInput
    {
        get => _optionsInput;
        set
        {
            _hasChanges = true;
            this.RaiseAndSetIfChanged(ref _optionsInput, value);
        }
    }

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

    private void UpdateOptionsIfChanged()
    {
        if (!_hasChanges)
        {
            return;
        }

        try
        {
            T? options = JsonSerializer.Deserialize<T>(_optionsInput, _serializerOptions);

            if (options is null)
            {
                throw new ArgumentNullException("Options is null");
            }
            _cryptographer.UpdateOptions(options);
            _hasChanges = false;
        }
        catch (Exception e)
        {
            throw new InvalidDataException("Invalid options", e);
        }
    }

    private Task OnEncrypt()
    {
        try
        {
            UpdateOptionsIfChanged();
            
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
            UpdateOptionsIfChanged();
            
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