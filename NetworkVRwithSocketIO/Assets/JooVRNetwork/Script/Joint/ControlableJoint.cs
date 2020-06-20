using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlableJoint : MonoBehaviour
{
    public int playerIndex = -1;
    public EJointType jointType = EJointType.Head;

    public Vector3 prevPos = Vector3.zero;
    public Vector3 prevRot = Vector3.zero;
    public Vector3 posSpd = Vector3.zero;
    public Vector3 rotSpd = Vector3.zero;
    public void GetPredictedTransforms(ref TransformSyncData data)
    {
        data.curPos = transform.localPosition;
        data.curRot = transform.localRotation.eulerAngles;

        data.posSpd = (data.curPos - prevPos) * 10f;
        data.rotSpd = (data.curRot - prevRot);
        data.rotSpd = new Vector3(
            (data.rotSpd.x + 540) % 360f - 180f,
            (data.rotSpd.y + 540) % 360f - 180f,
            (data.rotSpd.z + 540) % 360f - 180f
            ) * 10f;

        data.posAcc = data.posSpd - posSpd;
        data.rotAcc = data.rotSpd - rotSpd;

        prevPos = data.curPos;
        prevRot = data.curRot;
        posSpd = data.posSpd;
        rotSpd = data.rotSpd;
        data.jointType = (int)this.jointType;
    }

}

public class TransformSyncData
{
    public int jointType = 0;
    public Vector3 curPos = Vector3.zero;
    public Vector3 curRot = Vector3.zero;

    public Vector3 posSpd = Vector3.zero;
    public Vector3 rotSpd = Vector3.zero;

    public Vector3 posAcc = Vector3.zero;
    public Vector3 rotAcc = Vector3.zero;
}

public class TransformSyncDataContainer
{
    public TransformSyncData[] data = null;
    public int ownerIndex = 0;
}