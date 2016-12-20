using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour {

    public int mDamagePoints = 0;
    public int mWeaponId = 0;
    public bool mUnlocked;


    // Use this for initialization
    void Start()
    {
        mUnlocked = false;
        mWeaponId = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.tag == "Player")
        {

            collider.gameObject.GetComponent<PlayerBehaviour>().playerHealth -= 10;
            Debug.Log("Player hit");
        }
    }

}
