
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    private PlayerController _player;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    private float _score;
    [SerializeField]
    private TextMeshProUGUI _distanceText;
    [SerializeField]
    private GameObject _gameOverPanel;
    
    // Update is called once per frame
    void Update()
    {
        if(!_player.HasCrashed)
        {
            if (!_player.TutorialActive)
            {
                _score += Time.deltaTime * _player.Speed;
                _scoreText.text = String.Format("{0:0}", _score);
                _distanceText.text = String.Format("{0:0}", _player.GetDistanceTravelled()) + " m";
                Debug.Log(_score);
            }
        }
        else
        {
            _gameOverPanel.SetActive(true);
        }
    }
}
