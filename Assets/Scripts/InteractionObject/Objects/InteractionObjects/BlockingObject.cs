using System.Collections.Generic;
using UnityEngine;

public class BlockingObject : InteractionObject
{
    [SerializeField] private GameObject _destroyObject;

    [SerializeField] private List<InteractionObject> _blockers = new List<InteractionObject>();

    private void Start()
    {
        foreach (InteractionObject blocker in _blockers)
        {
            blocker.GetComponent<Blocker>().OnDestroy += () =>
            {
                _blockers.Remove(blocker);

                if (_blockers.Count <= 0)
                {
                    Destroy(_destroyObject);
                }
            };
        }
    }
}
