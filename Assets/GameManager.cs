using Runtime.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] UnityEvent gameStart;
    private void OnEnable()
    {

    }

    [ContextMenu("GameStart!")]
    public void OnGameStart()
    {
        gameStart?.Invoke();
    }
}
