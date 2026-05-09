using UnityEngine;

public abstract class SystemicApp : PhoneApp
{
    [SerializeField] private PhoneUIController _phoneController;

    public PhoneUIController PhoneController => _phoneController;
}
