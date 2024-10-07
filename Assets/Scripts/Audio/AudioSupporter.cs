using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;


namespace Audio
{
    public class AudioSupporter : MonoBehaviour
    {
        /// <summary>
        /// ����� Resources ������Ƶ
        /// </summary>
        class ResourcesAudioLoaderPool : AbstractAudioLoaderPool
        {
            protected override IAudioLoader CreateLoader()
            {
                return new ResourcesAudioLoader();
            }
        }

        class ResourcesAudioLoader : IAudioLoader
        {
            private AudioClip mClip;

            public AudioClip Clip => mClip;

            public AudioClip LoadClip(AudioSearchKeys panelSearchKeys)
            {
                mClip = Resources.Load<AudioClip>(panelSearchKeys.AssetName);
                return mClip;
            }

            public void LoadClipAsync(AudioSearchKeys audioSearchKeys, Action<bool, AudioClip> onLoad)
            {
                var resourceRequest = Resources.LoadAsync<AudioClip>(audioSearchKeys.AssetName);
                resourceRequest.completed += operation =>
                {
                    var clip = resourceRequest.asset as AudioClip;
                    onLoad(clip, clip);
                };
            }

            public void Unload()
            {
                Resources.UnloadAsset(mClip);
            }
        }


        void Start()
        {
            // ����ʱ��Ҫ����һ��
            AudioKit.Config.AudioLoaderPool = new ResourcesAudioLoaderPool();
        }
    }
}

