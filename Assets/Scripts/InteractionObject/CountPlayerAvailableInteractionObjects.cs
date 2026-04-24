using UnityEngine;

public class CountPlayerAvailableInteractionObjects : MonoBehaviour
{
    [SerializeField] private InteractionObject _interactionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.PlayerInteractionObject.AvailableInteractionObjects.Add(_interactionObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
        {
            playerManager.PlayerInteractionObject.AvailableInteractionObjects.Remove(_interactionObject);
        }
    }
}
