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

        if (state == GameState.IntroVideo) {
            videoPlayer.gameObject.SetActive(true);
            if (videoPlayer.isPaused) {
                GameManager.instance.state = GameState.Main;
            }
        } else if (state == GameState.Main) {
            videoPlayer.gameObject.SetActive(false);
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
