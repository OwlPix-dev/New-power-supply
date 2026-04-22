using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public struct ButtonData
{
    [SerializeField] private string[] _buttonClassNames;
    [SerializeField] private string _buttonText;
    [SerializeField] private UnityEvent _onClick;

    public string[] ButtonClassNames => _buttonClassNames;
    public string ButtonText => _buttonText;
    public UnityEvent OnClick => _onClick;
}

[System.Serializable]
public class ButtonsContainerData
{
    [SerializeField] private string _buttonsContainerClassName;
    [SerializeField] private ButtonData[] _buttons;

    public void DrawButtons(VisualElement root)
    {
        VisualElement buttonsContainer = root.Q<VisualElement>(className: _buttonsContainerClassName);

        foreach (ButtonData button in _buttons)
        {
            Button newButton = new Button();

            foreach (string className in button.ButtonClassNames)
            {
                newButton.AddToClassList(className);
            }

            newButton.text = button.ButtonText;

            newButton.clicked += () => button.OnClick?.Invoke();

            buttonsContainer.Add(newButton);
        }
    }
}