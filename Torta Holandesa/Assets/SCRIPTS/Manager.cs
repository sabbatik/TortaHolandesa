using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject[] _levelToLoad;
    [SerializeField] int _levelID;
    // I wanna talk to your manager
    
    public void SetID(int id)
    {
        _levelID = id;
    }

    public void LoadLevel(int levelID)
    {
        _levelToLoad[levelID].SetActive(true);
    }

    public void EndLevel()
    {
        _levelToLoad[_levelID].SetActive(false);
    }
}
