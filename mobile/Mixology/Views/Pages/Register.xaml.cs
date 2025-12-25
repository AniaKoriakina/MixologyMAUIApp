using ReactiveUI.Maui;
using Mixology.ViewModels;

namespace Mixology.Views.Pages;

public partial class Register : ReactiveContentPage<RegisterVM>
{
    public Register(RegisterVM vm)
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
