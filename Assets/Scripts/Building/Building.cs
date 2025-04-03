using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Renderer rendererComponent;
    [SerializeField] private Collider colliderComponent;
    [field: SerializeField] public Vector2Int Size { get; private set; }
    [field: SerializeField] public bool AllowBuildingAbove {  get; private set; }
    [field: SerializeField] public string ResolverID { get; private set; }

    public Vector2Int Position { get; private set; }
    public void Init()
    {
        colliderComponent.enabled = false;
    }
    public void UpdateBuilding(int x, int y, bool available)
    {
        Position = new Vector2Int(x, y);
        if (available)
        {
            rendererComponent.material.color = new Color(0, 1f, 0f, 0.3f);
        }
        else
        {
            rendererComponent.material.color = new Color(1, 0, 0f, 0.3f);
        }
    }
    public void SetNormal()
    {
        rendererComponent.material.color = Color.white;
        colliderComponent.enabled = true;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Gizmos.color = new Color(0, 0.5f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}