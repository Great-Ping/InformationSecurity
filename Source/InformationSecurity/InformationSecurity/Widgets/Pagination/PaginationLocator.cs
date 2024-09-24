using Avalonia.Controls;
using Avalonia.Controls.Templates;
using InformationSecurity.Shared;
using InformationSecurity.Widgets.Cryptography;

namespace InformationSecurity.Widgets.Pagination;

public class PaginationLocator : IDataTemplate
{

    public Control? Build(object? data)
    {   
        return new CryptographyWidget();
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}