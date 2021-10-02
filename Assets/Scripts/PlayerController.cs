using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;

    public bool hasBrain = true;
    public bool hasHeart = true;
    public bool hasLungs = true;
    public bool hasLeftKidney = true;
    public bool hasRightKidney = true;
    public bool hasSpleen = true;
    public int money = 1000;
    public double blood = 100;

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;

    private SpriteRenderer speechBubble;
    private SpriteRenderer organIcon;

    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(m_Rigidbody.freezeRotation == true);
        Debug.Assert(m_Rigidbody.gravityScale == 0.0);

        Transform speechBubbleTransform = m_Transform.Find("SpeechBubble");
        speechBubble = speechBubbleTransform.GetComponent<SpriteRenderer>();
        Transform organIconTransform = speechBubbleTransform.Find("OrganIcon");
        organIcon = organIconTransform.GetComponent<SpriteRenderer>();

        GameManager.instance.players.Add(this);
    }

    void FixedUpdate()
    {
        bool isPlayer = GameManager.instance.player == this;
        if (isPlayer) {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 position = m_Transform.position;
            float speed = GameManager.instance.config.MovementSpeed;
            m_Rigidbody.MovePosition(position + Time.fixedDeltaTime * speed * input);
        }
    }

    public bool WantsBrain() { return !hasBrain && money > GameManager.instance.config.BrainPrice; }
    public bool WantsHeart() { return !hasHeart && money > GameManager.instance.config.HeartPrice; }
    public bool WantsLungs() { return !hasLungs && money > GameManager.instance.config.LungPrice; }
    public bool WantsLeftKidney() { return !hasLeftKidney && money > GameManager.instance.config.LeftKidneyPrice; }
    public bool WantsRightKidney() { return !hasRightKidney && money > GameManager.instance.config.RightKidneyPrice; }
    public bool WantsSpleen() { return !hasSpleen && money > GameManager.instance.config.SpleenPrice; }

    void Update()
    {
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

        // Only player logic from here on
        if (!isPlayer) return;

        PlayerController closestPlayer = null;
        float distanceToClosestPlayer = Mathf.Infinity;
        Debug.Assert(GameManager.instance.players.Count > 1);
        foreach (PlayerController player in GameManager.instance.players) {
            if (player == this) continue; // Skip ourselves

            float distanceToPlayer = Vector2.Distance(m_Transform.position, player.m_Transform.position);
            if (distanceToPlayer < distanceToClosestPlayer)
                closestPlayer = player;
        }
        Debug.Assert(closestPlayer != null);

        if (hasBrain && closestPlayer.WantsBrain()) {
            UIController.SetMessage($"Press E to sell Brain to {closestPlayer.name} for ${config.BrainPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasBrain = true;
                this.hasBrain = false;
                closestPlayer.money -= config.BrainPrice;
                this.money += config.BrainPrice;
                // TODO: Swap active player
            }
        } else if (hasHeart && closestPlayer.WantsHeart()) {
            UIController.SetMessage($"Press E to sell Heart to {closestPlayer.name} for ${config.HeartPrice}");
            if (Input.GetKeyDown(KeyCode.E)) {
                closestPlayer.hasHeart = true;
                this.hasHeart = false;
                closestPlayer.money -= config.HeartPrice;
                this.money += config.HeartPrice;
            }
        }

    }
}
