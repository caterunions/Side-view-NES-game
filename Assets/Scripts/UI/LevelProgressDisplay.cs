using UnityEngine;
using UnityEngine.UI;

public class LevelProgressDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _progressBar;

    [SerializeField]
    private LevelManager _levelManager;

    private void OnEnable()
    {
        _progressBar.fillAmount = 0;
        _levelManager.OnLevelScoreChanged += HandleScoreChanged;
    }

    private void OnDisable()
    {
        _levelManager.OnLevelScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(LevelManager lvlManager, int newScore)
    {
        _progressBar.fillAmount = (float)newScore / (float)_levelManager.LevelData.TotalLevelScore;
    }
}
