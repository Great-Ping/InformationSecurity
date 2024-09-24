using InformationSecurity.Shared;
using InformationSecurity.Shared.Pagination;
using InformationSecurity.Widgets.Cryptography;
using InformationSecurity.Widgets.Pagination;

namespace InformationSecurity.Pages.Cryptography;

public class CryptographyPageModel(
        //CryptographyWidgetModel widget
    ) : ViewModelBase {
    public PaginationWidgetModel Pages { get; } = new PaginationWidgetModel();
    //public CryptographyWidgetModel Widget { get; } = widget;
}