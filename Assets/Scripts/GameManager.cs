using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    IntoVideo, // Intro Cutscene
    Main,
    DeathVideo,
    Dead,
    WinVideo,
    Win
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;
    public Config config;
    public PlayerController player;

    public List<PlayerController> players = new List<PlayerController>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Multiple instances of GameManager. There may be only one!!!");

        if (config == null)
            Debug.LogError("Config cannot be null.");

        players = new List<PlayerController>();
    }
}
