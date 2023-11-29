using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] private Button _optionsButton;

    [SerializeField] private Button _quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        _optionsButton.onClick.AddListener(OnOptionsButtonClick);
        _quitGameButton.onClick.AddListener(OnQuitGameButtonClick);
    }


    private void OnOptionsButtonClick()
    {
        // show a slider to modify mouse sensitivity and sound volume



    }

    private void OnQuitGameButtonClick()
    {
        // load main menu
        Time.timeScale = 1f;
        ScenesManager.Instance.LoadMainMenu();
    }
}
