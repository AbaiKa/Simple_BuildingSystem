using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class BuildingSystem : MonoBehaviour
{
    #region Props
    [Header("Grid")]
    [SerializeField] private LayerMask gridMask;
    [SerializeField, Range(0, 10)] private float gridTargetingRange = 7;
    [Header("Building")]
    [SerializeField, Range(0, 90)] private float rotationAngle = 45;
    [SerializeField] private Building prefab;

    public UnityEvent onStart = new UnityEvent();
    public UnityEvent onFinish = new UnityEvent();
    private Dictionary<string, ABuildingPositionResolver> resolvers = new Dictionary<string, ABuildingPositionResolver>();

    private Building current;
    private BuildingGrid currentGrid;
    private ABuildingPositionResolver currentResolver;
    private BuildingPositionProperties currentProperties;
    #endregion
    #region Methods
    #region Unity
    private void Update()
    {
        if (current != null)
        {
            currentGrid = GetGrid();
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            var props = GetProps(currentGrid, current, ray);

            current.transform.position = props.position;
            current.UpdateBuilding(props.x, props.y, props.isAllowed);

            currentProperties = props;
        }
    }
    #endregion
    #region Private
    [Inject]
    private void Construct(AInput input)
    {
        var ground = new GroundBuildingPositionResolver();
        var wall = new WallBuildingPositionResolver();
        resolvers.Add(ground.ID, ground);
        resolvers.Add(wall.ID, wall);

        input.onClick.AddListener(OnClick);
        input.onScroll.AddListener(OnScroll);
    }
    private void OnClick(Vector2 position)
    {
        if (current != null && currentProperties != null)
        {
            if (currentProperties.isAllowed)
            {
                FinishBuilding(currentProperties.x, currentProperties.y);
            }
        }
    }
    private void OnScroll(float scroll)
    {
        if (current != null)
        {
            if (scroll != 0)
            {
                float angle = scroll > 0 ? rotationAngle : -rotationAngle;
                current.transform.Rotate(0, angle, 0, Space.World);
            }
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

        onStart?.Invoke();
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

        onFinish?.Invoke();
    }

    #region Get
    private BuildingPositionProperties GetProps(BuildingGrid grid, Building building, Ray ray)
    {
        var props = new BuildingPositionProperties();
        if (grid != null && currentResolver != null)
        {
            props = currentResolver.GetPositionProperties(grid, building, ray);
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 4f;
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
    #endregion
    #endregion
    #endregion
}
