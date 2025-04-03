using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementComponent;

    private AInput input;
    public void Init(AInput input)
    {
        this.input = input;
        movementComponent.Init();
        input.onMove.AddListener(movementComponent.Move);
        input.onLook.AddListener(movementComponent.Rotate);
    }
}
