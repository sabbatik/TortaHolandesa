using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fruit : MonoBehaviour
{   
    [SerializeField] Vector2[] _spawnpoints;
    int _fruitCount = 0;
    float _currentTime = 0f;
    public float _maxFruitTime;
    public Text _timerText;
    public Text _fruitCountText;
    public int _maxFruits;

    [SerializeField] GameEvent _onLost;
    [SerializeField] GameEvent _onWin;

    int _lastNumber;
    int GetRandom (int min, int max)
    {
        int rand = Random.Range (min, max);
        while (rand == _lastNumber)
        rand = Random.Range (min, max);
        _lastNumber = rand;
        return rand;
    }

    void Start()
    {
        _currentTime = _maxFruitTime;
        transform.position = _spawnpoints[GetRandom(0, _spawnpoints.Length)];
    }

    void Update()
    {
        if(_fruitCount<=19) _currentTime -= 1 * Time.deltaTime;
        _timerText.text = _currentTime.ToString("0");
        _fruitCountText.text = _fruitCount.ToString() + " / " + _maxFruits.ToString();

        if(_currentTime <= 0)
        {
            if(_fruitCount == 0)
            {
                _currentTime = _maxFruitTime;
                transform.position = _spawnpoints[GetRandom(0, _spawnpoints.Length)];
            }
            else if(_fruitCount == _maxFruits)
            {
                _fruitCount = _maxFruits;
                _currentTime = 0f;
            }
            else
            {
                _onLost?.Invoke();
                _currentTime = 0f;
            }
        }
    }

    public void FruitTouched()
    {
        switch(_fruitCount)
        {
            case 0 :
                _fruitCount += 1;
                transform.position = _spawnpoints[GetRandom(0, _spawnpoints.Length)];
                _currentTime = _maxFruitTime;
                break;
            case var value when value == _maxFruits - 1:
                _onWin?.Invoke();
                _fruitCount = _maxFruits;
                _fruitCountText.text = _maxFruits.ToString();
                break;
            default:
                _fruitCount += 1;
                transform.position = _spawnpoints[GetRandom(0, _spawnpoints.Length)];
                _currentTime = _maxFruitTime;
                break;
        }
    }

    public void Restart()
    {
        _fruitCount = 0;
        _currentTime = _maxFruitTime;
    }
}