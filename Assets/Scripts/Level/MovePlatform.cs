using UnityEngine;
using UnityEngine.Serialization;


public class MovePlatform : MonoBehaviour
{
    public Transform posA;
    public Transform posB;
    
    [SerializeField] private float _moveSpeed;
    private Transform _target;

    void Start()
    {
        transform.position = posA.position;
        _target = posB;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, posB.position) < 0.1f)
        {
            _target = posA;
        }
        if (Vector2.Distance(transform.position, posA.position) < 0.1f)
        {
            _target = posB;
        }
        transform.position = Vector2.MoveTowards(transform.position, _target.position, Time.deltaTime * _moveSpeed);
    }
}
