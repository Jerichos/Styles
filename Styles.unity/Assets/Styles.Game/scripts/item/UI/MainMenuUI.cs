using System;
using Styles.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Styles.Game
{
public class MainMenuUI : UIPanel
{
    [SerializeField] private Button _buttonExitGame;
    [SerializeField] private Button _buttonRestart;

    private void ExitGame()
    {
        Application.Quit();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        _buttonRestart.onClick.AddListener(Restart);
        _buttonExitGame.onClick.AddListener(ExitGame);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        
        _buttonRestart.onClick.RemoveAllListeners();
        _buttonExitGame.onClick.RemoveAllListeners();
    }
}
}