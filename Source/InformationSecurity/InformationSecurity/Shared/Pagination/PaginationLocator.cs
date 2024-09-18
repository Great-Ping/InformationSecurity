using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace InformationSecurity.Shared.Pagination;

public class PaginationLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data == null)
            return null;
        //Activator.CreateInstance();
        return new Control();
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}