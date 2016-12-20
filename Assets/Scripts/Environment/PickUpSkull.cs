using UnityEngine;
using System.Collections;

public class PickUpSkull : MonoBehaviour {

    public int mSkullId = 0;
    public string mSkullColor;
    public bool mDestroyItself = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	    if(mDestroyItself)
        {
            Destroy(gameObject);
        }

        Hoover();

    }

    public void ShowUp()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    public void HideItself()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void Hoover()
    {
        
    }
}
