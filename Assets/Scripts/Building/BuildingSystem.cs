using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private LayerMask gridMask;
    [SerializeField, Range(0, 10)] private float gridTargetingRange;

    private Dictionary<string, ABuildingPositionResolver> resolvers = new Dictionary<string, ABuildingPositionResolver>();

    private AInput input;

    private Building current;
    private BuildingGrid currentGrid;
    private ABuildingPositionResolver currentResolver;
    private BuildingPositionProperties currentProperties;
    public void Init(AInput input)
    {
        this.input = input;
        var ground = new GroundBuildingPositionResolver();
        var wall = new WallBuildingPositionResolver();
        resolvers.Add(ground.ID, ground);
        resolvers.Add(wall.ID, wall);

        input.onClick.AddListener(OnClick);
    }
    private void Update()
    {
        if (current != null)
        {
            currentGrid = GetGrid();
            if (currentGrid != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var props = GetProps(currentGrid, current, ray);

                current.transform.position = props.position;
                current.SetTransparent(props.isAllowed);

                currentProperties = props;
            }
        }
    }

    private void OnClick(Vector2 position)
    {
        if (current != null)
        {
            FinishBuilding(currentProperties.x, currentProperties.y);
        }
    }
    public void StartBuilding(Building buildingPrefab)
    {
        if (current != null)
        {
            Destroy(current.gameObject);
        }

        current = Instantiate(buildingPrefab);
        current.Init();

        if (resolvers.ContainsKey(current.ResolverID))
        {
            currentResolver = resolvers[current.ResolverID];
        }
        else
        {
            currentResolver = null;
        }
    }
    private void FinishBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < current.Size.x; x++)
        {
            for (int y = 0; y < current.Size.y; y++)
            {
                currentGrid.Buildings[placeX + x, placeY + y] = current;
            }
        }

        current.SetNormal();
        current = null;
    }
    private BuildingPositionProperties GetProps(BuildingGrid grid, Building building, Ray ray)
    {
        var props = new BuildingPositionProperties();
        if (currentResolver != null)
        {
            currentResolver.GetPositionProperties(grid, building, ray);
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            props.isAllowed = false;
            props.position = targetPosition;
        }

        return props;
    }
    private BuildingGrid GetGrid()
    {
        var grids = Physics.OverlapSphere(transform.position, gridTargetingRange, gridMask);

        BuildingGrid closestGrid = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in grids)
        {
            if (collider.TryGetComponent(out BuildingGrid grid) && currentResolver.ID == grid.ResolverID)
            {
                float distance = Vector3.Distance(transform.position, grid.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGrid = grid;
                }
            }
        }

        return closestGrid;
    }
}
