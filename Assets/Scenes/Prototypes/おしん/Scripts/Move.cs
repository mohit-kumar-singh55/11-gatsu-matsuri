using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;

    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.linearVelocity = transform.forward * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = -transform.forward * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = transform.right * speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = -transform.right * speed;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rb.linearVelocity = transform.up * speed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rb.linearVelocity = -transform.up * speed;
        }

    }
}
