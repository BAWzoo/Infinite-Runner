using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    [SerializeField] private PlayerController player;

    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isDead) {
            if (!isPlaying) {
            source.Stop();
            source.clip = clip;
            source.volume = 0.3F;
            source.Play();
            isPlaying = true;
        }
    }
        
        
    }
}
