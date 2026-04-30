using UnityEngine;

public class CountAvailableInteractionObjects : MonoBehaviour
{
    [SerializeField] private InteractionObject _interactionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.CameraManager.CameraInteractionObject.AvailableInteractionObjects.Add(_interactionObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.CameraManager.CameraInteractionObject.AvailableInteractionObjects.Remove(_interactionObject);
        }
    }
}
