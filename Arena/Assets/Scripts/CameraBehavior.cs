using Unity.VisualScripting;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Vector3 camOffset = new Vector3(0f,1.2f,-2.6f);
    private Transform target;
    public RaycastHit hit;

    void Start()
    {
        target = GameObject.Find("Player").transform;
    }
    void LateUpdate()
    {
        Debug.DrawLine(this.transform.position,target.position,Color.red);
        WallAlpha();
        this.transform.position = target.TransformPoint(camOffset);
        this.transform.LookAt(target);
    }
    private GameObject lastHitObject;
    void WallAlpha()
    {
        if (Physics.Linecast(target.transform.position,this.transform.position,out hit))
        {
            GameObject currentHitObject=hit.collider.gameObject;
            if (currentHitObject != lastHitObject)
            {
                Debug.Log("Set transparent to:"+currentHitObject);
                Material matC = currentHitObject.GetComponent<Renderer>().material;
                matC.color = new Color(matC.color.r,matC.color.g,matC.color.b,0.5f);
                if (lastHitObject != null)
                {
                    Debug.Log("Revert transparent:"+lastHitObject);
                    Material matO = lastHitObject.GetComponent<Renderer>().material;
                    matO.color = new Color(matO.color.r,matO.color.g,matO.color.b,1);
                }
                lastHitObject = currentHitObject;
            }
        }
        else
        {
            if (lastHitObject != null)
            {
                if (lastHitObject.GetComponent<Renderer>().material.color.a != 1)
                {
                    Debug.Log("Revert transparent:"+lastHitObject);
                Material matO = lastHitObject.GetComponent<Renderer>().material;
                matO.color = new Color(matO.color.r,matO.color.g,matO.color.b,1);
                }
            }
            lastHitObject = null;
        }
    }
}
