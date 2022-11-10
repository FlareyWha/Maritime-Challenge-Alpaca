using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [SerializeField]
    private AudioSource cannonballAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Client]
    public void PlayCannonballHitSFX(string sceneName, Vector3 pos)
    {
        if (PlayerData.activeSubScene != sceneName)
            return;

        cannonballAudioSource.transform.position = pos;

        // Play SFX
        cannonballAudioSource.Play();
    }
}
