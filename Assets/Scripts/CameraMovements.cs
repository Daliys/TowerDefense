using UnityEngine;

/// <summary>
///  Class for camera movements and zooming.
/// </summary>
public class CameraMovements : MonoBehaviour
{
    // This is the borders of the camera. The camera can't go beyond this borders. 
    [SerializeField] private Point zoomBorder;
    [SerializeField] private Point moveBorder;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float moveSpeed;

    private Camera _camera;

    /// <summary>
    ///  This is the starting zoom of the camera. Used to calculate the additional speed of the camera.
    /// </summary>
    private readonly float _startZoom = 5f;


    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    
    // Updating camera position and zoom 
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.y < moveBorder.y)
        {
            transform.position += Vector3.up * (Time.deltaTime * moveSpeed * _camera.orthographicSize) / _startZoom;
        }

        if (Input.GetKey(KeyCode.S) && transform.position.y > -moveBorder.y)
        {
            transform.position += Vector3.down * (Time.deltaTime * moveSpeed * _camera.orthographicSize) / _startZoom;
        }

        if (Input.GetKey(KeyCode.A) && transform.position.x > -moveBorder.x)
        {
            transform.position += Vector3.left * (Time.deltaTime * moveSpeed * _camera.orthographicSize) / _startZoom;
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < moveBorder.x)
        {
            transform.position += Vector3.right * (Time.deltaTime * moveSpeed * _camera.orthographicSize) / _startZoom;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (_camera.orthographicSize < zoomBorder.y)
                _camera.orthographicSize += Time.deltaTime * zoomSpeed;

        }

        if (Input.GetKey(KeyCode.E))
        {
            if (_camera.orthographicSize > zoomBorder.x)
                _camera.orthographicSize -= Time.deltaTime * zoomSpeed;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (_camera.orthographicSize > zoomBorder.x)
                _camera.orthographicSize -= 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (_camera.orthographicSize < zoomBorder.y)
                _camera.orthographicSize += 1;
        }
    }
}
