using FTKModLib.Objects;

namespace FTKModLib.Example {
    public class ExampleProficiency : CustomProficiency {
        public ExampleProficiency() {
            ID = "CustomProficiency";
            Name = new("Example Attack");
            SlotOverride = 2;
        }
    }
}
