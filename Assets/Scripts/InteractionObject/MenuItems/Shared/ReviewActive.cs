using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ReviewActive : InteractionObjectActive
{
    [SerializeField] private InteractionObject[] _childInteractionObjects;

    [SerializeField] private Transform _reviewPoint;

    public Transform ReviewPoint => _reviewPoint;

    public override bool Active(PlayerManager playerManager)
    {
        Review(playerManager);

        return true;
    }

    public virtual void Review(PlayerManager playerManager)
    {
        playerManager.CameraManager.CameraInteractionObject.AvailableInteractionObjects.AddRange(_childInteractionObjects);

        playerManager.CameraManager.CameraReviewObjectState.AddReviewObject(this);

        MainObject.IsCanInteraction = false;
    }

    public virtual void CancelReview(PlayerManager playerManager)
    {
        playerManager.CameraManager.CameraInteractionObject.AvailableInteractionObjects.
            RemoveAll(interactionObject => _childInteractionObjects.Contains(interactionObject));

        playerManager.CameraManager.CameraInteractionObject.CloseContextMenu();

        MainObject.IsCanInteraction = true;
    }
}
