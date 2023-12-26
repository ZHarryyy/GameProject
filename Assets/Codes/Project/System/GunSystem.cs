using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace PlatformShoot
{
    public interface IGunSystem : ISystem
    {
        void SwitchGun(bool isPositive, LoopList<GunInfo> infos);
        GunInfo[] GetAll();
        bool GetGun(string name, out GunInfo gun);
    }

    [Serializable]
    public class GunInfo
    {
        public string name;
        public int remainingCount;
        public int capacity;
        public int consumptionQuantity;
        public float frequency;
        public float reloadTime;

        public GunInfo(string name, int capacity, float frequency, float reloadTime, int consumptionQuantity)
        {
            this.name = name;
            this.capacity = capacity;
            this.frequency = frequency;
            this.reloadTime = reloadTime;
            this.consumptionQuantity = consumptionQuantity;
            remainingCount = capacity;
        }

        public GunInfo(GunInfo info) : this(info.name, info.capacity, info.frequency, info.reloadTime, info.consumptionQuantity)
        {

        }

        public void Reload()
        {
            remainingCount = capacity;
        }

        public bool Shoot()
        {
            if(remainingCount >= consumptionQuantity)
            {
                remainingCount -= consumptionQuantity;
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class LoopList<T> : IEnumerable<T>
    {
        [SerializeField] private List<T> mItems;
        [SerializeField] private int vernier;
        public T Current => mItems[vernier];

        public LoopList(int capacity)
        {
            mItems = new List<T>(capacity);
            vernier = 0;
        }

        public LoopList(IEnumerable<T> items)
        {
            mItems = items.ToList<T>();
        }

        public LoopList() : this(2)
        {

        }

        public void Add(T e)
        {
            mItems.Add(e);
        }

        public void LoopNeg()
        {
            vernier = (vernier + mItems.Count - 1) % mItems.Count;
        }

        public void LoopPos()
        {
            vernier = (vernier + 1) % mItems.Count;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return default;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return default;
        }
    }

    public class GunSystem : AbstractSystem, IGunSystem
    {
        private Dictionary<string, GunInfo> mGuns;

        protected override void OnInit()
        {
            mGuns = new Dictionary<string, GunInfo>()
            {
                {"手枪", new GunInfo("手枪", 10, 0.5f, 1f, 1)},
                {"机枪", new GunInfo("机枪", 40, 0.2f, 2f, 1)}
            };
        }

        GunInfo[] IGunSystem.GetAll()
        {
            var guns = new GunInfo[mGuns.Count];
            int index = 0;
            foreach(var gun in mGuns.Values)
            {
                guns[index++] = gun;
            }
            return guns;
        }

        bool IGunSystem.GetGun(string name, out GunInfo gun)
        {
            if(mGuns.TryGetValue(name, out gun))
            {
                gun = new GunInfo(gun);
                return true;
            }
            return false;
        }

        void IGunSystem.SwitchGun(bool isPositive, LoopList<GunInfo> infos)
        {
            if(isPositive)
            {
                infos.LoopPos();
            }
            else
            {
                infos.LoopNeg();
            }
        }
    }
}