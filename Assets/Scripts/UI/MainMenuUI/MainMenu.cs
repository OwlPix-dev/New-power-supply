using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private string _continueButtonName;
    [SerializeField] private string _newGameButtonName;
    [SerializeField] private string _settingsButtonName;
    [SerializeField] private string _exitButtonName;

    [SerializeField] private string _levelMenuSceneName;

    private void Start()
    {
        VisualElement root = _uIDocument.rootVisualElement;

        void ButtonRegister(string buttonName, Action onClick)
        {
            Button button = root.Q<Button>(buttonName);
            button.clicked += onClick;
        }

        ButtonRegister(_continueButtonName, ContinueButtonClick);
        ButtonRegister(_newGameButtonName, NewGameButtonClick);
        ButtonRegister(_settingsButtonName, SettingsButtonClick);
        ButtonRegister(_exitButtonName, ExitButtonClick);
    }

    private void ContinueButtonClick()
    {
        SceneManager.LoadScene(_levelMenuSceneName);
    }

    private void NewGameButtonClick()
    {
        SceneManager.LoadScene(_levelMenuSceneName);
    }

    private void SettingsButtonClick()
    {
        
    }

    private void ExitButtonClick()
    {
        Application.Quit();
    }
}
