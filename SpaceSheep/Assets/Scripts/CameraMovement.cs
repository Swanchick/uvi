using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _characterPos;
    
    private void Update()
    {
        Vector3 pos = new Vector3(_characterPos.position.x, _characterPos.position.y, -10);
        
        transform.position = Vector3.Lerp(transform.position, pos, _speed * Time.deltaTime);
    }
}
