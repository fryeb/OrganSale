using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerBalance;
    public TextMeshProUGUI playerBlood;
    public TextMeshProUGUI message;
    public GameObject uiPanel;
    private bool messageWasSet;

    public Image brainImage;
    public Image heartImage;
    public Image lungImage;
    public Image leftKidneyImage;
    public Image rightKidneyImage;
    public Image spleenImage;
    public VideoPlayer videoPlayer;

    private AudioSource m_AudioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Multiple instances of UIController. There may be only one!!!");

        Debug.Assert(videoPlayer.playOnAwake);
    }

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        Debug.Assert(m_AudioSource.playOnAwake);
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

        videoPlayer.gameObject.SetActive(state != GameState.Main);
        uiPanel.SetActive(state == GameState.Main);

        if (state == GameState.IntroVideo) {
            PlayVideo(config.introVideo, GameState.TitleVideo);
        } else if (state == GameState.TitleVideo) {
            PlayVideo(config.titleVideo, GameState.TitleVideo);
            SetMessage("Press <space> to start.");
            if (Input.GetKeyDown(KeyCode.Space))
                GameManager.instance.state = GameState.Main;
        } else if (state == GameState.DeathVideo) {
            PlayVideo(config.deathVideo, GameState.Dead);
        } else if (state == GameState.WinVideo) {
            PlayVideo(config.winVideo, GameState.Win);
        }

        if (state == GameState.Win || state == GameState.Dead) {
            SetMessage("Press <space> to respawn.");
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Music
        if (state == GameState.Main)
        {
            m_AudioSource.clip = config.gameplaySong;
        } 
        else
        {
            m_AudioSource.clip = config.titleSong;
        }

        if (!m_AudioSource.isPlaying) m_AudioSource.Play();
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
