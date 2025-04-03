using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [field: SerializeField] public Vector2Int Size {  get; private set; }
    [field: SerializeField] public Vector3 Offset { get; private set; }
    [field: SerializeField] public bool Positive { get; private set; }
    [field: SerializeField] public string ResolverID { get; private set; }
    [SerializeField] private bool horizontal;

    public Building[,] Buildings { get; private set; }
    private void Start()
    {
        Buildings = new Building[Size.x, Size.y];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        float offsetX = Offset.x - 0.5f;
        float offsetY = horizontal ? Offset.z : Offset.y - 0.5f;
        float offsetZ = horizontal ? Offset.y - 0.5f : Offset.z;

        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

        for (int x = 0; x <= Size.x; x++)
        {
            Vector3 start = new Vector3(x, 0, 0) + offset;
            Vector3 end = new Vector3(x, horizontal ? 0 : Size.y, horizontal ? Size.y : 0) + offset;
            Gizmos.DrawLine(start, end);
        }
        for (int y = 0; y <= Size.y; y++)
        {
            Vector3 start = new Vector3(0, horizontal ? 0 : y, horizontal ? y : 0) + offset;
            Vector3 end = new Vector3(Size.x, horizontal ? 0 : y, horizontal ? y : 0) + offset;
            Gizmos.DrawLine(start, end);
        }
    }
}
