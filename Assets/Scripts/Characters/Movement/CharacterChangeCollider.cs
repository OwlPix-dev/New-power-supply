using UnityEngine;

public abstract class CharacterChangeCollider : MonoBehaviour
{
    private CharacterController _characterController;
    private CapsuleCollider _capsuleCollider;

    public CharacterController CharacterController => _characterController;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;

    private Transform _transform;

    public virtual void Awake()
    {
        _characterController = GetCharacterController();
        _capsuleCollider = GetCapsuleCollider();

        _transform = transform;
    }

    public bool ChangeCollider(float radius, float height, Vector3 center)
    {
        Vector3 bottom = _transform.position + Vector3.up * radius;
        Vector3 top = bottom + Vector3.up * (height - radius * 2);

        int layer = ~LayerMask.GetMask("Player");

        bool isCanChangeCollider = Physics.CheckCapsule(bottom, top, radius, layer, QueryTriggerInteraction.Ignore) == false;

        if (isCanChangeCollider == true)
        {
            SetCharacterControllerParameters(radius, height, center);

            SetCapsuleColliderParameters(radius, height, center);
        }

        return isCanChangeCollider;
    }

    private void SetCharacterControllerParameters(float radius, float height, Vector3 center)
    {
        _characterController.radius = radius;
        _characterController.height = height;
        _characterController.center = center;
    }

    private void SetCapsuleColliderParameters(float radius, float height, Vector3 center)
    {
        _capsuleCollider.radius = radius;
        _capsuleCollider.height = height;
        _capsuleCollider.center = center;
    }

    public abstract CharacterController GetCharacterController();

    public abstract CapsuleCollider GetCapsuleCollider();
}
