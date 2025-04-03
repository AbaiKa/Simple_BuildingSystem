using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameManager : MonoBehaviour
{
    public UnityEvent<GameState> onStateChanged = new UnityEvent<GameState>();

    private GameState gameState;
    [Inject]
    private void Construct(BuildingSystem buildingSystem)
    {
        buildingSystem.onStart.AddListener(() => { SetState(GameState.Build); });
        buildingSystem.onFinish.AddListener(() => { SetState(GameState.None); });
    }
    public void SetState(GameState state)
    {
        gameState = state;
        onStateChanged?.Invoke(state);
    }
}

public enum GameState
{
    None,
    Build,
}