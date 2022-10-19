using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace SCKRM.Input
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VirtualPointerState : IInputStateTypeInfo
    {
        public FourCC format => new FourCC('V', 'P', 'T', 'F');

#if UNITY_EDITOR
        [InputControl(layout = "Vector2", displayName = "Position", usage = "Point", processors = "AutoWindowSpace", dontReset = true)]
#else
        [InputControl(layout = "Vector2", displayName = "Position", usage = "Point", dontReset = true)]
#endif
        public Vector2 position;

        [InputControl(layout = "Delta", displayName = "Delta", usage = "Secondary2DMotion")]
        public Vector2 delta;
    }

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [InputControlLayout(displayName = "Virtual Pointer", stateType = typeof(VirtualPointerState))]
    public class VirtualPointer : InputDevice, IInputUpdateCallbackReceiver
    {
        static VirtualPointer() => InputSystem.RegisterLayout<VirtualPointer>();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void InitializeInPlayer() { }

        public Vector2Control position { get; private set; }
        public Vector2Control delta { get; private set; }

        // The Input System calls this method after it constructs the Device,
        // but before it adds the device to the system. Do any last-minute setup
        // here.
        protected override void FinishSetup()
        {
            base.FinishSetup();

            position = GetChildControl<Vector2Control>("position");
            delta = GetChildControl<Vector2Control>("delta");
        }

        public void OnUpdate()
        {
            VirtualPointerState state = new VirtualPointerState();
            Debug.Log(Pointer.current.position);
            InputSystem.QueueStateEvent(this, state);
        }
    }
}
