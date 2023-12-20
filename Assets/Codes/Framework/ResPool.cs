using System;
using System.Collections.Generic;

namespace QFramework
{
    public class ResPool<T> where T : UnityEngine.Object
    {
        private Dictionary<string, T> mResDic = new Dictionary<string, T>();

        public void Get(string key, Action<T> callBack)
        {
            if(mResDic.TryGetValue(key, out T data))
            {
                callBack(data);
                return;
            }
            ResHelper.AsyncLoad<T>(key, o =>
            {
                callBack(o);
                mResDic.Add(key, o);
            });
        }

        public void Clear() => mResDic.Clear();
    }
}
