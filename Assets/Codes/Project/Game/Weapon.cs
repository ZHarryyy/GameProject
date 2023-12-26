using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public abstract class Weapon : PlatformShootGameController
    {
        public enum E_State : byte
        {
            Idle, CoolDown, Reload, Wait
        }

        [SerializeField] protected LoopList<GunInfo> mGuns;
        [SerializeField] protected E_State mWeaponState;

        protected virtual void Awake()
        {
            mGuns = new LoopList<GunInfo>(this.GetSystem<IGunSystem>().GetAll());
            mWeaponState = E_State.Idle;
        }

        protected abstract void ShootFunc(float shootDir);

        public void Shoot(float shootDir)
        {
            switch(mWeaponState)
            {
                case E_State.Idle:
                    if(mGuns.Current.Shoot())
                    {
                        ShootFunc(shootDir);
                        mWeaponState = E_State.CoolDown;
                    }
                    else
                    {
                        mWeaponState = E_State.Reload;
                    }
                    break;
                case E_State.CoolDown:
                    this.GetSystem<ITimerSystem>().AddTimer(mGuns.Current.frequency, () =>
                    {
                        mWeaponState = E_State.Idle;
                    });
                    mWeaponState = E_State.Wait;
                    break;
                case E_State.Reload:
                    this.GetSystem<ITimerSystem>().AddTimer(mGuns.Current.reloadTime, () =>
                    {
                        mWeaponState = E_State.Idle;
                        mGuns.Current.Reload();
                    });
                    mWeaponState = E_State.Wait;
                    break;
                case E_State.Wait:
                    break;
            }
        }
    }
}