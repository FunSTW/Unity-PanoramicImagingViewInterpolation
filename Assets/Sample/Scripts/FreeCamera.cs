// By FunSTW, 2022, https://github.com/FunSTW/Unity/
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [Header("Drag")]
    public float sensitivity = 5.0f;

    public float maxYAngle = 80f;
    public float dragInertia = 0.6f;
    public float inertiaThreshold = 0.0001f;

    [Header("Zoom")]
    public float zoomSpeed = 2.0f;

    public Vector2 FOVClamp = new Vector2(20, 120);
    [Range(0, 1)] public float zoomSmooth = 0.1f;

    [Header("Other")]
    public bool sensitivityMulZoom = true;

    private Vector2 currentRotation;
    private Vector2 lastDelta;
    private new Camera camera;
    private Transform camTrans;
    private bool updateInertia;

    private float _currentFOV;
    public float CurrentFOV => _currentFOV;

    private void Awake()
    {
        camera = Camera.main;
        camTrans = camera.transform;

        _currentFOV = camera.fieldOfView;

        currentRotation.x = camTrans.rotation.eulerAngles.y;
        currentRotation.y = camTrans.rotation.eulerAngles.x;
    }

    private void Update()
    {
        UpdateZoom();
        UpdateDrag();
    }

    private void UpdateZoom()
    {
        _currentFOV += -Input.mouseScrollDelta.y * zoomSpeed;
        _currentFOV = Mathf.Clamp(_currentFOV, FOVClamp.x, FOVClamp.y);
        if (Mathf.Abs(camera.fieldOfView - _currentFOV) > 0.01f)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, _currentFOV, zoomSmooth);
        }
    }

    private void UpdateDrag()
    {
        updateInertia = lastDelta.x * lastDelta.x + lastDelta.y * lastDelta.y > inertiaThreshold;
        if (Input.GetMouseButton(0))
        {
            currentRotation += lastDelta;
            float zoomOffset = (camera.fieldOfView - FOVClamp.x) / (FOVClamp.y - FOVClamp.x);
            zoomOffset = zoomOffset * 0.9f + 0.1f;
            float sen = sensitivityMulZoom ? sensitivity * zoomOffset : sensitivity;
            lastDelta.x = -Input.GetAxis("Mouse X") * sen * Time.deltaTime;
            lastDelta.y = +Input.GetAxis("Mouse Y") * sen * Time.deltaTime;
        }
        else
        {
            if (updateInertia)
            {
                currentRotation += lastDelta;
                lastDelta *= InvPower4(dragInertia);
            }
        }

        if (updateInertia)
        {
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            camTrans.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
    }

    private float InvPower4(float value)
    {
        value = 1 - value;
        value = value * value * value * value;
        return 1 - value;
    }
}