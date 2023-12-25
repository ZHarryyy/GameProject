using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QFramework
{
    public interface IInputDeviceMgrSystem : ISystem
    {
        void OnEnable();
        void OnDisable();
        void RegisterDeviceChange<T>(Action fun);
    }

    public class InputDeviceMgrSystem : AbstractSystem, IInputDeviceMgrSystem
    {
        private Dictionary<Type, Action> mDeviceSwitchTable;
        private InputDevice mCurDevice;
        
        void IInputDeviceMgrSystem.RegisterDeviceChange<T>(Action fun)
        {
            var type = typeof(T);
            if(mDeviceSwitchTable.ContainsKey(type))
            {
                mDeviceSwitchTable[type] += fun;
            }
            else
            {
                mDeviceSwitchTable.Add(type, fun);
            }
        }

        protected override void OnInit()
        {
            mDeviceSwitchTable = new Dictionary<Type, Action>();
        }

        public void OnEnable()
        {
            InputSystem.onActionChange += DetectCurrentInputDevice;
            InputSystem.onDeviceChange += OnDeviceChanged;
        }

        public void OnDisable()
        {
            InputSystem.onActionChange -= DetectCurrentInputDevice;
            InputSystem.onDeviceChange -= OnDeviceChanged;
            mDeviceSwitchTable.Clear();
        }

        private void DetectCurrentInputDevice(object obj, InputActionChange change)
        {
            if(change != InputActionChange.ActionPerformed) return;
            SwitchingDevice((obj as InputAction).activeControl.device);
        }

        private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            switch(change)
            {
                case InputDeviceChange.Reconnected:
                    Debug.Log(device + "重新链接");
                    SwitchingDevice(device);
                    break;
                case InputDeviceChange.Disconnected:
                    Debug.Log(device + "断开链接");
#if UNITY_STANDALONE || UNITY_EDITOR
                    if(device is Gamepad)
                        device = Keyboard.current;
                    SwitchingDevice(device);
#endif
                    break;
            }
        }

        private void SwitchingDevice(InputDevice device)
        {
            if(device == null || mCurDevice == device) return;

            Type type = null;

            if(device is Keyboard) type = typeof(Keyboard);
            else if(device is Gamepad) type = typeof(Gamepad);
            else if(device is Pointer) type = typeof(Pointer);
            else if(device is Joystick) type = typeof(Joystick);

            if(type == null || !mDeviceSwitchTable.TryGetValue(type, out var callback)) return;
            mCurDevice = device;
            callback?.Invoke();
        }
    }
}