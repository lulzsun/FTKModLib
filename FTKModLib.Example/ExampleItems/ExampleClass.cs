using FTKModLib.Objects;
using GridEditor;

namespace FTKModLib.Example {
    public class ExampleClass : CustomClass {
        public ExampleClass() {
            ID = "CustomClass";
            DisplayName = "Gunslinger";
            //StartWeapon = (FTK_itembase.ID)customGun;
            Awareness = 0.9f;
        }
    }
}
