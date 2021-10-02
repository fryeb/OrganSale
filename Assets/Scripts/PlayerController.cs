using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public bool isAlive = true;
    public bool hasHeart = true;
    public bool hasLungs = true;
    public bool hasLKidney = true;
    public bool hasRKidney = true;
    public bool hasSpleen = true;
    public int money = 1000;
    public double blood = 100;

    private Transform m_Transform;
    private Rigidbody2D m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        Debug.Assert(m_Rigidbody.freezeRotation == true);
        Debug.Assert(m_Rigidbody.gravityScale == 0.0);
    }

    // Update is called once per frame
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
}
