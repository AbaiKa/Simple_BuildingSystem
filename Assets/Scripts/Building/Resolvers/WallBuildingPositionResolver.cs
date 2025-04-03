using UnityEngine;

public class WallBuildingPositionResolver : ABuildingPositionResolver
{
    public override string ID => "Wall";

    public override BuildingPositionProperties GetPositionProperties(BuildingGrid grid, Building building, Ray ray)
    {
        var props = new BuildingPositionProperties();

        var wallPlane = new Plane(Vector3.forward, new Vector3(0, grid.Offset.y, 0));

        if (wallPlane.Raycast(ray, out float position))
        {
            bool available = true;
            Vector3 worldPosition = ray.GetPoint(position);

            int x = Mathf.RoundToInt(worldPosition.x - grid.Offset.x);
            int y = Mathf.RoundToInt(worldPosition.y - grid.Offset.y);

            if (x < 0 || x > grid.Buildings.GetLength(0) - building.Size.x) available = false;
            if (y < 0 || y > grid.Buildings.GetLength(1) - building.Size.y) available = false;

            float depth = grid.Offset.z;

            if (building.AllowBuildingAbove)
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject targetObject = hit.collider.gameObject;

                    if (targetObject.TryGetComponent(out Building target))
                    {
                        depth = target.transform.position.z + (grid.Positive ? target.Size.x : -target.Size.x);
                    }
                }

                if (available && IsPlaceTaken(grid.Buildings, building, x, y))
                {
                    depth = grid.Buildings[x, y].transform.position.z + (grid.Positive ? grid.Buildings[x, y].Size.x : -grid.Buildings[x, y].Size.x);
                }
                else
                {
                    depth = grid.Offset.z;
                }
            }

            props.x = x;
            props.y = y;
            props.isAllowed = available;
            props.position = new Vector3(x + grid.Offset.x, y + grid.Offset.y, depth);
        }

        return props;
    }
}
