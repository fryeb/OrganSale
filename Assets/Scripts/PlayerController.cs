using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    public bool isAlive {get; private set;}
    private bool hasPlayedStartSound = true;

    public bool hasBrain = true;
    public bool hasHeart = true;
    public bool hasLungs = true;
    public bool hasLeftKidney = true;
    public bool hasRightKidney = true;
    public bool hasSpleen = true;
    public int money {get; private set;}

    public double blood {get; private set;}
    public double bleed {get; private set;}
    private double bloodCountDown;

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;
    private AudioSource m_AudioSource;

    private SpriteRenderer speechBubble;
    private SpriteRenderer organIcon;

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(m_Rigidbody.freezeRotation == true);
        Debug.Assert(m_Rigidbody.gravityScale == 0.0);

        Transform speechBubbleTransform = m_Transform.Find("SpeechBubble");
        speechBubble = speechBubbleTransform.GetComponent<SpriteRenderer>();
        Transform organIconTransform = speechBubbleTransform.Find("OrganIcon");
        organIcon = organIconTransform.GetComponent<SpriteRenderer>();

        blood = GameManager.instance.config.MaxBlood;
        bloodCountDown = GameManager.instance.config.BleedDelay;
        money = 0;
        isAlive = true;

        GameManager.instance.players.Add(this);
    }

    void FixedUpdate()
    {
        // Don't update unless in main state
        if (GameManager.instance.state != GameState.Main) return;

        bool isPlayer = GameManager.instance.player == this;
        if (isPlayer) {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 position = m_Transform.position;
            float speed = GameManager.instance.config.MovementSpeed;
            m_Rigidbody.MovePosition(position + Time.fixedDeltaTime * speed * input);
        }
    }

    public bool WantsBrain() { return !hasBrain; }
    public bool WantsHeart() { return !hasHeart; }
    public bool WantsLungs() { return !hasLungs; }
    public bool WantsLeftKidney() { return !hasLeftKidney; }
    public bool WantsRightKidney() { return !hasRightKidney; }
    public bool WantsSpleen() { return !hasSpleen; }

    void PlaySoundEffect(AudioClip clip, bool loop=false) 
    {
        m_AudioSource.clip = clip;
        m_AudioSource.loop = loop;
        m_AudioSource.Play();
    }

    void Update()
    {
        // Don't update unless in main state
        if (GameManager.instance.state != GameState.Main) return;

        bool isPlayer = GameManager.instance.player == this;
        Config config = GameManager.instance.config;

        // Update speechBubble & organ icon
        bool isBubbleVisible = true;
        if (!isPlayer && WantsBrain()) organIcon.sprite = config.BrainSprite;
        else if (!isPlayer && WantsHeart()) organIcon.sprite = config.HeartSprite;
        else if (!isPlayer && WantsLungs()) organIcon.sprite = config.LungSprite;
        else if (!isPlayer && WantsLeftKidney()) organIcon.sprite = config.LeftKidneySprite;
        else if (!isPlayer && WantsRightKidney()) organIcon.sprite = config.RightKidneySprite;
        else if (!isPlayer && WantsSpleen()) organIcon.sprite = config.SpleenSprite;
        else isBubbleVisible = false;
        speechBubble.gameObject.SetActive(isBubbleVisible);

        // Bleeding
        // Only player logic from here on
        if (!isPlayer) {
            if (m_AudioSource.clip != config.brainSFX)
                m_AudioSource.clip = null; // Mute NPCs
            return;
        };

        bloodCountDown -= Time.deltaTime;
        bleed = 0;
        if (!hasBrain) bleed += config.BrainBleed;
        if (!hasHeart) bleed += config.HeartBleed;
        if (!hasLungs) bleed += config.LungBleed;
        if (!hasLeftKidney) bleed += config.LeftKidneyBleed;
        if (!hasRightKidney) bleed += config.RightKidneyBleed;
        if (!hasSpleen) bleed += config.SpleenBleed;

        if (bloodCountDown <= 0) {
            bloodCountDown = config.BleedDelay;
            blood -= bleed;
        }

        if (blood < 0) isAlive = false;

        // Sales
        PlayerController closestPlayer = null;
        float distanceToClosestPlayer = Mathf.Infinity;
        Debug.Assert(GameManager.instance.players.Count > 1);
        foreach (PlayerController player in GameManager.instance.players) {
            if (player == this) continue; // Skip ourselves

            float distanceToPlayer = Vector2.Distance(m_Transform.position, player.m_Transform.position);
            if (distanceToPlayer < distanceToClosestPlayer) {
                closestPlayer = player;
                distanceToClosestPlayer = distanceToPlayer;
            }
        }
        Debug.Assert(closestPlayer != null);

        if (distanceToClosestPlayer > config.SaleDistance) return; // Don't sell organs if there is no-one near

        if (hasBrain && closestPlayer.WantsBrain()) {
            UIController.SetMessage($"Press E to sell Brain to {closestPlayer.name} for ${config.BrainPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasBrain = true;
                this.hasBrain = true; // Can't sell brain back to original
                closestPlayer.money -= config.BrainPrice;
                this.money += config.BrainPrice;

                int tempMoney = this.money;
                this.money = closestPlayer.money;
                closestPlayer.money = tempMoney;
                GameManager.instance.player = closestPlayer;
                PlaySoundEffect(config.brainSFX);
            }
        } else if (hasHeart && closestPlayer.WantsHeart()) {
            UIController.SetMessage($"Press E to sell Heart to {closestPlayer.name} for ${config.HeartPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasHeart = true;
                this.hasHeart = false;
                closestPlayer.money -= config.HeartPrice;
                this.money += config.HeartPrice;
                PlaySoundEffect(config.saleSFX);
            }
        } else if (hasLungs && closestPlayer.WantsLungs()) {
            UIController.SetMessage($"Press E to sell Lungs to {closestPlayer.name} for ${config.LungPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasLungs = true;
                this.hasLungs = false;
                closestPlayer.money -= config.LungPrice;
                this.money += config.LungPrice;
                PlaySoundEffect(config.saleSFX);
            }
        } else if (hasLeftKidney && closestPlayer.WantsLeftKidney()) {
            UIController.SetMessage($"Press E to sell Left Kidney to {closestPlayer.name} for ${config.LeftKidneyPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasLeftKidney = true;
                this.hasLeftKidney = false;
                closestPlayer.money -= config.LeftKidneyPrice;
                this.money += config.LeftKidneyPrice;
                PlaySoundEffect(config.saleSFX);
            }
        } else if (hasRightKidney && closestPlayer.WantsRightKidney()) {
            UIController.SetMessage($"Press E to sell Right Kidney to {closestPlayer.name} for ${config.RightKidneyPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasRightKidney = true;
                this.hasRightKidney = false;
                closestPlayer.money -= config.RightKidneyPrice;
                this.money += config.RightKidneyPrice;
                PlaySoundEffect(config.saleSFX);
            }
        } else if (hasSpleen && closestPlayer.WantsSpleen()) {
            UIController.SetMessage($"Press E to sell Spleen to {closestPlayer.name} for ${config.SpleenPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasSpleen = true;
                this.hasSpleen = false;
                closestPlayer.money -= config.SpleenPrice;
                this.money += config.SpleenPrice;
            }
            PlaySoundEffect(config.saleSFX);
        } else {
            UIController.SetMessage($"{closestPlayer.name}: I don't want anything your selling");
        }

        // Background Sound Effect
        bool isWalking = Input.GetAxisRaw("Horizontal") != 0.0 || Input.GetAxisRaw("Vertical") != 0;
        if (!m_AudioSource.isPlaying) {
            if (!hasPlayedStartSound) {
                hasPlayedStartSound = true;
                PlaySoundEffect(config.playerGameStart, true);
            } else if (blood < config.BleedSoundThreshold) {
                PlaySoundEffect(config.playerBleedingOut);
            } else if (isWalking) {
                PlaySoundEffect(config.playerFootsteps, true);
            }
        }

        if (m_AudioSource.clip == config.playerFootsteps && !isWalking) {
            m_AudioSource.Stop();
        }
    }
}
