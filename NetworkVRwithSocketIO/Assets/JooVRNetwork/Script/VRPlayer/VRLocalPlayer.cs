using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using JYJ_Utils;
using Valve.VR;

public class VRLocalPlayer : VRPlayer
{
    public ControlableJoint[] joints;
    protected Dictionary<EJointType, ControlableJoint> dictJoints = null;
    
    public override void InitializePlayer(int pPlayerIndex,Vector3 spawnPosition)
    {
        base.InitializePlayer(pPlayerIndex, spawnPosition);
        dictJoints = new Dictionary<EJointType, ControlableJoint>();
        foreach (var joint in joints)
        {
            joint.playerIndex = pPlayerIndex;
            dictJoints.Add(joint.jointType, joint);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(CoEmitTransforms());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected IEnumerator CoEmitTransforms()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var transforms = joints.Select(n =>
            {
                var tr = new TransformSyncData();
                n.GetPredictedTransforms(ref tr);
                return tr;
            }).ToArray();
            TransformSyncDataContainer container = new TransformSyncDataContainer();
            container.data = transforms;
            container.ownerIndex = playerIndex;
            var jsonStr = JsonConvert.SerializeObject(container);
            JooNetManager.Instance.socket.Emit("deadReckoning", JSONObject.Create(jsonStr));
        }
    }
}
