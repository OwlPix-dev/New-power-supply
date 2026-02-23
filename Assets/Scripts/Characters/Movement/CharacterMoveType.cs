using UnityEngine;

[CreateAssetMenu(fileName = "NewMoveType", menuName = "MoveType")]
public class CharacterMoveType : ScriptableObject
{
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _accelerationSpeed = 3f;
    [SerializeField] private float _brakingSpeed = 4f;

    [Space]

    [SerializeField] private float _colliderRadius = 0.35f;
    [SerializeField] private float _colliderHeight = 1.6f;
    [SerializeField] private Vector3 _colliderCenter = new Vector3(0f, 0.9f, 0f);

    public float MoveSpeed => _moveSpeed;
    public float AccelerationSpeed => _accelerationSpeed;
    public float BrakingSpeed => _brakingSpeed;

    public float ColliderRadius => _colliderRadius;
    public float ColliderHeight => _colliderHeight;
    public Vector3 ColliderCenter => _colliderCenter;
}
