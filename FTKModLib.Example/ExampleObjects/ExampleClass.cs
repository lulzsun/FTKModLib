using FTKModLib.Objects;
using GridEditor;

namespace FTKModLib.Example {
    public class ExampleClass : CustomClass {
        public ExampleClass() {
            ID = "CustomClass";
            Name = new CustomLocalizedString("Gunslinger");
            Description = new CustomLocalizedString("He's a good shot... but bad at everything else.");
            //StartWeapon = (FTK_itembase.ID)customGun;
            Strength = 0f;
            Vitality = 0f;
            Intelligence = 0f;
            Awareness = 0.9f;
            Talent = 0.9f;
            Speed = 0f;
            Luck = 0f;
        }
    }
}