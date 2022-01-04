using System;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

namespace UnityStandardAssets.Characters.ThirdPerson 
{
    //[RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        //new 
        PhotonView m_pv;
        TextMesh pName;
        
        private void Awake()
        {
            m_pv = GetComponent<PhotonView>();
            m_Character = GetComponent<ThirdPersonCharacter>();
            pName = transform.GetChild(1).GetComponent<TextMesh>(); //make sure name field is always seconf child in all player avatar 
            if (!m_pv.IsMine)
            {
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(transform.GetChild(1).gameObject);// make sure virtual camera child 1
                Destroy(GetComponent<ThirdPersonCharacter>());
                Destroy(GetComponent<Rigidbody>());

                //Destroy(GetComponent<Animator>());
            }
            // make sure camera chold 0 of player avatars
            //else
            //{
               // m_pv.RPC("RPC_UpdateName", RpcTarget.AllBuffered, GameRoom.room.playerInGame.ToString());
            //}
            m_Cam = transform.GetChild(0);
          

        }
        /*[PunRPC]
        void RPC_UpdateName(string name)
        {
            pName.text = name;
        }

        void LateUpdate()
        {
            pName.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }*/

    


        private void Update()
        {
            if (!m_Jump && m_pv.IsMine && !GameRoom.room.Ended && GameRoom.room.started)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);
            if (m_pv.IsMine && !GameRoom.room.Ended && GameRoom.room.started)
            {
                // calculate move direction to pass to character
                if (m_Cam != null)
                {
                    // calculate camera relative direction to move:
                    m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                    if(v>0)
                        m_Move = v * m_CamForward + h * m_Cam.right;
                    else
                        m_Move = 0 * m_CamForward + h * m_Cam.right;
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    m_Move = v * Vector3.forward + h * Vector3.right;
                }
                // pass all parameters to the character control script
                m_Character.Move(m_Move, crouch, m_Jump);
                m_Jump = false;
            }
            else if(m_pv.IsMine && GameRoom.room.Ended && GameRoom.room.started)
            {
                m_Character.Move(Vector3.zero,crouch,false);
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if(m_pv.IsMine && collision.gameObject.CompareTag("Chicken") && !GameRoom.room.Ended && GameRoom.room.started)
            {

                collision.gameObject.GetComponent<ChickenScript>().Collected(m_pv);
                ScoreUIController.instance.AnimChickenCollected();
               
            }
        }
        



    }
}
