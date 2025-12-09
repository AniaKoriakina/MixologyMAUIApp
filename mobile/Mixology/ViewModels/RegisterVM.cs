using Mixology.Services;
using ReactiveUI;
using System.Reactive;
using Mixology.Services.Requests;

namespace Mixology.ViewModels;

public class RegisterVM : ReactiveObject, IActivatableViewModel
{
    private readonly UserService _userService;
    private readonly ObservableAsPropertyHelper<bool> _canRegister;
    public ViewModelActivator Activator { get; } = new();
    public bool CanRegister => _canRegister.Value;
    
    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

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

    public bool FieldValidation()
    {
        return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    }

    public ReactiveCommand<Unit, bool> RegisterCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }

    public RegisterVM(UserService userService)
    {
        _userService = userService;

        var canRegister = this
            .WhenAnyValue(
                x => x.Username,
                x => x.Email,
                x => x.Password,
                (u, e, p) =>
                    !string.IsNullOrWhiteSpace(u) &&
                    !string.IsNullOrWhiteSpace(e) &&
                    !string.IsNullOrWhiteSpace(p)
            );
        
        NavigateToLoginCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await Shell.Current.GoToAsync("main");
        });

        RegisterCommand = ReactiveCommand.CreateFromTask(RegisterAsync, canRegister);
        
        canRegister.ToProperty(this, x => x.CanRegister, out _canRegister);
    }

    private async Task<bool> RegisterAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        var request = new RegisterRequest
        {
            Username = Username,
            Email = Email,
            Password = Password
        };

        var success = await _userService.Register(request);

        if (!success)
            ErrorMessage = "Ошибка регистрации";

        IsBusy = false;
        return success;
    }
}
