using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Config config;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Multiple instances of GameManager. There may be only one!!!");

        if (config == null)
            Debug.LogError("Config cannot be null.");
    }
}
