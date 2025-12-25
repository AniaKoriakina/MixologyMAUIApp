using Mixology.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using Mixology.Services.Requests;

namespace Mixology.ViewModels;

public class RegisterVM : ReactiveObject, IActivatableViewModel
{
    private readonly UserService _userService;
    private readonly NotificationService _notificationService;
    private readonly IFirebaseService _firebaseService;

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

    private string? _firebaseToken;

    public string? FirebaseToken
    {
        get => _firebaseToken;
        set => this.RaiseAndSetIfChanged(ref _firebaseToken, value);
    }

    public ReactiveCommand<Unit, bool> RegisterCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToLoginCommand { get; }
    public ReactiveCommand<Unit, Unit> GetFirebaseTokenCommand { get; }

    public RegisterVM(UserService userService, NotificationService notificationService,
        IFirebaseService firebaseService)
    {
        _userService = userService;
        _notificationService = notificationService;
        _firebaseService = firebaseService;

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
            await Shell.Current.GoToAsync("//login");
        });

        RegisterCommand = ReactiveCommand.CreateFromTask(RegisterAsync, canRegister);

        GetFirebaseTokenCommand = ReactiveCommand.CreateFromTask(GetFirebaseTokenAsync);

        canRegister.ToProperty(this, x => x.CanRegister, out _canRegister);

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            Observable.FromAsync(InitializeFirebaseAsync).Subscribe().DisposeWith(disposables);
        });
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

        try
        {
            var success = await _userService.Register(request);

            if (!success)
            {
                ErrorMessage = "Ошибка регистрации";
                return false;
            }

            var loginRequest = new LoginRequest
            {
                Email = Email,
                Password = Password
            };

            var loginResponse = await _userService.Login(loginRequest);

            if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
            {
                await SendDeviceTokenAsync();
                await Shell.Current.GoToAsync("//main");
                return true;
            }
            else
            {
                ErrorMessage = "Не удалось войти";
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<bool> SendDeviceTokenAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(FirebaseToken))
            {
                System.Diagnostics.Debug.WriteLine("Получем токен...");
                await InitializeFirebaseAsync();

                if (string.IsNullOrEmpty(FirebaseToken))
                {
                    System.Diagnostics.Debug.WriteLine("Не удалось получить Firebase токен");
                    return false;
                }
            }

            var request = new RecipientRequest
            {
                Name = Username,
                ContactInfo = new ContactInfo
                {
                    DeviceToken = FirebaseToken
                }
            };

            var success = await _notificationService.SendDeviceTokenAsync(request);

            if (success)
            {
                System.Diagnostics.Debug.WriteLine($"Токен устройства успешно отправлен для: {Username}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Ошибка при отправке токена устройства");
            }

            return success;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Исключение при отправке токена устройства: {ex.Message}");
            return false;
        }
    }

    private async Task InitializeFirebaseAsync()
    {
        try
        {
            var token = await _firebaseService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                System.Diagnostics.Debug.WriteLine($"Firebase Token получен: {token[..Math.Min(20, token.Length)]}...");
                FirebaseToken = token;
                await _firebaseService.SubscribeToTopicAsync("general");
                await _firebaseService.SubscribeToTopicAsync("new_mixes");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Firebase Token не получен");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка Firebase: {ex.Message}");
        }
    }

    private async Task GetFirebaseTokenAsync()
    {
        try
        {
            var token = await _firebaseService.GetTokenAsync();

            FirebaseToken = string.IsNullOrEmpty(token) ? "Токен не получен" : $"Токен: {token[..20]}...";

            if (!string.IsNullOrEmpty(token))
            {
                await Clipboard.SetTextAsync(token);
            }
        }
        catch (Exception ex)
        {
        }
    }
}