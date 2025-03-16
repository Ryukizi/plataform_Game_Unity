using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Camera cam;
    public Transform followTarget;
    Vector2 startingPosition;
    float startingZ;
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    float zDistanceFromTarget => transform.position.z - followTarget.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
       Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
