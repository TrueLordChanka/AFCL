using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;
using Unity;


namespace AndrewFTW
{ 
    public class ObjectMountOnGround : FVRPhysicalObject
    {
        public Transform PlacementRayPoint;
        public LayerMask PlacementLayerMask;
        public float MaxPlacementAngle = 75f;
        public float DistToGroundFromOrigin;

        [NonSerialized]
        public RaycastHit m_hit;

        private bool m_OnGround;

#if !(UNITY_EDITOR || UNITY_5 || DEBUG == true)

        public override void BeginInteraction(FVRViveHand hand)
        {
            base.BeginInteraction(hand);
            m_OnGround = false;
        }

        public override void EndInteraction(FVRViveHand hand)
        {
            base.EndInteraction(hand);
            if (Physics.Raycast(PlacementRayPoint.position, -base.transform.up, out m_hit, 0.125f, PlacementLayerMask) && m_hit.collider.attachedRigidbody == null && Vector3.Angle(Vector3.up, m_hit.normal) < MaxPlacementAngle)
            {
                base.transform.position = m_hit.point + transform.up * DistToGroundFromOrigin; //mount it the set distance from ground
                m_OnGround = true;
                SM.PlayBulletImpactHit(BulletImpactSoundType.Grass, base.transform.position, 0.3f, 1f);
                base.RootRigidbody.isKinematic = true;
            }

        }

        public override bool IsDistantGrabbable()
        {
            return !this.m_OnGround && base.IsDistantGrabbable();
        }


#endif
    }
}



