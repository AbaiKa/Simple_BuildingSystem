using UnityEngine;

public class DesktopInput : AInput
{
    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;

        onMove?.Invoke(new Vector2(horizontal, vertical));

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        onLook?.Invoke(new Vector2(mouseX, mouseY));

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        onScroll?.Invoke(scrollInput);

        if (Input.GetMouseButtonDown(0))
        {
            onClick?.Invoke(Input.mousePosition);
        }
    }
}
