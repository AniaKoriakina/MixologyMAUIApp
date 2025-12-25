using Mixology.Services;

namespace Mixology;

public partial class App : Application
{
    private readonly UserService _userService;
    public App(UserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void CheckUserAsync(AppShell shell)
    {
        string? token = null;
        try
        {
            token = await SecureStorage.Default.GetAsync("token");
        }
        catch {}

        if (string.IsNullOrWhiteSpace(token))
        {
            MainPage = new AppShell();
            await Shell.Current.GoToAsync("//login");
            return;
        }
        try
        {
            var user = await _userService.GetCurrentUserInfo(token);
            if (user != null)
            {
                MainPage = new AppShell();
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                await RedirectToLoginAsync();
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await RedirectToLoginAsync();
        }
        catch
        {
            await RedirectToLoginAsync();
        }
    }
    
    private async Task RedirectToLoginAsync()
    {
        try
        {
            await SecureStorage.Default.SetAsync("token", string.Empty);
        }
        catch { }

        MainPage = new AppShell();
        await Shell.Current.GoToAsync("//login");
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var shell = new AppShell();
        CheckUserAsync(shell);     
        return new Window(shell);
    }
}