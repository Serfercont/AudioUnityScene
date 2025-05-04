using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles
{
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 5f;
        public float gravity = -15f;

        Vector3 velocity;
        bool isGrounded;

        // Audio components
        private AudioSource audioSource;
        
        // Footstep audio clips
        public AudioClip[] gridFootsteps;
        public AudioClip[] woodFootsteps;
        public AudioClip[] marbleFootsteps;
        public AudioClip[] rockFootsteps;
        public AudioClip[] carpetFootsteps;
        
        // Footstep parameters
        public float footstepInterval = 0.5f;
        private float footstepTimer = 0f;

        void Start()
        {
            // Get or add AudioSource component
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = controller.isGrounded;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            // Handle footstep sounds if the player is moving and on the ground
            if (isGrounded && (x != 0f || z != 0f))
            {
                HandleFootsteps();
            }
            else
            {
                // Reset timer when not walking
                footstepTimer = 0;
            }
        }

        void HandleFootsteps()
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                // Reset timer
                footstepTimer = footstepInterval;
                
                // Play footstep sound based on surface
                PlayFootstepSound();
            }
        }

        void PlayFootstepSound()
        {
            // Check what the player is standing on
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                // Select appropriate footstep sounds
                AudioClip[] selectedFootsteps;
                
                if (hit.collider.CompareTag("Ground"))
                {
                    selectedFootsteps = gridFootsteps;
                }
                else if (hit.collider.CompareTag("Marble"))
                {
                    selectedFootsteps = marbleFootsteps;
                }
                else if (hit.collider.CompareTag("Rock"))
                {
                    selectedFootsteps = rockFootsteps;
                }
                else if (hit.collider.CompareTag("Carpet"))
                {
                    selectedFootsteps = carpetFootsteps;
                }
                else
                {
                    selectedFootsteps = woodFootsteps; // Default to wood
                }

                // Select a random footstep sound from the array
                if (selectedFootsteps != null && selectedFootsteps.Length > 0)
                {
                    int randomIndex = Random.Range(0, selectedFootsteps.Length);
                    AudioClip randomFootstep = selectedFootsteps[randomIndex];
                    
                    // Play the sound
                    audioSource.PlayOneShot(randomFootstep);
                }
            }
        }
    }
}