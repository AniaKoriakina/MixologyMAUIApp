using Android.App;
using Android.Runtime;
using Firebase;

namespace Mixology;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    
    public override void OnCreate()
    {
        base.OnCreate();

        if (!FirebaseApp.GetApps(this).Any())
        {
            FirebaseApp.InitializeApp(this);
        }
    }
}