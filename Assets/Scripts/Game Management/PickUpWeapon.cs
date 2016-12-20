using UnityEngine;
using System.Collections;

public class PickUpWeapon : MonoBehaviour {

    public int mWeaponId;
    public bool mDestroyItself = false;
    public bool mUnlocked;

	// Use this for initialization
	void Start () {
        mUnlocked = false;

    }
	
	// Update is called once per frame
	void Update () {

        if(mDestroyItself)
        {
            Destroy(gameObject);
        }
	
	}
}
