using UnityEngine;

public class WallBuildingPositionResolver : ABuildingPositionResolver
{
    public override string ID => "Wall";

    public override BuildingPositionProperties GetPositionProperties(BuildingGrid grid, Building building, Ray ray)
    {
        var props = new BuildingPositionProperties();

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            bool available = true;
            Vector3 worldPosition = hit.point;

            int x = Mathf.RoundToInt(worldPosition.x - grid.Offset.x);
            int y = Mathf.RoundToInt(worldPosition.y - grid.Offset.y);

            if (x < 0 || x > grid.Buildings.GetLength(0) - building.Size.x) available = false;
            if (y < 0 || y > grid.Buildings.GetLength(1) - building.Size.y) available = false;

            float depth = grid.Offset.z;

            if (building.AllowBuildingAbove)
            {
                RaycastHit additionalHit;

                if (Physics.Raycast(ray, out additionalHit))
                {
                    GameObject targetObject = additionalHit.collider.gameObject;

                    if (targetObject.TryGetComponent(out Building target))
                    {
                        depth = grid.Buildings[x, y].transform.position.z + (grid.Positive ? grid.Buildings[x, y].Size.x : -grid.Buildings[x, y].Size.x);
                    }
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
