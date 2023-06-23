using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace PFX
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpd;
        [SerializeField] private float rotSpd;
        [SerializeField] private float scrollRotSpd;
        [SerializeField] private int edgeScrollThreshold;
        [SerializeField] private float dragPanSpd;

        [SerializeField] private float zoomSpd;
        [SerializeField] private float zoomInc;

        [SerializeField] private float minFOV;
        [SerializeField] private float maxFOV;

        [SerializeField] private float minFollowOffset = 5f;
        [SerializeField] private float maxFollowOffset = 50f;

        [SerializeField] private float minOffsetY = 10f;
        [SerializeField] private float maxOffsetY = 50f;

        [SerializeField] private bool useZoomFOV;

        [SerializeField] private bool useEdgeScrolling;
        [SerializeField] private CinemachineVirtualCamera cam;

        private bool dragPanMoveActive;
        private bool scrollRotateActive;

        private InputManager inputs;
        private Vector2 lastMousePos;
        private float targetFov = 50f;
        private Vector3 followOffset;

        private void Start()
        {
            inputs = InputManager.I;
            followOffset = cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        }

        private void Update()
        {

            KeyboardMovement();
            KeyboardRotation();
            ScrollRotation();

            if(useEdgeScrolling)
                EdgeScrolling();

            DragPanMovement();
            if (useZoomFOV)
                ZoomFOV();
            else if (inputs.HoldShift())
                ZoomLowerY();
            else
                ZoomDistance();
        }

        //Keyboard Controls
        private void KeyboardMovement()
        {
            Vector3 inputDir = new Vector3(0, 0, 0);

            //Keyboard Movement
            float horizontal = InputManager.I.Move().x;
            float vertical = InputManager.I.Move().y;
            inputDir = new Vector3(horizontal, 0, vertical).normalized;

            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            transform.position += -moveDir * moveSpd * Time.deltaTime;
        }    
        private void KeyboardRotation()
        {
            float rotationDir = 0;
            if (inputs.KeyRotate() > 0)
                rotationDir = 1f;
            else if (inputs.KeyRotate() < 0)
                rotationDir = -1f;

            transform.eulerAngles += new Vector3(0, rotationDir * rotSpd * Time.deltaTime, 0);
        }

        //Mouse controls
        private void EdgeScrolling()
        {
            Vector3 dir = new Vector3(0, 0, 0);

            if (Input.mousePosition.x < edgeScrollThreshold) dir.x = -1f;
            if (Input.mousePosition.y < edgeScrollThreshold) dir.z = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollThreshold) dir.x = 1f;
            if (Input.mousePosition.y > Screen.height - edgeScrollThreshold) dir.z = 1f;

            Vector3 moveDir = transform.forward * dir.z + transform.right * dir.x;

            transform.position += moveDir * moveSpd * Time.deltaTime;
        }
        private void DragPanMovement()
        {
            Vector3 dir = new Vector3(0, 0, 0);

            if (inputs.RightMouseClick())
            {
                dragPanMoveActive = true;
                lastMousePos = Input.mousePosition;
            }
            
            if(inputs.RightMouseRelease())
                dragPanMoveActive = false;

            if(dragPanMoveActive)
            {
                Vector2 mouseMoveDelta = (Vector2)Input.mousePosition - lastMousePos;

                dir.x = mouseMoveDelta.x * dragPanSpd;
                dir.z = mouseMoveDelta.y * dragPanSpd;

                lastMousePos = Input.mousePosition;
            }



            Vector3 moveDir = transform.forward * dir.z + transform.right * dir.x;

            transform.position += moveDir * moveSpd * Time.deltaTime;
        }

        private void ScrollRotation()
        {
            Vector3 dir = new Vector3(0, 0, 0);

            if (inputs.MiddleMouseClick())
            {
                scrollRotateActive = true;
                lastMousePos = Input.mousePosition;
            }

            if (inputs.MiddleMouseRelease())
                scrollRotateActive = false;

            if (scrollRotateActive)
            {
                Vector2 mouseMoveDelta = (Vector2)Input.mousePosition - lastMousePos;
                float rotationDirY = 0;
                if (mouseMoveDelta.x > 0)
                    rotationDirY = 1f;
                else if (mouseMoveDelta.x < 0)
                    rotationDirY = -1f;

                transform.eulerAngles += new Vector3(0, rotationDirY * rotSpd * Time.deltaTime,0);

                lastMousePos = Input.mousePosition;
            }
        }

        private void ZoomFOV()
        {
            if(inputs.Scroll() > 0)
            {
                targetFov -= zoomInc;
            }

            if (inputs.Scroll() < 0)
            {
                targetFov += zoomInc;
            }

            targetFov = Mathf.Clamp(targetFov, minFOV, maxFOV);

            Mathf.Lerp(cam.m_Lens.FieldOfView, targetFov, Time.deltaTime * zoomSpd);
            cam.m_Lens.FieldOfView = targetFov;
        }
        private void ZoomDistance()
        {
            Vector3 zoomDir = followOffset.normalized;
            if (inputs.Scroll() > 0)
            {
                followOffset -= zoomDir * zoomInc;
            }

            if (inputs.Scroll() < 0)
            {
                followOffset += zoomDir * zoomInc;
            }

            if(followOffset.magnitude < minFollowOffset)
            {
                followOffset = zoomDir * minFollowOffset;
            }

            if (followOffset.magnitude > maxFollowOffset)
            {
                followOffset = zoomDir * maxFollowOffset;
            }

            cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpd);
        }

        private void ZoomLowerY()
        {
            if (inputs.Scroll() > 0)
            {
                followOffset.y -= zoomInc;
            }

            if (inputs.Scroll() < 0)
            {
                followOffset.y += zoomInc;
            }

            followOffset.y = Mathf.Clamp(followOffset.y, minOffsetY, maxOffsetY);

            cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpd);
        }

    }


}
