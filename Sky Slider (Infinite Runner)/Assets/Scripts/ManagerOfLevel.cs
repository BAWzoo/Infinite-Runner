using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerOfLevel : MonoBehaviour
{
    public static ManagerOfLevel instance;

    private void Awake()
    {
        if (ManagerOfLevel.instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void GameOver()
    {
        ManagerOfUI _ui = GetComponent<ManagerOfUI>();
        if (_ui != null)
        {
            _ui.ToggleDeathScreen();
        }
    }
}
