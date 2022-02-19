using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal") * _speed;

        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y);
    }
}
