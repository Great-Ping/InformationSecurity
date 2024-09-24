using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using InformationSecurity.Cryptography;
using InformationSecurity.Cryptography.Permutation;
using InformationSecurity.Shared;
using InformationSecurity.Widgets.Cryptography;
using ReactiveUI;

namespace InformationSecurity.Widgets.Pagination;

public class PaginationWidgetModel : ViewModelBase
{
    private int _pageIndex;
    
    public PaginationWidgetModel()
    {   
        PageIndex = 0;
        Pages = [
            new CryptographyWidgetModel( 
                new SubstitutionCryptographer("01234", 2) 
            ), 
            new CryptographyWidgetModel( 
                new PermutationCryptographer([0, 5, 4, 3, 2, 1])
            )
        ];
        PageIndexes = new ObservableCollection<int>(Enumerable.Range(1, Pages.Length));
        
        IncrementPageCommand = ReactiveCommand.Create(OnIncrementPage);
        DecrementPageCommand = ReactiveCommand.Create(OnDecrementPage);
        SelectPageCommand = ReactiveCommand.Create<int>(OnSelectPage);
        
        
        this.WhenAnyValue(x => x.PageIndex)
            .Subscribe(
                Observer.Create((int t) =>
                {
                    this.RaisePropertyChanged(nameof(SelectedPage));
                }
            )
        );
    }
    
    public ViewModelBase[] Pages { get; set; }
    public ViewModelBase? SelectedPage => Pages.ElementAtOrDefault(PageIndex);
    public ObservableCollection<int> PageIndexes { get; set; }
    
    public int PageIndex
    {
        get => _pageIndex;
        set => this.RaiseAndSetIfChanged(ref _pageIndex, value);
    }
    
    public ICommand IncrementPageCommand { get; }
    public ICommand DecrementPageCommand { get; }
    public ICommand SelectPageCommand { get; }


    public void OnSelectPage(int pageIndex)
    {
        PageIndex = pageIndex - 1;
    }

    public void OnIncrementPage()
    {
        PageIndex = (PageIndex + 1) % Pages.Length;
    }
    
    
    public void OnDecrementPage()
    {
        int currentIndex = PageIndex;
        
        if (currentIndex < 0)
            currentIndex = Pages.Length;
        
        PageIndex = (currentIndex - 1) % Pages.Length;
    }
    
    
}