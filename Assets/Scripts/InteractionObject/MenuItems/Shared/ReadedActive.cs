using System.Linq;
using UnityEngine;

public class ReadedActive : InteractionObjectActive
{
    [SerializeField] private ExternalApp[] _addApps;

    [SerializeField] private NewScreen[] _openScreens;

    private PlayerManager _playerManager;

    private bool _isReading => _playerManager != null;

    private void Update()
    {
        if (_isReading == false) { return; }

        if (_playerManager.PlayerMovementState.PlayerMove.IsPlayerMove == true)
        {
            _playerManager.UIManager.PhoneUIController.MenuApp.ExternalApps.RemoveAll(app => _addApps.Contains(app));
            _playerManager = null;
        }
    }

    public override bool Active(PlayerManager playerManager)
    {
        _playerManager = playerManager;

        _playerManager.UIManager.PhoneUIController.MenuApp.ExternalApps.AddRange(_addApps);

        _playerManager.UIManager.PhoneUIController.OpenPhoneByScreens(_openScreens);

        return true;
    }
}
