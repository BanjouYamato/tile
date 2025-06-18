using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    State _state;
    public State state => _state;
    public static event Action OnGameOver;
    public static event Action OnStart;
    public static event Action OnReady;
    public static GameState Instance {  get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        SelectState(State.ready);
    }

    public void SelectState(State newState)
    {
        switch (newState)
        {
            case State.ready:
                OnReady.Invoke();
                break;
            case State.play:
                OnPlayHandle(); break;
            case State.gameover:
                OnGameOverHandle(); break;
        }
        _state = newState;
    }
    void OnPlayHandle()
    {
        OnStart.Invoke();
    }
    void OnGameOverHandle()
    {
        OnGameOver.Invoke();
    }
}
public enum State
{
    ready,
    play,
    gameover
}
