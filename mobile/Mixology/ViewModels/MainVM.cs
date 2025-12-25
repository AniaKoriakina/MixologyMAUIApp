using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Mixology.Services;
using Mixology.Services.DTOs;
using Mixology.Views.Pages;
using ReactiveUI;

namespace Mixology.ViewModels;

public class MainVM : ReactiveObject, IActivatableViewModel
{
    private readonly MixService _mixService;
    private readonly UserService _userService;
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

    private bool _isLoadingMore;

    public bool IsLoadingMore
    {
        get => _isLoadingMore;
        set => this.RaiseAndSetIfChanged(ref _isLoadingMore, value);
    }

    private string? _errorMessage;

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    private int _currentPage = 1;

    public int CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    private bool _hasNextPage;

    public bool HasNextPage
    {
        get => _hasNextPage;
        set => this.RaiseAndSetIfChanged(ref _hasNextPage, value);
    }

    private int _totalCount;

    public int TotalCount
    {
        get => _totalCount;
        set => this.RaiseAndSetIfChanged(ref _totalCount, value);
    }

    private const int PageSize = 5;

    public ReactiveCommand<Unit, Unit> LoadMixesCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadMoreMixesCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleFilterCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearSearchCommand { get; }
    public ReactiveCommand<string, Unit> SetSortCommand { get; }
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

    public MainVM(MixService mixService, UserService userService)
    {
        _mixService = mixService;
        _userService = userService;
        Activator = new ViewModelActivator();

        OrderBy = "rating_desc";

        LoadMixesCommand = ReactiveCommand.CreateFromTask(LoadMixesAsync);
        LoadMoreMixesCommand = ReactiveCommand.CreateFromTask(LoadMoreMixesAsync,
            this.WhenAnyValue(x => x.HasNextPage, x => x.IsLoadingMore, (hasNext, loading) => hasNext && !loading));
        RefreshCommand = ReactiveCommand.CreateFromTask(RefreshMixesAsync);
        ToggleFilterCommand = ReactiveCommand.Create(() => { IsFilterVisible = !IsFilterVisible; });
        ClearSearchCommand = ReactiveCommand.Create(() => { SearchText = ""; });
        SetSortCommand = ReactiveCommand.Create<string>(sortBy => { OrderBy = sortBy; });

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(500))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => RefreshCommand.Execute().Subscribe());

        this.WhenAnyValue(x => x.OrderBy)
            .Skip(1)
            .Subscribe(_ => RefreshCommand.Execute().Subscribe());

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            LoadMixesCommand.Execute().Subscribe().DisposeWith(disposables);
        });

        LogoutCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await _userService.LogoutAsync();
            await Shell.Current.GoToAsync("//login");
        });
    }

    private async Task LoadMixesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            CurrentPage = 1;

            var pagedResult = await _mixService.GetMixesPagedAsync(SearchText, OrderBy, CurrentPage, PageSize);

            Mixes = new ObservableCollection<MixDto>(pagedResult.Items);
            HasNextPage = pagedResult.HasNextPage;
            TotalCount = pagedResult.TotalCount;
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

    private async Task LoadMoreMixesAsync()
    {
        if (!HasNextPage || IsLoadingMore) return;

        try
        {
            IsLoadingMore = true;
            ErrorMessage = null;
            CurrentPage++;

            var pagedResult = await _mixService.GetMixesPagedAsync(SearchText, OrderBy, CurrentPage, PageSize);

            foreach (var mix in pagedResult.Items)
            {
                Mixes.Add(mix);
            }

            HasNextPage = pagedResult.HasNextPage;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка загрузки: {ex.Message}";
            CurrentPage--;
        }
        finally
        {
            IsLoadingMore = false;
        }
    }

    private async Task RefreshMixesAsync()
    {
        CurrentPage = 1;
        await LoadMixesAsync();
    }
}