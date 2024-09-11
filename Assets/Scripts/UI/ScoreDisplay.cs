using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private ScoreKeeper _scoreKeeper;
    [SerializeField]
    private RollTextUp _rollUp;

    private void OnEnable()
    {
        _rollUp.StartValue = _scoreKeeper.Score;
        _rollUp.EndValue = _scoreKeeper.Score;
        _rollUp.CurrentValue = _scoreKeeper.Score;
        _scoreKeeper.OnScoreChange += UpdateScoreText;
    }

    private void OnDisable()
    {
        _scoreKeeper.OnScoreChange -= UpdateScoreText;
    }

    private void UpdateScoreText(ScoreKeeper scoreKeeper, int oldScore)
    {
        _rollUp.StartValue = oldScore;
        _rollUp.EndValue = _scoreKeeper.Score;
    }
}
