using UnityEngine;
using UnityEngine.Events;

public abstract class AInput : MonoBehaviour
{
    public UnityEvent<Vector2> onClick = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> onMove = new UnityEvent<Vector2>();
    public UnityEvent<Vector2> onLook = new UnityEvent<Vector2>();
    public UnityEvent<float> onScroll = new UnityEvent<float>();
}
