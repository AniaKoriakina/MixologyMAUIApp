using Mixology.Services.DTOs;

namespace Mixology.Views.Components;

public partial class MixCard : ContentView
{
    public static readonly BindableProperty MixProperty =
        BindableProperty.Create(
            nameof(Mix),
            typeof(MixDto),
            typeof(MixCard),
            null,
            propertyChanged: OnMixChanged);

    public MixDto? Mix
    {
        get => (MixDto?)GetValue(MixProperty);
        set => SetValue(MixProperty, value);
    }

    public MixCard()
    {
        InitializeComponent();
    }

    private static void OnMixChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MixCard card)
        {
            card.BindingContext = newValue; 
        }
    }
}
