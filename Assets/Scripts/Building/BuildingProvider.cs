using UnityEngine;

public class BuildingProvider : MonoBehaviour
{
    // “ут мог быть Inject
    [SerializeField] private Building prefab;
    [SerializeField] private BuildingSystem buildingSystem;
    private void OnMouseDown()
    {
        buildingSystem.StartBuilding(prefab);
    }
}
