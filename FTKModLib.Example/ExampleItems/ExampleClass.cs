using FTKModLib.Objects;
using GridEditor;

namespace FTKModLib.Example {
    public class ExampleClass : CustomClass {
        public ExampleClass() {
            ID = "CustomClass";
            Name = new CustomLocalizedString("Gunslinger");
            Description = new CustomLocalizedString("He's a good shot.");
            //StartWeapon = (FTK_itembase.ID)customGun;
            Awareness = 0.9f;
            Talent = 0.9f;
        }
    }
}
