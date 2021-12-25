using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{

    [SerializeField] private int damage;

    [SerializeField] private HealthController _healthController;

    public AudioSource source;
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            source.PlayOneShot(clip);
            Damage();
        }

    }

    void Damage()
    {
        _healthController.playerHealth = _healthController.playerHealth - damage;
        _healthController.UpdateHealth();
    }
}
