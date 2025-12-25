using Mixology.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Mixology.Services.Requests;

namespace Mixology.ViewModels;
public class LoginVM : ReactiveObject, IActivatableViewModel
{
    private readonly UserService _userService;
    private readonly ObservableAsPropertyHelper<bool> _canLogin;
    public ViewModelActivator Activator { get; } = new();
    
    public bool CanLogin => _canLogin.Value;

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(ref _email, value);
    }
    
    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }
    
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }
    
    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    
    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToMainCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToRegisterCommand { get; }

    public LoginVM(UserService userService)
    {
        _userService = userService;
        var canLogin = this
            .WhenAnyValue(
                x => x.Email,
                x => x.Password,
                (e, p) =>
                    !string.IsNullOrWhiteSpace(e) &&
                    !string.IsNullOrWhiteSpace(p));

        NavigateToMainCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Shell.Current.GoToAsync($"//main");
        });
        
        NavigateToRegisterCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Shell.Current.GoToAsync("//register");
        });
        
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync, canLogin);
        
        canLogin.ToProperty(this, x => x.CanLogin, out _canLogin);
    }
    
    private async Task LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        var request = new LoginRequest
        {
            Email = Email,
            Password = Password
        };

        try
        {
            var loginResponse = await _userService.Login(request);
            if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
            {
                await SecureStorage.Default.SetAsync("token", loginResponse.Token);
                
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка входа: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}