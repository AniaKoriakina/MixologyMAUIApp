using Mixology.ViewModels;
using ReactiveUI;
using ReactiveUI.Maui;

namespace Mixology.Views.Pages;

public partial class Main : ReactiveContentPage<MainVM>
{
    public Main(MainVM vm)
    {
        InitializeComponent();
        
        ViewModel = vm;
        
        this.WhenActivated(disposables =>
        {
        });
    }
}