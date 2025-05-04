using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOpenSound : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Obtener el componente DoorSound y reproducir el sonido de apertura
        SojaExiles.DoorSound doorSound = animator.GetComponent<SojaExiles.DoorSound>();
        if (doorSound != null)
        {
            doorSound.PlayOpenSound();
        }
    }
}
