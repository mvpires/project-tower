using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float mSmooth = 1.5F;
    public Transform mPlayerTransform;
    private Vector3 mRelCameraPos;
    private float mRelCameraPosMag;
    private Vector3 mCameraNewPos;

    void Awake()
    {
        mRelCameraPos = transform.position - mPlayerTransform.position;
        mRelCameraPosMag = mRelCameraPos.magnitude - 0.5F;
        
    }

    void FixedUpdate()
    {
        Vector3 standardPos = mPlayerTransform.position + mRelCameraPos;
        Vector3 abovePos = mPlayerTransform.position + Vector3.up * mRelCameraPosMag;
        Vector3[] checkPoints = new Vector3[5];
        checkPoints[0] = standardPos;
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25F);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.50F);
        //checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75F);
        checkPoints[3] = abovePos;

        for (int i = 0; i < checkPoints.Length; i++)
        {
            if(ViewingPosCheck(checkPoints[i]))
            {
                break;
            }
        }

        transform.position = Vector3.Lerp(transform.position, mCameraNewPos, mSmooth * Time.deltaTime);
        //SmoothLookAt();
    }

    bool ViewingPosCheck(Vector3 checkPos)
    {
        RaycastHit hit;

        if(Physics.Raycast(checkPos, mPlayerTransform.position - checkPos, out hit, mRelCameraPosMag))
        {
            if(hit.transform != mPlayerTransform && hit.collider.gameObject.tag != "Skeleton" && hit.collider.gameObject.tag != "PickUpSkull")
            {
                return false;
            }
        }

        mCameraNewPos = checkPos;

        return true;
    }

    void SmoothLookAt()
    {
        Vector3 relPlayerPos = mPlayerTransform.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, mSmooth * Time.deltaTime);
    }

}
