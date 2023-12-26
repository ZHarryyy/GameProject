using System;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlatformShoot
{
    public interface IPlayerInputSystem : ISystem
    {
        void Enable();
        void Disable();
    }

    public struct DirInputEvent
    {
        public int x;
        public int y;
    }

    public struct JumpInputEvent
    {

    }

    public struct ShootInputEvent
    {
        public bool isTrigger;
    }

    public enum E_InputDevice
    {
        Keyboard, Gamepad, Pointer
    }

    public class PlayerInputSystem : AbstractSystem, IPlayerInputSystem, GameControls.IGamePlayActions
    {
        private GameControls mControls = new GameControls();

        private DirInputEvent mInputEvent = new DirInputEvent();
        private ShootInputEvent ShootInput = new ShootInputEvent();

        private float sensitivity = 0.2f;

        private E_InputDevice mCurrentInputDevice;

        protected override void OnInit()
        {   
            mControls.GamePlay.SetCallbacks(this);
            
            var deviceMgr = this.GetSystem<IInputDeviceMgrSystem>();

            deviceMgr.RegisterDeviceChange<Pointer>(() => mCurrentInputDevice = E_InputDevice.Pointer);
            deviceMgr.RegisterDeviceChange<Keyboard>(() => mCurrentInputDevice = E_InputDevice.Keyboard);
            deviceMgr.RegisterDeviceChange<Gamepad>(() => mCurrentInputDevice = E_InputDevice.Gamepad);
        }

        void IPlayerInputSystem.Enable()
        {
            mControls.GamePlay.Enable();
        }

        void IPlayerInputSystem.Disable()
        {
            mControls.GamePlay.Disable();
        }

        void GameControls.IGamePlayActions.OnJump(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            this.SendEvent<JumpInputEvent>();
        }

        void GameControls.IGamePlayActions.OnMove(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                var input = ctx.ReadValue<Vector2>();
                switch(mCurrentInputDevice)
                {
                    case E_InputDevice.Keyboard:
                        mInputEvent.x = (int)input.x;
                        mInputEvent.y = (int)input.y;
                        break;
                    case E_InputDevice.Gamepad:
                        mInputEvent.x = Math.Abs(input.x) < sensitivity ? 0 : input.x > 0 ? 1 : -1;
                        mInputEvent.y = Math.Abs(input.y) < sensitivity ? 0 : input.y > 0 ? 1 : -1;
                        break;
                }
                this.SendEvent(mInputEvent);
            }
            else if(ctx.canceled)
            {
                switch(mCurrentInputDevice)
                {
                    case E_InputDevice.Keyboard:
                        var board = Keyboard.current;

                        switch(mInputEvent.x)
                        {
                            case -1: mInputEvent.x = board.dKey.wasPressedThisFrame || board.rightArrowKey.wasPressedThisFrame ? 1 : 0; break;
                            case 1: mInputEvent.x = board.aKey.wasPressedThisFrame || board.leftArrowKey.wasPressedThisFrame ? -1 : 0; break;
                        }
                        switch(mInputEvent.y)
                        {
                            case -1: mInputEvent.y = board.wKey.wasPressedThisFrame || board.upArrowKey.wasPressedThisFrame ? 1 : 0; break;
                            case 1: mInputEvent.y = board.sKey.wasPressedThisFrame || board.downArrowKey.wasPressedThisFrame ? -1 : 0; break;
                        }
                        break;
                    default:
                        mInputEvent.x = 0;
                        mInputEvent.y = 0;
                        break;
                }
                this.SendEvent(mInputEvent);
            }
        }

        void GameControls.IGamePlayActions.OnShoot(InputAction.CallbackContext context)
        {
            ShootInput.isTrigger = context.ReadValueAsButton();
            this.SendEvent(ShootInput);
        }
    }
}