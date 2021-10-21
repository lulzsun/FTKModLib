# FTKModLib - For The King Modding Library

[![Downloads][download-badge]][download-link] [![Discord][discord-badge]][discord-link]

[download-badge]: https://img.shields.io/github/downloads/lulzsun/FTKModLib/total
[download-link]: https://github.com/lulzsun/FTKModLib/releases/

[discord-badge]: https://img.shields.io/discord/900685481858183178?label=discord&logo=discord
[discord-link]: https://discord.gg/FqhEBfZaC8
FTKModLib is a work-in-progress modding framework to aid developers in creating For The King mods, by providing centralized and simplified APIs for the game.

## Installation
1. **Install BepInEx**  
Download [BepInEx](https://github.com/BepInEx/BepInEx/releases), extract into your For The King folder (`~\<PathToYourSteamLibrary>\steamapps\common\For The King`).

2. **Install FTKModLib**  
Download the latest release from the releases tab (at the time, does not exist yet), or compile source code, and place .dll in the BepInEx plugins folder.

You are now ready to launch the game and mod away!

## Documentation
There is a provided example plugin(s) made with the use of the library located in the Examples folder.

## Features
#### Objects
- **CustomItem**  
Can create an item with full custom functionality or inherit functionality from existing items (herbs, orbs, armor, weapons).
- **CustomClass**  
Can create a character class with full custom functionality or inherit functionality from existing classes (blacksmith, hunter, etc.).

#### Managers
- **AssetManager**  
Allows importing custom assets such as models and images, to be used with custom or existing GameObjects.
- **ItemManager**  
Allows the modification and addition of items. As simple as ```ItemManager.AddItem(new CustomItem());``` !
- **ClassManager**  
Allows the modification and addition of classes. As simple as ```ClassManager.AddClass(new CustomClass());``` !

## Special thanks to
[999gary](https://github.com/999gary)/[FTKExampleItemMod](https://github.com/999gary/FTKExampleItemMod) used as learning reference

[CarbonNikm](https://github.com/CarbonNikm)/[FTK-Debugging-Commands](https://github.com/CarbonNikm/FTK-Debugging-Commands) used as learning reference

Valheim-Modding/[Jotunn](https://github.com/Valheim-Modding/Jotunn) used as learning reference
