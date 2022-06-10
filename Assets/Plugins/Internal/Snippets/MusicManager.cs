using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace DanielLochner.Assets
{
    public class MusicManager : MonoBehaviourSingleton<MusicManager>
    {
        #region Fields
        [SerializeField] private SerializableDictionaryBase<string, AudioClip> music;
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private bool useTimeScaledPitch;

        private AudioSource[] sources = new AudioSource[2];
        private int current;
        #endregion

        #region Methods
        private void Start()
        {
            sources[0] = gameObject.AddComponent<AudioSource>();
            sources[1] = gameObject.AddComponent<AudioSource>();

            sources[0].playOnAwake = sources[1].playOnAwake = false;
            sources[0].outputAudioMixerGroup = sources[1].outputAudioMixerGroup = audioMixer;
        }
        private void Update()
        {
            if (useTimeScaledPitch)
            {
                sources[0].pitch = sources[1].pitch = Time.timeScale;
            }
        }

        public void FadeTo(string m)
        {
            int next = (current + 1) % 2;
            sources[next].clip = music[m];
            sources[next].Play();

            StartCoroutine(FadeRoutine(sources[current], 2.5f, 0f));
            StartCoroutine(FadeRoutine(sources[next], 2.5f, 1f));

            current = next;
        }
        public static IEnumerator FadeRoutine(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
        #endregion
    }
}