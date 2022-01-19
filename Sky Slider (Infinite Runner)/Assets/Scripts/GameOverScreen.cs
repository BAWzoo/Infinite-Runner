using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField] private PlayerController player;

    public AudioSource source;
    public AudioClip clip;
    public void Setup()
    {
        player.isDead = true;
        gameObject.SetActive(true);

        source.clip = clip;
        source.volume = 0.3F;
        source.Play();
    }

    public void RestartButton() {
        source.Stop();
        SceneManager.LoadScene("Tutorial");
    }
}
