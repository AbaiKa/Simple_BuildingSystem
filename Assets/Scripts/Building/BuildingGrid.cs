using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Size {  get; private set; }
    [field: SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public bool Positive { get; private set; }
    [field: SerializeField] public string ResolverID { get; private set; }

    public Building[,] Buildings { get; private set; }
    private void Start()
    {
        Buildings = new Building[Size.x, Size.y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float offsetX = Offset.x - 0.5f;
        float offsetY = Offset.z;
        float offsetZ = Offset.y - 0.5f;

        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

        Vector3 transformedOffset = transform.TransformPoint(offset);

        for (int x = 0; x <= Size.x; x++)
        {
            Vector3 start = transform.TransformPoint(new Vector3(x, 0, 0) + offset);
            Vector3 end = transform.TransformPoint(new Vector3(x, 0, Size.y) + offset);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= Size.y; y++)
        {
            Vector3 start = transform.TransformPoint(new Vector3(0, 0, y) + offset);
            Vector3 end = transform.TransformPoint(new Vector3(Size.x, 0, y) + offset);
            Gizmos.DrawLine(start, end);
        }
    }

}
