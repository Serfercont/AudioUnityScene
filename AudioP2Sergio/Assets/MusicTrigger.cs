using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class MusicTrigger : MonoBehaviour
    {
        // Audio clips for indoor and outdoor areas
        public AudioClip indoorMusic;
        public AudioClip outdoorMusic;
        
        // Audio source for playing the music
        private AudioSource musicSource;
        
        // Optional fade duration (in seconds)
        public float fadeDuration = 1.0f;
        
        // Tag to identify the player (usually "Player")
        public string playerTag = "Player";
        
        // Flag to track if player is inside
        private bool playerIsInside = false;
        
        void Start()
        {
            // Get or create an AudioSource
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Configure the AudioSource
            musicSource.loop = true;
            musicSource.playOnAwake = true;
            musicSource.spatialBlend = 0f; // 2D sound (non-positional)
            
            // Start with outdoor music by default
            if (outdoorMusic != null)
            {
                musicSource.clip = outdoorMusic;
                musicSource.Play();
            }
        }
        
        void OnTriggerEnter(Collider other)
        {
            // Check if the player entered the trigger
            if (other.CompareTag(playerTag) && !playerIsInside)
            {
                playerIsInside = true;
                SwitchToIndoorMusic();
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            // Check if the player exited the trigger
            if (other.CompareTag(playerTag) && playerIsInside)
            {
                playerIsInside = false;
                SwitchToOutdoorMusic();
            }
        }
        
        void SwitchToIndoorMusic()
        {
            if (indoorMusic != null && (musicSource.clip != indoorMusic || !musicSource.isPlaying))
            {
                StartCoroutine(CrossfadeToClip(indoorMusic));
            }
        }
        
        void SwitchToOutdoorMusic()
        {
            if (outdoorMusic != null && (musicSource.clip != outdoorMusic || !musicSource.isPlaying))
            {
                StartCoroutine(CrossfadeToClip(outdoorMusic));
            }
        }
        
        IEnumerator CrossfadeToClip(AudioClip newClip)
        {
            // Store the original volume
            float originalVolume = musicSource.volume;
            
            // Fade out current music
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(originalVolume, 0, timer / fadeDuration);
                yield return null;
            }
            
            // Change clip and play
            musicSource.clip = newClip;
            musicSource.Play();
            
            // Fade in new music
            timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0, originalVolume, timer / fadeDuration);
                yield return null;
            }
            
            // Ensure we're at the original volume
            musicSource.volume = originalVolume;
        }
    }
}
