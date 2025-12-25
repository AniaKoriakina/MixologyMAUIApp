namespace Mixology.Services.Requests;

public class RecipientRequest
{
    public string Name { get; set; }
    public ContactInfo ContactInfo { get; set; } 
}

public class ContactInfo
{
    public string DeviceToken { get; set; }
}