using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRemotePlayer : VRPlayer
{
    public List<DeadReckoning> joints = null;
    protected Dictionary<EJointType, DeadReckoning> dictReckonings = null;

    public override void InitializePlayer(int pPlayerIndex, Vector3 spawnPosition)
    {
        base.InitializePlayer(pPlayerIndex, spawnPosition);
        dictReckonings = new Dictionary<EJointType, DeadReckoning>();
        joints.ForEach((n) =>
        {
            n.ownerIndex = playerIndex;
            dictReckonings.Add(n.jointType, n);
        });
    }
    public void SetTransforms(TransformSyncData[] trs)
    {
        for(int i = 0; i< trs.Length; i++)
        {
            dictReckonings[(EJointType)trs[i].jointType].SetTargetTransform(trs[i]);
        }
    }
}
