using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CameraPickLevel : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private ButtonsContainerData _buttonsContainerData;

    [SerializeField] private string _levelPhotoClassName;
    [SerializeField] private string _levelNameClassName;
    [SerializeField] private string _levelDescripClassName;

    [SerializeField] private int _mouseIndex = 0;
    [SerializeField] private float _rayMaxDistance = 100f;

    private LevelTransition _currentLevel;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseIndex))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayMaxDistance) &&
                hit.collider.TryGetComponent(out LevelTransition level))
            {
                _uIDocument.enabled = true;
                _currentLevel = level;

                VisualElement root = _uIDocument.rootVisualElement;

                VisualElement levelPhoto = root.Q<VisualElement>(className: _levelPhotoClassName);

                Label levelName = root.Q<Label>(className: _levelNameClassName);
                Label levelDescrip = root.Q<Label>(className: _levelDescripClassName);

                levelPhoto.style.backgroundImage = new StyleBackground(_currentLevel.LevelPhoto);

                levelName.text = _currentLevel.LevelName;
                levelDescrip.text = _currentLevel.LevelDescrip;

                _buttonsContainerData.DrawButtons(root);
            }
            else if (_uIDocument.enabled == true)
            {
                Vector2 mousePosition = Input.mousePosition;
                mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);

                if (_uIDocument.rootVisualElement.panel.Pick(mousePosition) == null)
                {
                    CloseUI();
                }
            }
        }
    }

    public void CloseUI()
    {
        _uIDocument.enabled = false;
        _currentLevel = null;
    }

    public void ToLevelButtonClick()
    {
        SceneManager.LoadScene(_currentLevel.SceneName);
    }
}
