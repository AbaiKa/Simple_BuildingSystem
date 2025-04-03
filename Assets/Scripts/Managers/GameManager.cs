using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private AInput input;

    private void Awake()
    {
        player.Init(input);
        buildingSystem.Init(input);
    }
}
