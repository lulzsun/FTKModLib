# FTKModLib - For The King Modding Library
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
#### Managers
- **Item Manager**  
Allows the modification and creation of items (herbs, orbs, armor, weapons). As simple as ```ItemManager.AddItem();``` !
- **Class Manager**  
Allows the modification and creation of classes (blacksmith, hunter, scholar...etc). As simple as ```ClassManager.AddClass();``` !

## Special thanks to
[999gary](https://github.com/999gary)/[FTKExampleItemMod](https://github.com/999gary/FTKExampleItemMod) used as learning reference

[CarbonNikm](https://github.com/CarbonNikm)/[FTK-Debugging-Commands](https://github.com/CarbonNikm/FTK-Debugging-Commands) used as learning reference

Valheim-Modding/[Jotunn](https://github.com/Valheim-Modding/Jotunn) used as learning reference
