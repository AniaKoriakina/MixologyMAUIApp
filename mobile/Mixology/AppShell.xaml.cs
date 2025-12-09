using Mixology.Views.Pages;

namespace Mixology;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("main", typeof(Main));

    }
}