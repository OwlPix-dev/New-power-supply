using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuUIController : UIState
{
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private ButtonsContainerData[] _menuButtonsContainers;

    [SerializeField] private string _mainMenuSceneName;
    [SerializeField] private string _levelsMenuSceneName;
    [SerializeField] private string _shopMenuSceneName;

    [SerializeField] private NewScreen[] _phoneByInventoryButton;

    [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;

    private void Update()
    {
        UpdatePassiveUI(_uIManager);
    }

    public override void UpdatePassiveUI(UIManager uIManager)
    {
        if (Input.GetKeyDown(_pauseKey))
        {
            if (_uIManager.CurrentUI != this)
            {
                OpenUI(_uIManager);
            }
            else
            {
                ResumeButtonClick();
            }
        }
    }

    public override void OpenUI(UIManager uIManager)
    {
        SetGamePause(true);

        base.OpenUI(uIManager);

        VisualElement root = _uIDocument.rootVisualElement;

        foreach (ButtonsContainerData buttonsContainerData in _menuButtonsContainers)
        {
            buttonsContainerData.DrawButtons(root);
        }
    }

    public override void CloseUI(UIManager uIManager)
    {
        base.CloseUI(uIManager);

        SetGamePause(false);
    }

    public void ResumeButtonClick()
    {
        _uIManager.PreviousUI.ResumeUI(_uIManager);
    }

    public void RestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingsButtonClick()
    {

    }

    public void MenuButtonClick()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void LevelsButtonClick()
    {
        SceneManager.LoadScene(_levelsMenuSceneName);
    }

    public void ShopButtonClick()
    {
        SceneManager.LoadScene(_shopMenuSceneName);
    }

    public void InventoryButtonClick()
    {
        _uIManager.PhoneUIController.OpenPhoneByScreens(_phoneByInventoryButton);
    }

    public void SetGamePause(bool isPause)
    {
        Time.timeScale = isPause == true ? 0f : 1f;

        _uIDocument.enabled = isPause;
    }
}
