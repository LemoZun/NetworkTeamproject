using System.Collections;
using _2.Public.Interfaces;
using UnityEngine;

namespace _1.Private.ParkJM.Scripts.States
{
    public class GrabbingState : PlayerState
    {
        private GameObject grabbedObject;
        private readonly float _moveSpeedOnGrab;
        private Coroutine grabCheckRoutine;
        private const float GrabSpeedOffset = 0.2f;

        public GrabbingState(PlayerController player) : base(player)
        {
            _moveSpeedOnGrab = player.model.moveSpeed * GrabSpeedOffset;
        }
        public override void Enter()
        {
            grabbedObject = null;
            player.view.SetBoolParameter(E_AniParameters.Pushing, true);
            grabCheckRoutine ??= player.StartCoroutine(CheckGrabPointRoutine());
        }

        public override void Update()
        {
            if (!RemoteInput.inputs[player.model.playerNumber].grabInput)
            {
                ReleaseGrabbedObject();
                player.ChangeState(E_PlayeState.Idle);
            }
        }

        public override void FixedUpdate()
        {
            ApplyMovement();

            if(grabbedObject != null)
            {
                PushOrPullGrabbedObject(grabbedObject);
            }

            player.MoveOnConveyor();
        }

        public override void LateUpdate()
        {
            player.view.UpSpine();
        }

        public override void Exit()
        {
            ReleaseGrabbedObject();
            player.view.SetBoolParameter(E_AniParameters.Pushing, false);
            player.view.SetBoolParameter(E_AniParameters.Pulling, false);
            
            if (grabCheckRoutine != null)
            {
                player.StopCoroutine(grabCheckRoutine);
                grabCheckRoutine = null;
            }
        }

        private void ReleaseGrabbedObject()
        {
            if (grabbedObject == null) 
                return;
            grabbedObject.GetComponent<IGrabbable>().OnGrabbedLeave();
            grabbedObject = null;
        }

        private void ApplyMovement()
        {
            Vector3 targetVel;
            if (player.isSlope)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(player.moveDir, player.chosenHit.normal).normalized;
                targetVel = slopeDirection * _moveSpeedOnGrab;
            }
            else
            {
                targetVel = player.moveDir * _moveSpeedOnGrab;
                targetVel.y = player.rb.velocity.y;
            }
            player.rb.velocity = targetVel;
        }

        private IEnumerator CheckGrabPointRoutine()
        {
            while (true)
            {
                GameObject detectedObject = player.CheckGrabPoint();

                if (detectedObject != grabbedObject)
                {
                    // 잡힌 오브젝트가 바뀌거나, 잡힌 오브젝트가 범위를 벗어났을 때
                    ReleaseGrabbedObject();
                    grabbedObject = detectedObject;

                    if (grabbedObject != null) 
                    {
                        // 새롭게 잡힌 오브젝트가 있을 경우
                        player.model.InvokePlayerGrabbing();
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void PushOrPullGrabbedObject(GameObject grabObject)
        {
            Rigidbody grabbedObjectRb = grabObject.gameObject.GetComponent<Rigidbody>();

            if (grabbedObjectRb == null || grabbedObjectRb.isKinematic)
            {
                Debug.LogWarning("rigidbody가 없거나 Kinematic 임");
                return;
            }

            Vector3 camForward = player.camTransform.forward;
            camForward.y = 0f;
            Vector3 moveDir = player.moveDir;

            // 내적 계산 후 push인지 pull인지 결정
            float dotProduct = Vector3.Dot(camForward, moveDir);

            switch (dotProduct)
            {
                case > 0f:
                {
                    // 밀기
                    grabbedObjectRb.velocity = moveDir * player.model.grabForce;
                    if (player.view.GetBoolParameter(E_AniParameters.Pushing)) 
                        return;
                    player.view.SetBoolParameter(E_AniParameters.Pushing, true);
                    player.view.SetBoolParameter(E_AniParameters.Pulling, false);
                    break;
                }
                case < 0f:
                {
                    // 당기기
                    grabbedObjectRb.velocity = moveDir * player.model.grabForce;
                    if (player.view.GetBoolParameter(E_AniParameters.Pulling)) 
                        return;
                    player.view.SetBoolParameter(E_AniParameters.Pushing, false);
                    player.view.SetBoolParameter(E_AniParameters.Pulling, true);
                    break;
                }
            }




            //grabbedObjectRb.AddForce(player.moveDir * player.model.grabForce, ForceMode.Force); // 지속적으로 해주는거라 이러면 안됨

            // 잡았다가 놓치는경우
            //if (grabbedObjectRb == null)
            //{
            //    player.ChangeState(E_PlayeState.Idle);
            //}

            //grabbedObject.gameObject.GetComponent<Rigidbody>().velocity = targetVelocity;
            //Debug.Log($"TargetVelocity : {targetVelocity}");
            //Debug.Log($"잡힌 오브젝트 이름 : {grabbedObject.gameObject.name}");


            //if (player.moveDir != Vector3.zero)
            //{
            //    Quaternion targetRotation = Quaternion.LookRotation(player.moveDir);
            //    targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            //    player.transform.rotation = Quaternion.Slerp(
            //        player.transform.rotation,
            //        targetRotation,
            //        Time.fixedDeltaTime * 15 // 수정필요
            //    );
            //}
        }
    }
}
