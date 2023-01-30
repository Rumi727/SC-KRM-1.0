using SCKRM.Renderer;
using System;

namespace SCKRM
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SaveLoadUIAttribute : Attribute
    {
        public NameSpacePathPair name { get; } = "";

        /// <summary>
        /// </summary>
        /// <param name="name">type is NameSpacePathPair</param>
        public SaveLoadUIAttribute(string name) => this.name = name;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveLoadUIIgnoreAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class SaveLoadUIConfigBaseAttribute : Attribute
    {
        public int roundingDigits { get; } = 2;
        public string[] hotkeyToDisplay { get; } = new string[0];

        public SaveLoadUIConfigBaseAttribute(int roundingDigits = 2, params string[] hotkeyToDisplay)
        {
            this.roundingDigits = roundingDigits;
            this.hotkeyToDisplay = hotkeyToDisplay;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SaveLoadUIColorPickerConfigAttribute : SaveLoadUIConfigBaseAttribute
    {
        public bool alphaShow { get; } = true;

        public SaveLoadUIColorPickerConfigAttribute(bool alphaShow = true, params string[] hotkeyToDisplay) : base(2, hotkeyToDisplay) => this.alphaShow = alphaShow;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SaveLoadUIInputFieldConfigAttribute : SaveLoadUIConfigBaseAttribute
    {
        public float mouseSensitivity { get; } = 1;
        public SaveLoadUIInputFieldConfigAttribute(float mouseSensitivity = 1, int roundingDigits = 2, params string[] hotkeyToDisplay) : base(roundingDigits, hotkeyToDisplay) => this.mouseSensitivity = mouseSensitivity;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SaveLoadUISliderConfigAttribute : SaveLoadUIInputFieldConfigAttribute
    {
        public float min { get; } = 0;
        public float max { get; } = 1;

        public SaveLoadUISliderConfigAttribute(float min, float max, float mouseSensitivity = 1, int roundingDigits = 2, params string[] hotkeyToDisplay) : base(mouseSensitivity, roundingDigits, hotkeyToDisplay)
        {
            this.min = min;
            this.max = max;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SaveLoadUIToggleConfigAttribute : SaveLoadUIConfigBaseAttribute
    {
        public SaveLoadUIToggleConfigAttribute(params string[] hotkeyToDisplay) : base(2, hotkeyToDisplay) { }
    }
}
