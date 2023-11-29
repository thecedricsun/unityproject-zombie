using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    // start game
    [SerializeField] private Button _startGameButton;
    // quit game
    [SerializeField] private Button _quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Menu");
        _startGameButton.onClick.AddListener(OnStartGameButtonClick);
        _quitGameButton.onClick.AddListener(OnQuitGameButtonClick);
    }

    private void OnStartGameButtonClick()
    {
        ScenesManager.Instance.LoadNewGame();
    }

    private void OnQuitGameButtonClick()
    {
        QuitGame();
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
