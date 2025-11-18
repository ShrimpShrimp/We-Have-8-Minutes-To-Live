using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour 
{
    // Called when the player presses E while in range
    public abstract void Interact();
}