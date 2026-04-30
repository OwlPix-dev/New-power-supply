using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelMenuUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private ButtonsContainerData _buttonsContainerData;

    [SerializeField] private string _levelPhotoClassName;
    [SerializeField] private string _levelNameClassName;
    [SerializeField] private string _levelDescripClassName;

    [SerializeField] private int[] _closeLevelInfoMouseIndices = new int[3] { 0, 1, 2 };

    private LevelMenuInfo _currentLevelInfo;

    private void Update()
    {
        if (_currentLevelInfo == null) { return; }

        foreach (int closeLevelInfoMouseIndex in _closeLevelInfoMouseIndices)
        {
            if (Input.GetMouseButtonDown(closeLevelInfoMouseIndex))
            {
                Vector2 mousePosition = Input.mousePosition;
                mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);

                if (_uIDocument.rootVisualElement.panel.Pick(mousePosition) == null)
                {
                    CloseLevelInfo();
                }
            }
        }
    }

    public void OpenLevelInfo(LevelMenuInfo levelInfo)
    {
        _uIDocument.enabled = true;
        _currentLevelInfo = levelInfo;

        VisualElement root = _uIDocument.rootVisualElement;

        VisualElement levelPhoto = root.Q<VisualElement>(className: _levelPhotoClassName);

        Label levelName = root.Q<Label>(className: _levelNameClassName);
        Label levelDescrip = root.Q<Label>(className: _levelDescripClassName);

        levelPhoto.style.backgroundImage = new StyleBackground(_currentLevelInfo.LevelPhoto);

        levelName.text = _currentLevelInfo.LevelName;
        levelDescrip.text = _currentLevelInfo.LevelDescrip;

        _buttonsContainerData.DrawButtons(root);
    }

    public void CloseLevelInfo()
    {
        _uIDocument.enabled = false;
        _currentLevelInfo = null;
    }

    public void ToLevelButtonClick()
    {
        SceneManager.LoadScene(_currentLevelInfo.SceneName);
    }
}
