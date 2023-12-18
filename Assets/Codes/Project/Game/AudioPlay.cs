using System.Collections.Generic;
using UnityEngine;

namespace PlatformShoot
{
    public class AudioPlay : MonoBehaviour
    {
        private List<AudioSource> mPlayingList;
        public static AudioPlay Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            mPlayingList = new List<AudioSource>();

            GameObject.DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(string name)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = Resources.Load<AudioClip>("Audio/Sound/" + name);
            source.Play();
            mPlayingList.Add(source);
        }

        private void Update()
        {
            for(int i = mPlayingList.Count -1; i >= 0; i--)
            {
                var source = mPlayingList[i];
                if(!source.isPlaying)
                {
                    mPlayingList.RemoveAt(i);
                    Destroy(source);
                }
            }
        }
    }
}
