# Clippy.WPF

![Clippy Preview](https://github.com/samoatesgames/Clippy.WPF/blob/main/Site/Example%20Clippy%2001.png?raw=true)

Clippy.WPF is a C# library which allows the creation and interaction of Clippy characters.

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
 * Handle double-click events
 * Say text messages
 * Say custom ```FrameworkElement``` messages
 
## Missing Features

 * Positioning
 * Show & Hide support (including animation)
 * Audio
 * Events for animation start/complete
 * Events for message show/dismiss
 * Custom Clippy characters
   
## API

### Creating, showing and closing characters

```
// Create the instance of the character we want
var character = new ClippyCharacter(Character.Clippy);

// Show will present the character as a top most window
character.Show();

// Dismisses a character, a dismissed character can not be reshown and must be recreated
character.Close();
```

### Animations

```
// Get a list of all supported animations for the character
var animations = character.AnimationNames;

// Loop an animation until a different animation is requested
character.PlayAnimation("Idle", AnimationMode.Loop);

// Play an animation once
// When it completes the previous animation will be resumed
character.PlayAnimation("Wave", AnimationMode.Once);
```

### Events

```
// Subscribe to character double click events
character.OnDoubleClick += HandleDoubleClick;

// Called when a character is double clicked
void HandleDoubleClick(ClippyCharacter character)
{
}
```

### Speech

```
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
