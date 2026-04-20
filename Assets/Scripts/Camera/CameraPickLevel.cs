using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraPickLevel : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private string _buttonsContainerClassName;

    [SerializeField] private string _toLevelButtonClassName;
    [SerializeField] private string _backButtonClassName;

    [SerializeField] private string _toLevelButtonText = "To Level";
    [SerializeField] private string _backButtonText = "Back";

    [SerializeField] private string _levelPhotoClassName;
    [SerializeField] private string _levelNameClassName;
    [SerializeField] private string _levelDescripClassName;

    [SerializeField] private int _mouseIndex = 0;
    [SerializeField] private float _rayMaxDistance = 100f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseIndex))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayMaxDistance) &&
                hit.collider.TryGetComponent(out LevelTransition level))
            {
                _uIDocument.enabled = true;

                VisualElement root = _uIDocument.rootVisualElement;

                VisualElement levelPhoto = root.Q<VisualElement>(className: _levelPhotoClassName);

                Label levelName = root.Q<Label>(className: _levelNameClassName);
                Label levelDescrip = root.Q<Label>(className: _levelDescripClassName);

                levelPhoto.style.backgroundImage = new StyleBackground(level.LevelPhoto);

                levelName.text = level.LevelName;
                levelDescrip.text = level.LevelDescrip;

                VisualElement buttonsContainer = root.Q<VisualElement>(className: _buttonsContainerClassName);

                void AddButton(string buttonClassName, string buttonText, Action onClick)
                {
                    Button newButton = new Button();
                    newButton.AddToClassList(buttonClassName);
                    newButton.text = buttonText;

                    newButton.clicked += onClick;

                    buttonsContainer.Add(newButton);
                }

                AddButton(_toLevelButtonClassName, _toLevelButtonText, () => SceneManager.LoadScene(level.SceneName));
                AddButton(_backButtonClassName, _backButtonText, () => _uIDocument.enabled = false);
            }
            else if (_uIDocument.enabled == true)
            {
                Vector2 mousePosition = Input.mousePosition;
                mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);

                if (_uIDocument.rootVisualElement.panel.Pick(mousePosition) == null)
                {
                    _uIDocument.enabled = false;
                }
            }
        }
    }
}
