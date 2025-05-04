using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    [RequireComponent(typeof(AudioSource))]
    public class DoorSound : MonoBehaviour
    {
        // Audio clips para los sonidos de la puerta
        public AudioClip openSound;
        public AudioClip closeSound;
        
        // Componente de audio
        private AudioSource audioSource;
        
        // Referencias para el control de la animación
        private Animator animator;
        private bool isOpen = false;
        
        // Parámetro del animator (ajustar según el nombre real de tu parámetro)
        public string doorOpenParameterName = "Opening";
        
        void Start()
        {
            // Obtener componentes necesarios
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            
            // Configurar el AudioSource
            if (!audioSource.playOnAwake)
            {
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1.0f; // Sonido totalmente 3D
            }
        }
        
        void Update()
        {
            if (animator != null)
            {
                // Verificar si el estado de la puerta ha cambiado
                bool currentlyOpen = animator.GetBool(doorOpenParameterName);
                
                // Si cambia de cerrada a abierta
                if (currentlyOpen && !isOpen)
                {
                    PlayOpenSound();
                    isOpen = true;
                }
                // Si cambia de abierta a cerrada
                else if (!currentlyOpen && isOpen)
                {
                    PlayCloseSound();
                    isOpen = false;
                }
            }
        }
        
        // Reproduce el sonido de apertura
        public void PlayOpenSound()
        {
            if (openSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
        
        // Reproduce el sonido de cierre
        public void PlayCloseSound()
        {
            if (closeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(closeSound);
            }
        }
    }
}
