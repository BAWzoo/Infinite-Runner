using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField] private PlayerController player;
    public void Setup()
    {
        player.isDead = true;
        gameObject.SetActive(true);
    }

    public void RestartButton() {
        SceneManager.LoadScene("Tutorial");
    }
}
