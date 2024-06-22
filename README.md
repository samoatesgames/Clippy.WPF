# Clippy.WPF

![Clippy Preview](https://github.com/samoatesgames/Clippy.WPF/blob/main/Site/Example%20Clippy%2001.png?raw=true)

Clippy.WPF is a C# library which allows the creation and interaction of Clippy characters.

![GitHub License](https://img.shields.io/github/license/samoatesgames/Clippy.WPF)
![NuGet Downloads](https://img.shields.io/nuget/dt/WPF.Clippy)


![Clippy Preview](https://github.com/samoatesgames/Clippy.WPF/blob/main/Site/Bonzi.gif?raw=true)

## Features

 * Create and how animation Clippy characters
   * Bonzi
   * Clippy
   * F1
   * Genie
   * Genius
   * Links
   * Merlin
   * Peedy
   * Rocky
   * Rover
 * Play any supported animation (looping and one shot)
 * Show & Hide support (including animation)
 * Events for animation complete
 * Handle double-click events
 * Say text messages
 * Say custom ```FrameworkElement``` messages
 * Get and set Clippy character location
 
## Missing Features

 * Audio
 * Events for message show/dismiss
 * Custom Clippy characters

## Thanks

Many thanks to the [ClippyJS](https://github.com/pi0/clippyjs) library which provided art and animation data used by this project.
   
## API

### Creating, showing and closing characters

```cs
// Create the instance of the character we want
var character = new ClippyCharacter(Character.Clippy);

// Show will present the character as a top most window
character.Show();

// Hides a character, hidden characters can be re-shown by calling Show() again.
character.Hide();

// Dismisses a character, a dismissed character can not be reshown and must be recreated
character.Close();
```

### Animations

```cs
// Get a list of all supported animations for the character
var animations = character.AnimationNames;

// Loop an animation until a different animation is requested
character.PlayAnimation("Idle", AnimationMode.Loop);

// Play an animation once
// When it completes the previous animation will be resumed
character.PlayAnimation("Wave", AnimationMode.Once);

// Get the name of the currently active looping animation
var activeLoopAnimation = character.GetActiveAnimation(AnimationMode.Loop);
```

### Events

```cs
// Subscribe to character double click events
character.OnDoubleClick += HandleDoubleClick;

// Called when a character is double clicked
void HandleDoubleClick(ClippyCharacter character)
{
}

// Subscribe to character location changed events
character.LocationChanged += HandleLocationChanged;

// Called when a characters location has changed
void HandleLocationChanged(ClippyCharacter character, Point location)
{
}

// Subscribe to character animation completed events
character.OnAnimationCompleted += HandleAnimationCompleted;

// Called when a characters animation has completed
void OnCharacterAnimationComplete(ClippyCharacter sender, string animationName, AnimationMode mode)
{
}
```

### Speech

```cs
// Display a text message, which will be automatically dismissed after 4 seconds
character.Say("Hello!", TimeSpan.FromSeconds(4));

// Show a custom control in a speech bubble.
// If no TimeSpan dismiss timout is specified, the speech bubble will be shown indefinatly
character.Say(new TextBlock 
{ 
	Text = "RAWR", 
	Foreground = Brushes.Red 
});
```
