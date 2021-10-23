using FTKModLib.Objects;
using GridEditor;

namespace FTKModLib.Example {
    public class ExampleClass : CustomClass {
        public ExampleClass() {
            ID = "CustomClass";
            Name = new CustomLocalizedString("Leprechaun");
            Description = new CustomLocalizedString("He's a lucky fellow. Also carries around lots of gold.");
            StartingGold = 40;
            StartWeapon = FTK_itembase.ID.bluntToyHammer;
            Strength = 0.4f;
            Vitality = 0.5f;
            Intelligence = 0.4f;
            Awareness = 0.4f;
            Talent = 0.6f;
            Speed = 0.7f;
            Luck = 0.7f;
        }
    }
}