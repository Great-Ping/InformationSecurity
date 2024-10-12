using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection.Metadata;
using System.Windows.Input;
using InformationSecurity.Cryptography;
using InformationSecurity.Cryptography.Gamma;
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
        HeaderText = "Let's encrypt.";
        
        PageIndex = 0;
        Pages = [
            new CryptographyWidgetModel<SubstitutionOptions>( 
                new SubstitutionCryptographer() 
            ), 
            new CryptographyWidgetModel<PermutationOptions>( 
                new PermutationCryptographer()
            ),
            new CryptographyWidgetModel<GammaCryptographerOptions>( 
                new GammaCryptographer()
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
    
    public string HeaderText { get; } 
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
        
        if (currentIndex < 1)
            currentIndex = Pages.Length;
        
        PageIndex = (currentIndex - 1) % Pages.Length;
    }
    
    
}