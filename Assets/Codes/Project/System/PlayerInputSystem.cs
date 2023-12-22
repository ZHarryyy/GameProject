using System.Numerics;
using QFramework;
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
        public int inputX;
        public int inputY;
    }

    public struct JumpInputEvent
    {

    }

    public struct ShootInputEvent
    {
        public bool isTrigger;
    }

    public class PlayerInputSystem : AbstractSystem, IPlayerInputSystem, GameControls.IGamePlayActions
    {
        private GameControls mControls = new GameControls();

        private DirInputEvent dirInput;
        private ShootInputEvent shootInput;

        public void Disable()
        {
            mControls.GamePlay.Disable();
        }

        public void Enable()
        {
            mControls.GamePlay.Enable();
        }

        protected override void OnInit()
        {
            mControls.GamePlay.SetCallbacks(this);
            mControls.GamePlay.Enable();
        }

        void GameControls.IGamePlayActions.OnJump(InputAction.CallbackContext context)
        {
            if(!context.started) return;
            this.SendEvent<JumpInputEvent>();
        }

        void GameControls.IGamePlayActions.OnMove(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Vector2 input = context.ReadValue<Vector2>();
                dirInput.inputX = (int)input.X;
                dirInput.inputY = (int)input.Y;
                this.SendEvent(dirInput);
            }
            else if(context.canceled)
            {
                dirInput.inputX = 0;
                dirInput.inputY = 0;
                this.SendEvent(dirInput);
            }
        }

        void GameControls.IGamePlayActions.OnShoot(InputAction.CallbackContext context)
        {
            shootInput.isTrigger = context.ReadValueAsButton();
            this.SendEvent(shootInput);
        }
    }
}