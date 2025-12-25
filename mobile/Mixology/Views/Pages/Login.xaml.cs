using System.Reactive.Disposables.Fluent;
using ReactiveUI.Maui;
using Mixology.ViewModels;
using ReactiveUI;

namespace Mixology.Views.Pages;

public partial class Login : ReactiveContentPage<LoginVM>
{
    public Login(LoginVM vm)
    {
        InitializeComponent();
        ViewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetNavBarIsVisible(this, false);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Shell.SetNavBarIsVisible(this, true);
        Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
    }
}