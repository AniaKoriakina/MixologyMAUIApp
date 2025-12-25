using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Firebase.Messaging;

namespace Mixology.Platforms.Android;

[Service(Exported = false)]
[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
public class FirebaseMessagingService : Firebase.Messaging.FirebaseMessagingService
{
    private const string ChannelId = "default_channel";
    private const string ChannelName = "Default Channel";

    public override void OnMessageReceived(RemoteMessage message)
    {
        base.OnMessageReceived(message);
        var notification = message.GetNotification();
        if (notification != null)
        {
            SendNotification(notification.Title, notification.Body);
        }

    }

    private void SendNotification(string title, string body)
    {
        try
        {
            CreateNotificationChannel();
            
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("from_notification", true);
            
            var pendingIntentFlags = PendingIntentFlags.UpdateCurrent;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                pendingIntentFlags |= PendingIntentFlags.Immutable;
            }
            
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, pendingIntentFlags);

            var notificationBuilder = new NotificationCompat.Builder(this, ChannelId)
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                .SetContentTitle(title ?? "Firebase Test")
                .SetContentText(body ?? "Test notification")
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetDefaults(NotificationCompat.DefaultAll);

            var notificationManager = NotificationManagerCompat.From(this);
            
            var notificationId = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            notificationManager.Notify(notificationId, notificationBuilder.Build());
        }
        catch (Exception ex)
        {
        }
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            
            var existingChannel = notificationManager?.GetNotificationChannel(ChannelId);
            if (existingChannel == null)
            {
                var channel = new NotificationChannel(ChannelId, ChannelName, NotificationImportance.High)
                {
                    Description = "Firebase push notifications"
                };
                
                notificationManager?.CreateNotificationChannel(channel);
            }
        }
    }
}