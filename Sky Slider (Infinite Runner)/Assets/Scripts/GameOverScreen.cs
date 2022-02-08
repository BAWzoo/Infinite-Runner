using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField] private PlayerController player;

    [SerializeField] private Text txt;


    private string[] insults = new string[] { "finger slip?", "rip", "L", "but next time, right?", "bozo", "you suck", "nice try", "really, again?", "Death speedrun?", "Tip: you suck", "Tip: don't die" };

    public AudioSource source;
    public AudioClip clip;
    public void Setup()
    {
        player.isDead = true;
        gameObject.SetActive(true);

        int randnum = UnityEngine.Random.Range(0, insults.Length);
        txt.text = insults[randnum];
        source.Stop();
        source.clip = clip;
        source.volume = 0.3F;
        source.Play();
    }

    public void RestartButton() {
        source.Stop();
        SceneManager.LoadScene("Tutorial");
    }
}
