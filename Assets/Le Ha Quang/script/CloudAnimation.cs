using UnityEngine;

public class CloudAnimation : MonoBehaviour
{
    [SerializeField] float transitionSpeed;
    [SerializeField] float transitionDistance;
    Vector3 startPoint;
    Vector3 endPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        startPoint = this.gameObject.transform.position;
        Vector3 transitionPoint = new Vector3(transitionDistance, 0, 0);
        endPoint = startPoint + transitionPoint;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * transitionSpeed, 1);
        transform.position = Vector3.Lerp(startPoint, endPoint, t);
    }
}
