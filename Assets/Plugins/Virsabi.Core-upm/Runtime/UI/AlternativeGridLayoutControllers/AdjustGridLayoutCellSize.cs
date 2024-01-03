using MyBox;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is still WIP. For now it is only tested usefull for making a grid that is 1x4 on portrait aspect ratios, and 2x2 in landscape, but with the benefit of filling out the entire screen width.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class AdjustGridLayoutCellSize : MonoBehaviour
{
    public enum Axis { X, Y };
    public enum RatioMode { Free, Fixed };

    [SerializeField] Axis expand;
    [SerializeField] RatioMode ratioMode;
    [SerializeField] float cellRatio = 1;
    [SerializeField] int minSize;

    [SerializeField, HideInInspector]
    new RectTransform transform;
    [SerializeField, HideInInspector] GridLayoutGroup grid;

    [SerializeField, ReadOnly]
    private float contentSize;

    void Awake()
    {
        transform = (RectTransform)base.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCellSize();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateCellSize();
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    void Update()
    {
        UpdateCellSize();
    }
#endif

    void OnValidate()
    {
        grid = GetComponent<GridLayoutGroup>();
        transform = (RectTransform)base.transform;
        grid = GetComponent<GridLayoutGroup>();
        UpdateCellSize();
    }

    void UpdateCellSize()
    {
        var count = grid.constraintCount;
        if (expand == Axis.X)
        {
            float spacing = (count - 1) * grid.spacing.x;
            contentSize = transform.rect.width - grid.padding.left - grid.padding.right - spacing;
            float sizePerCell = contentSize / count;

            Vector2 cellSize = new Vector2(sizePerCell, ratioMode == RatioMode.Free ? grid.cellSize.y : sizePerCell * cellRatio);

            if (contentSize / grid.constraintCount > minSize)
            {
                grid.cellSize = cellSize;
            }
            else
                grid.cellSize = new Vector2(contentSize, ratioMode == RatioMode.Free ? grid.cellSize.y : contentSize * cellRatio);



        }
        else //if (expand == Axis.Y)
        {
            float spacing = (count - 1) * grid.spacing.y;
            float contentSize = transform.rect.height - grid.padding.top - grid.padding.bottom - spacing;
            float sizePerCell = contentSize / count;


            Vector2 cellSize = new Vector2(ratioMode == RatioMode.Free ? grid.cellSize.x : sizePerCell * cellRatio, sizePerCell);

            if (cellSize.y > minSize)
            {
                grid.cellSize = cellSize;
            }
            else
                grid.cellSize = new Vector2(ratioMode == RatioMode.Free ? grid.cellSize.x : sizePerCell * cellRatio, minSize);
        }
    }
}
