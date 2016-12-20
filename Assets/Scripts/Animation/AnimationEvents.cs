using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour
{

    public GameObject mWeapon;

    void ActivateWeaponCollider()
    {
        mWeapon.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("Activated");
    }

    void DeactivateWeaponCollider()
    {
        mWeapon.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Deactivated");
    }
}
