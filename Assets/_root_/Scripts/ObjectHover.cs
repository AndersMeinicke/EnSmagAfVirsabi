using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    private Material originalMaterial;
    public Material outlineMaterial;
    private GameObject outlineObject;

    private void Start()
    {
        CreateOutlineObject();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.name == "CursorHit")
       {
            Debug.Log("HIT");
            outlineObject.SetActive(true);
     }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cursor")
        {
            outlineObject.SetActive(false);
        }
    }
    private void CreateOutlineObject()
    {
        outlineObject = new GameObject("Outline");
        outlineObject.transform.SetParent(transform);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one;

        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();
        MeshFilter outlineMeshFilter = outlineObject.AddComponent<MeshFilter>();
        outlineMeshFilter.mesh = originalMeshFilter.mesh;

        MeshRenderer originalMeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer outlineMeshRenderer = outlineObject.AddComponent<MeshRenderer>();
        outlineMeshRenderer.material = outlineMaterial;
        outlineMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineMeshRenderer.receiveShadows = false;
        outlineObject.transform.localScale = Vector3.one * 1.05f; // Adjust the value to control the outline thickness


        outlineObject.SetActive(false);
    }
}
