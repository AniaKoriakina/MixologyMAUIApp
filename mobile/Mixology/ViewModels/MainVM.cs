using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Mixology.Services;
using Mixology.Services.DTOs;
using ReactiveUI;

namespace Mixology.ViewModels;

public class MainVM : ReactiveObject, IActivatableViewModel
{
    private readonly MixService _mixService;
    private readonly MaterialService _materialService;
    public ViewModelActivator Activator { get; }
    
    
    private ObservableCollection<MixDto> _mixes = new();
    public ObservableCollection<MixDto> Mixes
    {
        get => _mixes;
        set => this.RaiseAndSetIfChanged(ref _mixes, value);
    }

    
    private string _searchText = "";
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }
    
    private string _orderBy = "";
    public string OrderBy
    {
        get => _orderBy;
        set => this.RaiseAndSetIfChanged(ref _orderBy, value);
    }
    
    private bool _isFilterVisible;
    public bool IsFilterVisible
    {
        get => _isFilterVisible;
        set => this.RaiseAndSetIfChanged(ref _isFilterVisible, value);
    }
    
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }
    
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    
    public ReactiveCommand<Unit, Unit> LoadMixesCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleFilterCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }
    public ReactiveCommand<string, Unit> SetSortCommand { get; }
    
    public MainVM(MixService mixService, MaterialService materialService)
    {
        _mixService = mixService;
        _materialService = materialService;
        Activator = new ViewModelActivator();
        
        OrderBy = "rating_desc";
        
        LoadMixesCommand = ReactiveCommand.CreateFromTask(LoadMixesAsync);
        ToggleFilterCommand = ReactiveCommand.Create(() => { IsFilterVisible = !IsFilterVisible; });
        ClearSearchCommand = ReactiveCommand.Create(() => { SearchText = ""; });
        SetSortCommand = ReactiveCommand.Create<string>(sortBy => { OrderBy = sortBy; });
        
        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => LoadMixesCommand.Execute().Subscribe());
        
        this.WhenAnyValue(x => x.OrderBy)
            .Skip(1) 
            .Subscribe(_ => LoadMixesCommand.Execute().Subscribe());
        
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadMixesCommand.Execute().Subscribe().DisposeWith(disposables);
        });
    }
    
    private async Task LoadMixesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            
            var mixesFromApi = await _mixService.GetMixesAsync(SearchText, OrderBy);
            
            foreach (var mix in mixesFromApi)
            {
                await LoadMaterialNamesForMix(mix);
            }
            
            Mixes = new ObservableCollection<MixDto>(mixesFromApi);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка загрузки: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    private async Task LoadMaterialNamesForMix(MixDto mix)
    {
        foreach (var composition in mix.Compositions)
        {
            try
            {
                var material = await _materialService.GetRawMaterial(composition.TobaccoId.ToString());
                composition.MaterialName = material.Name;
            }
            catch
            {
                composition.MaterialName = $"ID: {composition.TobaccoId}";
            }
        }
    }
    
}