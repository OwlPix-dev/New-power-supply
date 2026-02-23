using UnityEngine;

public abstract class CharacterRotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 360;

    [SerializeField] private float _minMoveSqrMagnitude = 0.001f;

    private Quaternion _targetRotation;

    private Transform _characterTransform;

    public float MinMoveSqrMagnitude => _minMoveSqrMagnitude;

    public float RotationSpeed => _rotationSpeed;

    public Quaternion TargetRotation
    {
        get => _targetRotation;
        set => _targetRotation = value;
    }

    public Transform CharacterTransform => _characterTransform;

    public virtual void Awake()
    {
        _characterTransform = GetCharacterTransform();
    }

    public virtual void Update()
    {
        if (!Mathf.Approximately(CharacterTransform.rotation.y, _targetRotation.y))
        {
            CharacterTransform.rotation = Quaternion.RotateTowards(CharacterTransform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    public abstract Transform GetCharacterTransform();
}
