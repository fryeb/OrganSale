using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    
    // Update is called once per frame
    void Update()
    {
        playerName.text = GameManager.instance.player.transform.name;
    }
}
