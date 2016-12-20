using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{

    private Vector3 mMoveDirection = Vector3.zero;
    //public GameObject mExitDoor;
    public PickUpSkull mSkullInHand;
    public WeaponBehaviour[] mWeapons;
    //public PickUpSkull[] mSkulls;
    public List<int> mPickedUpWeapons;
    private Collider mCollider;
    private int mCurrentWeapon = 0;
    private const string PICK_UP_WEAPON_TAG = "PickUpWeapon";
    private const string PICK_UP_SKULL_TAG = "PickUpSkull";
    private const string EXIT_DOOR_TAG = "ExitDoor";
    private const string CRYPT_TAG = "Crypt";
    private bool mSkeletonsActive = false;
    //private Vector3 mExitDoorMoveTowards;
    //private bool mOpenExit = false;
    private bool mHasWeapon = false;
    private Animation mAnimation;
    private Rigidbody mRigidbody;
    public GameObject mWeaponBone;
    public SkeletonBehaviour[] mSkeletons;


    private string mColliderName;
    private string mActiveWeapon;
    //private bool mIsGameOver = false;
    private bool mInTrigger = false;
    private bool mWeaponTrigger = false;
    private bool mSkullTrigger = false;
    private bool mCryptTrigger = false;
    private bool mIsSkullInHand = false;
    private bool mIsAttacking = false;
    private int mSkullsInPlace = 0;
    public float mTurnSmooth = 15f;
    public float mSpeedDampTime = 0.1f;
    public float mMoveSpeed = 13F;
    public float mJumpSpeed = 8.0F;
    public float mGravity = 20.0F;
    public int playerHealth = 100;
    private float camRayLength = 1000f;
    private Vector3 movement;
    private int floorMask;
    private GameManager gameManager;


    // Use this for initialization
    void Awake()
    {
        gameManager = GameManager.Instance;
        //mExitDoorMoveTowards = new Vector3(mExitDoor.transform.position.x, 15.0F, mExitDoor.transform.position.z);
        //gameManager.isGameOver = false;
        mAnimation = GetComponent<Animation>();
        mRigidbody = GetComponent<Rigidbody>();
        mSkullInHand = GetComponentInChildren<PickUpSkull>();
        floorMask = LayerMask.GetMask("Floor");


        mAnimation["Walk"].layer = 1;
        mAnimation["Idle"].layer = 1;
        mAnimation["Attack"].layer = 2;

    }

    void Update()
    {
        if (playerHealth <= 0)
        {
            Death();
        }
        if (!gameManager.isGameOver)
        {
            if (mInTrigger && Input.GetKeyDown(KeyCode.F))
            {
                if (mWeaponTrigger)
                {
                    PickUpWeapon();
                }
                else if (mSkullTrigger)
                {
                    PickUpSkull();
                }
                else if (mCryptTrigger)
                {
                    DropSkull();
                }


            }

            if (Input.GetKeyDown(KeyCode.X))
            {

                ChangeWeapons();

            }

            if (Input.GetButtonDown("Attack"))
            {

                if (mHasWeapon)
                {
                    Attack();
                }
            }

            //if (Input.GetButton("Jump"))
            //{
            //    mMoveDirection.y = mJumpSpeed;
            //}
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.isGameOver)
        {

            

            //if (this.transform.position.z > mPlayerOriginalPosition.z + 10)
            //{
            //    mMainCamera.transform.position = new Vector3(mMainCamera.transform.position.x, mMainCamera.transform.position.y, this.transform.position.z - 30);
            //}


            //if (mCharacterController.isGrounded)
            //{

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            MovementManager(horizontal, vertical);
            Turning();

            //mMoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));


            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //   mMoveDirection -= Vector3.right;

            //}

            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //     mMoveDirection += Vector3.right;

            //}

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    mMoveDirection += Vector3.forward;
            //}

            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    mMoveDirection -= Vector3.forward;

            //}

            //if (Input.GetButton("Jump"))
            //{
            //    mMoveDirection.y = mJumpSpeed;
            //}



            //}

            //  mMoveDirection.y -= mGravity * Time.deltaTime;
            // mCharacterController.Move(mMoveDirection.normalized * mMoveSpeed * Time.deltaTime);
        }
    }

    private void MovementManager(float horizontal, float vertical)
    {
        if (!mIsAttacking)
        {
            if (horizontal != 0f || vertical != 0f)
            {
                mAnimation.CrossFade("Walk");
                mAnimation["Walk"].speed = 5F;

                movement.Set(horizontal, 0f, vertical);
                movement = movement.normalized * mMoveSpeed * Time.deltaTime;
                mRigidbody.MovePosition(transform.position + movement);

                //Vector3 newPosition = transform.position;
                //newPosition += transform.forward * mMoveSpeed * Time.deltaTime;
                //transform.position = newPosition;                

                //RotatePlayer(horizontal, vertical);
                //Vector3 newPosition = transform.position;
                //newPosition += transform.forward * mMoveSpeed * Time.deltaTime;
                //transform.position = newPosition;
            }
            else
            {
                mAnimation.CrossFade("Idle");
            }

        }
    }

    private void Attack()
    {

        mIsAttacking = true;
        mAnimation.Blend("Attack");
        mAnimation["Attack"].speed = 2F;
    }

    void AttackEnded()
    {
        mIsAttacking = false;
    }

    private void Death()
    {
        mAnimation.Blend("Death");
        gameManager.isGameOver = true;

    }

    //private void RotatePlayer(float horizontal, float vertical)
    //{
    //    Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
    //    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
    //    Quaternion newRotation = Quaternion.Lerp(mRigidbody.rotation, targetRotation, mTurnSmooth * Time.deltaTime);
    //    mRigidbody.MoveRotation(newRotation);

    //}

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            mRigidbody.MoveRotation(newRotation);
        }

    }

    private void PickUpWeapon()
    {
        ActivateSkull();

        mCurrentWeapon = mCollider.gameObject.GetComponent<PickUpWeapon>().mWeaponId;

        mPickedUpWeapons.Add(mCurrentWeapon);

        switch (mCurrentWeapon)
        {
            case 0:
                mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = true;
                mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                break;
            case 1:
                mWeapons[1].GetComponent<MeshRenderer>().enabled = true;
                mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                break;
            case 2:
                mWeapons[2].GetComponent<MeshRenderer>().enabled = true;
                mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                break;
        }

        mWeapons[mCurrentWeapon].GetComponent<WeaponBehaviour>().mUnlocked = true;
        mHasWeapon = true;

        //if (mCurrentWeapon == 0)
        //{
        //    mWeapons[mCurrentWeapon].GetComponent<SkinnedMeshRenderer>().enabled = true;
        //    mWeapons[mCurrentWeapon].GetComponent<WeaponBehaviour>().mUnlocked = true;
        //}
        //else {
        //    mWeapons[mCurrentWeapon].GetComponent<MeshRenderer>().enabled = true;
        //    mWeapons[mCurrentWeapon].GetComponent<WeaponBehaviour>().mUnlocked = true;
        //}

        mInTrigger = false;
        mCollider.gameObject.GetComponent<PickUpWeapon>().mDestroyItself = true;
        mWeaponTrigger = false;

    }

    private void ActivateSkull()
    {
        if (!mSkeletonsActive)
        {


            //for (int i = 0; i < mSkulls.Length; i++)
            //{
            //    mSkulls[i].ShowUp();


            //}

            for (int i = 0; i < mSkeletons.Length; i++)
            {
                mSkeletons[i].ActivateSkeleton();
            }

            mSkeletonsActive = true;
        }
    }

    void PickUpSkull()
    {
        if (!mIsSkullInHand)
        {
            mSkullInHand.ShowUp();
            mSkullInHand.mSkullColor = mCollider.gameObject.GetComponent<PickUpSkull>().mSkullColor;
            mCollider.gameObject.GetComponent<PickUpSkull>().mDestroyItself = true;
            mIsSkullInHand = true;
            mSkullTrigger = false;
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        mCollider = collider;
        mInTrigger = true;

        switch(collider.gameObject.tag)
        {
            case PICK_UP_WEAPON_TAG:
                mWeaponTrigger = true;
                mSkullTrigger = false;
                mCryptTrigger = false;
                break;

            case PICK_UP_SKULL_TAG:
                mWeaponTrigger = false;
                mSkullTrigger = true;
                mCryptTrigger = false;
                break;

            case CRYPT_TAG:
                mWeaponTrigger = false;
                mSkullTrigger = false;
                mCryptTrigger = true;
                break;

            case "LevelPortal":
                SceneManager.LoadScene("L02-Forest");
                break;

        }

        if (collider.gameObject.tag == PICK_UP_WEAPON_TAG)
        {

            mWeaponTrigger = true;
            mSkullTrigger = false;
            mCryptTrigger = false;
        }
        else if (collider.gameObject.tag == PICK_UP_SKULL_TAG)
        {
            mWeaponTrigger = false;
            mSkullTrigger = true;
            mCryptTrigger = false;
        }
        else if (collider.gameObject.tag == CRYPT_TAG)
        {
            mWeaponTrigger = false;
            mSkullTrigger = false;
            mCryptTrigger = true;
        }





    }

    void OnTriggerExit(Collider collider)
    {
        mWeaponTrigger = false;
        mSkullTrigger = false;
        mCryptTrigger = false;
    }

    void ChangeWeapons()
    {
        if (mPickedUpWeapons.Count > 1 && mPickedUpWeapons.Count < 4)
        {

            //if (mCurrentWeapon == 0)
            //{

            //    if (mPickedUpWeapons.Contains(mCurrentWeapon + 1))
            //    {
            //        mCurrentWeapon++;
            //    }
            //    else if (mPickedUpWeapons.Contains(mCurrentWeapon + 2))
            //    {
            //        mCurrentWeapon++;
            //    }


            //}
            //else if (mCurrentWeapon == 1)
            //{
            //    if (mPickedUpWeapons.Contains(mCurrentWeapon - 1))
            //    {
            //        mCurrentWeapon--;
            //    }
            //    else if (mPickedUpWeapons.Contains(mCurrentWeapon + 1))
            //    {
            //        mCurrentWeapon++;
            //    }
            //}
            //else if (mCurrentWeapon == 2)
            //{
            //    mCurrentWeapon = 0;

            //    if (!mPickedUpWeapons.Contains(mCurrentWeapon))
            //    {
            //        mCurrentWeapon = 1;
            //    }

            //}

            if (mCurrentWeapon == 1 || mCurrentWeapon == 0)
            {
                mCurrentWeapon++;


            }
            else if (mCurrentWeapon == 2)
            {
                mCurrentWeapon = 0;
            }

            switch (mCurrentWeapon)
            {


                case 0:
                    if (mWeapons[0].GetComponent<WeaponBehaviour>().mUnlocked)
                    {

                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = true;
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                        Debug.Log("Sword");
                    }
                    else
                    {
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = true;
                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                        mCurrentWeapon = 1;
                        Debug.Log("Staff");
                    }
                    break;

                case 1:
                    if (mWeapons[1].GetComponent<WeaponBehaviour>().mUnlocked)
                    {
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = true;
                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                        Debug.Log("Staff");
                    }
                    else
                    {
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = true;
                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                        mCurrentWeapon = 2;
                        Debug.Log("Axe");
                    }
                    break;

                case 2:
                    if (mWeapons[2].GetComponent<WeaponBehaviour>().mUnlocked)
                    {
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = true;
                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = false;
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                        Debug.Log("Axe");
                    }
                    else
                    {
                        mWeapons[0].GetComponent<SkinnedMeshRenderer>().enabled = true;
                        mWeapons[1].GetComponent<MeshRenderer>().enabled = false;
                        mWeapons[2].GetComponent<MeshRenderer>().enabled = false;
                        mCurrentWeapon = 0;
                        Debug.Log("Sword");
                    }
                    break;
            }
        }

    }

    void ActivateWeaponCollider()
    {
        mWeaponBone.GetComponent<BoxCollider>().enabled = true;
        Debug.Log("Activated");
    }

    void DeactivateWeaponCollider()
    {
        mWeaponBone.GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Deactivated");
    }

    void DropSkull()
    {
        if (mIsSkullInHand)
        {
            mSkullInHand.HideItself();
            mCollider.gameObject.GetComponentInChildren<PickUpSkull>().ShowUp();
            mCollider.gameObject.GetComponentInChildren<Light>().enabled = true;
            mCryptTrigger = false;
            mIsSkullInHand = false;
            mSkullsInPlace++;
            if (mSkullsInPlace == 5)
            {
                gameManager.openExit = true;
            }
        }
    }




}
