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
}
