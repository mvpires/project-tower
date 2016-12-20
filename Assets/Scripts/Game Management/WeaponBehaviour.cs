using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour {

    public int mDamagePoints = 0;
    public int mWeaponId = 0;
    public bool mUnlocked;
    public GameObject mWeaponGameObject;


    // Use this for initialization
    void Start ()
    {
        mWeaponGameObject = gameObject;
        mUnlocked = false;       
        mWeaponId = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {

        if (collider.GetType() == typeof(CapsuleCollider) && collider.gameObject.tag == "Skeleton")
        {
            collider.gameObject.GetComponent<SkeletonBehaviour>().Health -= 2;
            Debug.Log("Skeleton hit");
        }
    }

}

   

