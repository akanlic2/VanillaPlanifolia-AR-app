using UnityEngine;
using UnityEngine.InputSystem;

public class PerfumeInteraction : MonoBehaviour
{
    public AudioSource spraySound;

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
            if (spraySound != null)
                spraySound.Play();

            UIManager.Instance.ShowPerfume();
        }
    }
}
