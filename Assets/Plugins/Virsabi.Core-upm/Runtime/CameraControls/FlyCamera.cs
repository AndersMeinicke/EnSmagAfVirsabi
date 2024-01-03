using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class FlyCamera : MonoBehaviour
{
    private const string hBufferProperty = "CurrentHeightBuffer";
    private const string dBufferProperty = "CurrentDistanceBuffer";
    private const string aBufferProperty = "AngleRotationBuffer";
    
    public Transform target;
    [SerializeField] private ViewCameraSettings settings;

    //todo: can be further decoupled with events
    [SerializeField] private CameraControls[] distanceControls;
    [SerializeField] private CameraControls[] angleRotationControls;
    [SerializeField] private CameraControls[] heightControls;

    public delegate void ReceiveInput(bool blocked);
    public event ReceiveInput OnReceiveInput;


    private float distance;

    private Quaternion rot; // todo: appears always before the rotation buffer, not needed
    
    // buffer vars that get affected until finally passed to the transform
    private Quaternion rotationBuffer;
    private Vector2 angleRotationBuffer; // xdeg, ydeg
    
    private Vector3 positionBuffer;
    private Vector3 targetPositionBuffer;
    private float currentDistanceBuffer;
    
    [Header("Min position of panning. Should be around the base")]
    [SerializeField] private float minPosition;
    private float maxPosition;

    // properties for the buffers
    public Vector2 AngleRotationBuffer
    {
        get { return angleRotationBuffer; }
        set { angleRotationBuffer = value; }
    }

    public float CurrentDistanceBuffer
    {
        get { return currentDistanceBuffer; }
        set { currentDistanceBuffer = value; }
    }

    public float CurrentHeightBuffer
    {
        get { return currentHeightBuffer; }
        set { currentHeightBuffer = value; }
    }

    public float MinimumPosition
    {
        get => minPosition;
        set => minPosition = value;
    }

    public float MaximumPosition
    {
        get => maxPosition;
        set => maxPosition = value;
    }


    private float currentHeightBuffer;


    [SerializeField] private bool useRectTransformArea;
    [Header("Mouse input over a specified area")] 
    [SerializeField] private GameObject g_rectTransform;
    private RectTransform rectTransform;

    [SerializeField] private bool useBoundsChecker;
    [Tooltip("Set a bounds checker to filter the camera's position")]
    [SerializeField]  private CameraPositionBounds cameraPositionBoundsChecker; 

    private bool mouseInRectangle, inputRectTransformBlocked;
    private int rectTransformID;
    
    private void Awake()
    {
        if (useRectTransformArea)
        {
            rectTransform = g_rectTransform.GetComponent<RectTransform>();
            rectTransformID = g_rectTransform.GetInstanceID();
        }
        // for checking raycasts
    }

    

    void Start()
    {
        Init();
    }

    
    
    
    

    /// <summary>
    /// Utility method to force position buffer update externally (like from an event that changes the max/min height)
    /// </summary>
    public void UpdateCurrentPositionBuffer()
    {
        // update current position
        currentHeightBuffer = Mathf.Clamp(currentHeightBuffer, minPosition, maxPosition);
        targetPositionBuffer.y = currentHeightBuffer;
        
        target.position = targetPositionBuffer;
        positionBuffer = targetPositionBuffer - (rotationBuffer * Vector3.forward * currentDistanceBuffer + settings.targetOffset); 
    }

    private void Update()
    {
        // input block
        if (useRectTransformArea)
        {
            inputRectTransformBlocked = RaycastForRectTransform();
            mouseInRectangle = IsMouseInRectangle();
        }
        
        
        OnReceiveInput?.Invoke(!inputRectTransformBlocked);

        // assign final values
        transform.position = positionBuffer;
        transform.rotation = rotationBuffer;
    }
    
    
    void LateUpdate()
    {

        // distance movement controls
        for (int i = 0; i < heightControls.Length; i++)
        {
            if (heightControls[i].IsActive())
            {
                ApplyControlMovement(heightControls[i], hBufferProperty);
            }
        }

        // distance movement controls
        for (int i = 0; i < distanceControls.Length; i++)
        {
            if (distanceControls[i].IsActive())
            {
                ApplyControlMovement(distanceControls[i], dBufferProperty);
            }
        }
        
        for (int i = 0; i < angleRotationControls.Length; i++)
        {
            if (angleRotationControls[i].IsActive())
            {
                ApplyControlMovement(angleRotationControls[i], aBufferProperty);
            }
        }
    }


    public void Init()
    {
        // allocates garbage when created every frame in RaycastForRectTransform below, done here only
        pointerEventData = new PointerEventData(EventSystem.current);
        currentDistanceBuffer = settings.startingDistance;
        targetPositionBuffer = target.position;
        
        targetPositionBuffer.y = currentHeightBuffer = settings.startingHeight;
        
        angleRotationBuffer = new Vector2(settings.xDegreesInitial, settings.yDegreesInitial);
        rot = Quaternion.Euler(angleRotationBuffer.y, angleRotationBuffer.x, 0);
        rotationBuffer = Quaternion.Euler(new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0f));
        positionBuffer = targetPositionBuffer - (rotationBuffer * Vector3.forward * currentDistanceBuffer + settings.targetOffset);
        
        transform.position = positionBuffer;
        transform.rotation = rotationBuffer;
        
    }
    
    #region Additional
    // raycasting
    private PointerEventData pointerEventData;
    List<RaycastResult> results = new List<RaycastResult>();
    private bool RaycastForRectTransform()
    {
        // raycast and check if the rectTransform is the first raycast
        pointerEventData.position = Input.mousePosition;
        results.Clear();
        EventSystem.current.RaycastAll(pointerEventData, results);

        if (results.Count > 0 && results[0].gameObject.GetInstanceID().Equals(rectTransformID))
            return false;
        
        return true;
    }

    private Vector2 localMousePos; // mouse position relative to the rectTransform used for updating input
    private bool IsMouseInRectangle()
    {
        localMousePos = rectTransform.InverseTransformPoint(Input.mousePosition);
        if (rectTransform.rect.Contains(localMousePos))
            return true;
        
        return false;
    }
    
    #endregion

    // unused
    public void SwitchSettings(ViewCameraSettings newSettings, Transform lookedTransform)
    {
        target = lookedTransform;
        settings = newSettings;
    }

    private void CalculatePosition()
    {
        targetPositionBuffer.y = currentHeightBuffer;
        currentHeightBuffer = Mathf.Clamp(currentHeightBuffer, minPosition, maxPosition);

        /*position calculation*/ rot = Quaternion.Euler(angleRotationBuffer.y, angleRotationBuffer.x, 0);
        /*position calculation*/ rotationBuffer = Quaternion.Euler(new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, 0f));
        /*position calculation*/ positionBuffer = targetPositionBuffer - (rotationBuffer * Vector3.forward * currentDistanceBuffer + settings.targetOffset);
    }

    private void ApplyControlMovement(CameraControls control, string bufferPropertyName)
    {
        control.MoveCamera(bufferPropertyName);
        CalculatePosition();

        if (useBoundsChecker)
        {
            control.Filter(cameraPositionBoundsChecker.IsPointInBounds(positionBuffer), bufferPropertyName);
            CalculatePosition();
        }
        else
        {
            control.Filter(true, bufferPropertyName);
        }
        
    }
    
}