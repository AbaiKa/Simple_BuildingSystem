using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movementComponent;

    [Inject]
    private void Construct(AInput input)
    {
        movementComponent.Init();
        input.onMove.AddListener(movementComponent.Move);
        input.onLook.AddListener(movementComponent.Rotate);
    }
}
