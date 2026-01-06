using UnityEngine;

public class ARContentSmoothing : MonoBehaviour
{
    [Header("Smoothing Settings")]
    public float positionSmoothTime = 0.1f;  // manja vrijednost → brže reaguje
    public float rotationSmoothTime = 0.1f;  // manja vrijednost → brže reaguje

    private Vector3 positionVelocity;
    private Vector3 rotationVelocity;

    private Vector3 targetPosition;
    private Vector3 targetEulerAngles;

    void Start()
    {
        // početna pozicija i rotacija
        targetPosition = transform.position;
        targetEulerAngles = transform.eulerAngles;
    }

    void LateUpdate()
    {
        // update ciljne pozicije i rotacije (AR engine postavlja transform)
        targetPosition = transform.position;
        targetEulerAngles = transform.eulerAngles;

        // smoothe poziciju
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionVelocity, positionSmoothTime);

        // smoothe rotaciju
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.x = Mathf.SmoothDampAngle(currentEuler.x, targetEulerAngles.x, ref rotationVelocity.x, rotationSmoothTime);
        currentEuler.y = Mathf.SmoothDampAngle(currentEuler.y, targetEulerAngles.y, ref rotationVelocity.y, rotationSmoothTime);
        currentEuler.z = Mathf.SmoothDampAngle(currentEuler.z, targetEulerAngles.z, ref rotationVelocity.z, rotationSmoothTime);

        transform.eulerAngles = currentEuler;
    }
}
