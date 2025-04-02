# Unity Transparent Window
A simple utility script that lets you build applications with a transparent background, allowing users to see through and click through the window.

> [!Note]
> While this script was primarily written for UnityEngine, it should also work for regular C# programs, as no Unity library was used.

> [!Important]
> Since the script uses functions from `user32.dll`, it is only usable on **Windows** systems

## Features
> This script comes with **3** modes to choose from:

- **Chroma**
    - Designate a color *(**eg.** green)* to be cut out
    - Areas originally to be rendered with that color, either from UI or objects, will become see-through and click-through
    - For 3D scenes, remember to take lighting into account
    - Does **not** support half-transparency *(**eg.** feathered edges of a sprite)*

- **Manual**
    - Turn the entire screen transparent based on alpha *(**eg.** set the `A` channel of the Camera's **Background** to `0`)*
    - Manually control when the program is click-through via code
    - Support half-transparency

- **None**
    - If you just want to hide the Taskbar icon

### Extras
> Some additional features

- **Always on Top**
    - Make the program continue to stay on the screen after the user clicks on other windows underneath *(**ie.** overlay effect)*
    - For UnityEngine, enable the **Run in Background** player setting to let the program continue running

- **Hide Taskbar**
    - Hide the icon of the program in the Taskbar
    - You will need to kill the process via Task Manager, if you did not set up a close button and lost focus to the program

## Prerequisite for Unity
> These requirements only concern UnityEngine

- Disable the **Flip Model Swapchain** under Resolution and Presentation in the Player settings
- Only the **built-in** *(a.k.a **legacy**, **standard**, etc.)* render pipeline is supported

> [!Important]
> To prevent breaking the Editor, this script only functions when actually built

<hr>

- **Reference:** [Video by. CodeMonkey](https://youtu.be/RqgsGaMPZTw?feature=shared)
