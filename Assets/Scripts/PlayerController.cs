using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
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

    void Update()
    {
        bool isPlayer = GameManager.instance.player == this;
        Config config = GameManager.instance.config;

        // Update speechBubble & organ icon
        bool isBubbleVisible = true;
        if (!isPlayer && !hasHeart)
            organIcon.sprite = config.HeartSprite;
        else if (!isPlayer && !hasLungs)
            organIcon.sprite = config.LungSprite;
        else if (!isPlayer && !hasLeftKidney)
            organIcon.sprite = config.LeftKidneySprite;
        else if (!isPlayer && !hasRightKidney)
            organIcon.sprite = config.RightKidneySprite;
        else if (!isPlayer && !hasSpleen)
            organIcon.sprite = config.SpleenSprite;
        else
            isBubbleVisible = false;
        speechBubble.gameObject.SetActive(isBubbleVisible);
    }
}
