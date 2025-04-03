using UnityEngine;

public abstract class ABuildingPositionResolver
{
    public abstract string ID { get; }
    public abstract BuildingPositionProperties GetPositionProperties(BuildingGrid grid, Building building, Ray ray);

    protected bool IsPlaceTaken(Building[,] grid, Building building, int placeX, int placeY)
    {
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

public class BuildingPositionProperties
{
    public int x;
    public int y;
    public bool isAllowed;
    public Vector3 position;
}