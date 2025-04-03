using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rigidbodyComponent;
    [SerializeField] private Camera cameraComponent;
    [Header("Properties")]
    [SerializeField] private float movementspeed;
    [SerializeField] private float lookSpeed;

    private float pitch;

    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Move(Vector2 direction)
    {
        Vector3 move = transform.right * direction.x + transform.forward * direction.y;
        rigidbodyComponent.MovePosition(rigidbodyComponent.position + move * movementspeed * Time.deltaTime);
    }
    public void Rotate(Vector2 direction)
    {
        transform.Rotate(Vector3.up * direction.x * lookSpeed);
        pitch = Mathf.Clamp(pitch - direction.y, -90f, 90f);
        cameraComponent.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
