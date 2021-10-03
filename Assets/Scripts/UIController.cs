using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerBalance;
    public TextMeshProUGUI playerBlood;
    public TextMeshProUGUI message;
    public GameObject uiPanel;
    public GameObject deathPanel;
    public GameObject winPanel;
    private bool messageWasSet;

    public Image brainImage;
    public Image heartImage;
    public Image lungImage;
    public Image leftKidneyImage;
    public Image rightKidneyImage;
    public Image spleenImage;
    public VideoPlayer videoPlayer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Multiple instances of UIController. There may be only one!!!");

        Debug.Assert(videoPlayer.playOnAwake);
    }

    void SetPanelsActive(GameState state)
    {
        uiPanel.SetActive(state == GameState.Main);
        deathPanel.SetActive(state == GameState.Dead);
        winPanel.SetActive(state == GameState.Win);
    }

    void PlayVideo(VideoClip clip, GameState nextState)
    {
        videoPlayer.gameObject.SetActive(true);
        if (videoPlayer.clip != clip) {
            videoPlayer.clip = clip;
            videoPlayer.Play();
        } else if (Input.GetKeyDown(KeyCode.Space) || videoPlayer.isPaused) {
            videoPlayer.clip = null;
            GameManager.instance.state = nextState;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        PlayerController player = GameManager.instance.player;
        Config config = GameManager.instance.config;
        GameState state = GameManager.instance.state;

        playerName.text = player.transform.name;
        playerBalance.text = $"Balance: ${player.money}";
        playerBlood.text = $"Blood: {player.blood} / ${config.MaxBlood} (-{player.bleed})";

        brainImage.sprite = player.isAlive ? config.BrainSprite : config.NoBrainSprite;
        heartImage.sprite = player.hasHeart ? config.HeartSprite : config.NoHeartSprite;
        lungImage.sprite = player.hasLungs ? config.LungSprite : config.NoLungSprite;
        leftKidneyImage.sprite = player.hasLeftKidney ? config.LeftKidneySprite : config.NoLeftKidneySprite;
        rightKidneyImage.sprite = player.hasRightKidney ? config.RightKidneySprite : config.NoRightKidneySprite;
        spleenImage.sprite = player.hasSpleen ? config.SpleenSprite : config.NoSpleenSprite;

        SetPanelsActive(state);

        if (state == GameState.IntroVideo) {
            PlayVideo(config.introVideo, GameState.Main);
        } else if (state == GameState.DeathVideo) {
            PlayVideo(config.deathVideo, GameState.Dead);
        } else if (state == GameState.WinVideo) {
            PlayVideo(config.winVideo, GameState.Win);
        }

        if (state == GameState.Dead || state == GameState.Main || state == GameState.Dead) {
            videoPlayer.gameObject.SetActive(false);
            SetPanelsActive(state);
        }
    }

    void LateUpdate()
    {
        // Get rid of message if it wasn't set this frame
        if (!messageWasSet)
            message.text = "";
        messageWasSet = false;
    }

    public static void SetMessage(string message)
    {
        instance.messageWasSet = true;
        instance.message.text = message;
    }
}
