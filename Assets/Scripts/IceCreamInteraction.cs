using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class IceCreamInteraction : MonoBehaviour
{
    public VideoClip videoClip;

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 pos = Pointer.current.position.ReadValue();
            CheckHit(pos);
        }
    }

    void CheckHit(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            UIManager.Instance.ShowIceCream(videoClip);
        }
    }
}
