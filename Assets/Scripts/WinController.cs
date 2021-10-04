using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public float radius = 64;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Update()
    {
        if (GameManager.instance.state != GameState.Main) return;

        PlayerController player = GameManager.instance.player;
        Config config = GameManager.instance.config;

        if (Vector2.Distance(transform.position, player.transform.position) < radius)
        {
            if (player.money < config.WinPrice)
            {
                UIController.SetMessage($"The game costs ${config.WinPrice}. You only have ${player.money}. Come back when you can afford it.");
            }
            else
            {
                UIController.SetMessage($"Press E to buy the game for ${config.WinPrice}.");
                if (Input.GetKeyDown(KeyCode.E))
                    GameManager.instance.state = GameState.WinVideo;
            }

        }
    }
}
