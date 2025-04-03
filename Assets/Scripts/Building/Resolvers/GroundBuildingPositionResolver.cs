using UnityEngine;

public class GroundBuildingPositionResolver : ABuildingPositionResolver
{
    public override string ID => "Ground";

    public override BuildingPositionProperties GetPositionProperties(BuildingGrid grid, Building building, Ray ray)
    {
        var props = new BuildingPositionProperties();

        var groundPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));

        if (groundPlane.Raycast(ray, out float position))
        {
            bool available = true;
            Vector3 worldPosition = ray.GetPoint(position);

            int x = Mathf.RoundToInt(worldPosition.x - grid.Offset.x);
            int y = Mathf.RoundToInt(worldPosition.z - grid.Offset.y);

            if (x < 0 || x > grid.Buildings.GetLength(0) - building.Size.x) available = false;
            if (y < 0 || y > grid.Buildings.GetLength(1) - building.Size.y) available = false;

            float height = grid.Offset.z;

            if (building.AllowBuildingAbove)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject targetObject = hit.collider.gameObject;

                    if (targetObject.TryGetComponent(out Building target))
                    {
                        height = target.transform.position.y + target.Size.y;
                    }
                }

                if (available && IsPlaceTaken(grid.Buildings, building, x, y))
                {
                    height = grid.Buildings[x, y].transform.position.y + grid.Buildings[x, y].Size.y;
                }
                else
                {
                    height = grid.Offset.z;
                }
            }

            props.x = x;
            props.y = y;
            props.isAllowed = available;
            props.position = new Vector3(x + grid.Offset.x, height + grid.Offset.z, y + grid.Offset.y);
        }

        return props;
    }
}
