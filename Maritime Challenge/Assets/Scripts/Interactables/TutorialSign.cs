using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSign : BaseInteractable
{
    [SerializeField]
    private ParticleSystem confetti;

    [SerializeField]
    private AudioSource audioSource;

    private bool interacted;
    public bool Interacted
    {
        get { return interacted; }
        private set { }
    }

    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "Press Me!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        confetti.Play();
        interacted = true;
        audioSource.Play();
    }
}
