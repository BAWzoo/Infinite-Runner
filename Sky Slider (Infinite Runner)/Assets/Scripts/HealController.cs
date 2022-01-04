using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{

    [SerializeField] private int healthRegen;

    [SerializeField] private HealthController _healthController;

    public AudioSource source;
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && _healthController.isDamaged())
        {
            //source.PlayOneShot(clip);
            Heal();
        }

    }

    void Heal()
    {
        _healthController.playerHealth = _healthController.playerHealth + healthRegen;
        _healthController.UpdateHealth();
        gameObject.SetActive(false);
    }
}
