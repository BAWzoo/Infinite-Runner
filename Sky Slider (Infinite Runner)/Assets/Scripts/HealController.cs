using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealController : MonoBehaviour
{

    [SerializeField] private int healthRegen;

    [SerializeField] private HealthController _healthController;

    public AudioSource source;
    public AudioClip clip;
    private bool isHit = false;

    void Start()
    {
        isHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && _healthController.isDamaged() && !isHit)
        {
            isHit = true;
            //source.PlayOneShot(clip);
            Heal();
        }

    }

    void Heal()
    {
        int new_health = _healthController.playerHealth + healthRegen;
        if (new_health > _healthController.maxHealth) 
        {
            new_health = _healthController.maxHealth;
        }
        else if (new_health < 0)
        {
            new_health = 0;
        }

        _healthController.playerHealth = new_health;
        _healthController.UpdateHealth();
        gameObject.SetActive(false);
    }
}
