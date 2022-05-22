using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public BoxCollider2D mapBounds;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    private float xMin, xMax, yMin, yMax;



    // Start is called before the first frame update
    private void Start()
    {
        var bounds = mapBounds.bounds;
        xMin = bounds.min.x;
        xMax = bounds.max.x;
        yMin = bounds.min.y;
        yMax = bounds.max.y;
    }

    // Update is called once per frame
    private void LateUpdate() {
        Follow();
    }

    private void Follow() {
        if (player) {
          Vector3 playerPosition = player.position + offset;
          Vector3 boundPosition = new Vector3(
              Mathf.Clamp(playerPosition.x, xMin, xMax),
              Mathf.Clamp(playerPosition.y, yMin, yMax),
              Mathf.Clamp(playerPosition.z, offset.z, offset.z)
          );

          Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
          // Debug.Log(xMin);
          transform.position = smoothPosition;
        }
    }
}
