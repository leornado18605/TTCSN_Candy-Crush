using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Curved2DController : MonoBehaviour
{
    [SerializeField] float curvature = 0.003f;      // 0.001–0.01 là đẹp
    [SerializeField] float centerY = 0f;            // tâm uốn theo local Y (pivot)
    [SerializeField] float radius = 10f;            // dùng khi bật cylindrical
    [SerializeField] bool cylindrical = false;

    Renderer r;
    static readonly int CurvID = Shader.PropertyToID("_Curvature");
    static readonly int CenterID = Shader.PropertyToID("_CenterY");
    static readonly int RadiusID = Shader.PropertyToID("_Radius");

    void Awake() { r = GetComponent<Renderer>(); Apply(); }
    void OnValidate() { if (!r) r = GetComponent<Renderer>(); Apply(); }

    void Apply()
    {
        if (!r || !r.sharedMaterial) return;
        var m = r.sharedMaterial;
        m.SetFloat(CurvID, curvature);
        m.SetFloat(CenterID, centerY);
        m.SetFloat(RadiusID, Mathf.Max(0.001f, radius));
        if (cylindrical) m.EnableKeyword("_CYLINDRICAL_ON");
        else m.DisableKeyword("_CYLINDRICAL_ON");
    }
}
