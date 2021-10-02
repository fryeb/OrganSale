using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 1; // Speed in metres(pixels) per second
    
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 position = m_Transform.position;
        m_Rigidbody.MovePosition(position + Time.fixedDeltaTime * speed * input);
    }
}
