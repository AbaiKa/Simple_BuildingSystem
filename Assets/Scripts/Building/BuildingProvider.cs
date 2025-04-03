using UnityEngine;
using Zenject;

public class BuildingProvider : MonoBehaviour
{
    [SerializeField] private Building prefab;
    
    private BuildingSystem buildingSystem;
    [Inject]
    private void Construct(BuildingSystem buildingSystem)
    {
        this.buildingSystem = buildingSystem;
    }
    private void OnMouseDown()
    {
        buildingSystem.StartBuilding(prefab);
    }
}
