
#if ANDROID
using Firebase.Messaging;
using Android.Gms.Extensions;
#endif

namespace Mixology.Services;

public interface IFirebaseService
{
    Task<string> GetTokenAsync();
    Task SubscribeToTopicAsync(string topic);
    Task UnsubscribeFromTopicAsync(string topic);
}

#if ANDROID

public class FirebaseService : IFirebaseService
{
    public async Task<string> GetTokenAsync()
    {
        try
        {
            var tokenTask = FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
            var token = await tokenTask;
            return token?.ToString() ?? string.Empty;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task SubscribeToTopicAsync(string topic)
    {
        try
        {
            var task = FirebaseMessaging.Instance.SubscribeToTopic(topic).AsAsync();
            await task;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error {topic}: {ex.Message}");
        }
    }

    public async Task UnsubscribeFromTopicAsync(string topic)
    {
        try
        {
            var task = FirebaseMessaging.Instance.UnsubscribeFromTopic(topic).AsAsync();
            await task;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error {topic}: {ex.Message}");
        }
    }
}

#else

public class FirebaseService : IFirebaseService
{
    public Task<string> GetTokenAsync() => Task.FromResult(string.Empty);
    public Task SubscribeToTopicAsync(string topic) => Task.CompletedTask;
    public Task UnsubscribeFromTopicAsync(string topic) => Task.CompletedTask;
}

#endif