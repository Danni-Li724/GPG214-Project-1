using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxPlayer : MonoBehaviour
    {
        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
           if (clip == null) return;
            Debug.Log($"SfxPlayer playing: {clip.name}, volume: {volume}, pitch: {pitch}");

            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
        }
    }
}