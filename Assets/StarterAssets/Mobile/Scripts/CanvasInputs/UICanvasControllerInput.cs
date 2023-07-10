using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;
        SinglePlayerScoreBoardScript sb;
        private void Start()
        {
            Invoke("GetRefrence", 1f);
        }
        void GetRefrence()
        {
            starterAssetsInputs = GameObject.FindGameObjectWithTag("Player").GetComponent<StarterAssetsInputs>();
            sb = SinglePlayerScoreBoardScript.instance;
        }

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }



        //specific to fight the bear 

        private void Update()
        {

            var tapCount = Input.touchCount;
            for (var i = 0; i < tapCount; i++)
            {
                if (i < 2)
                {
                    var screenTouch = Input.GetTouch(i);
                    if (screenTouch.position.x > Screen.width / 2f)
                    {
                        if (screenTouch.phase == TouchPhase.Moved)
                        {
                            float m = 3f;
                            if (sb.isMobile())
                                m = -50f;
                            VirtualLookInput(new Vector2(screenTouch.deltaPosition.x, -screenTouch.deltaPosition.y) * m);
                        }
                        else
                            VirtualLookInput(Vector2.zero);
                    }
                }
            }

        }
    }
}
