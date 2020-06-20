using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DeadReckoning : MonoBehaviour
{
    public int ownerIndex = 0;
    public EJointType jointType = EJointType.Head;
    public bool lazyInterpolation = false;
    //public bool LazyInterpolation
    //{
    //    get
    //    {
    //        return lazyInterpolation;
    //    }
    //    set
    //    {
    //        lazyInterpolation = value;
    //        if (lazyInterpolation)
    //        {
    //            actSyncLogic = LazySync;
    //        }
    //        else
    //        {
    //            actSyncLogic = Reckoning;
    //        }
    //    }
    //}
    private void Awake()
    {
        //LazyInterpolation = lazyInterpolation;
    }
    public const float kUpdateDuration = 0.1f;
    protected float curLerpTime = 0f;
    public float CurLerpTime
    {
        get
        {
            return curLerpTime;
        }
        set
        {
            curLerpTime = Mathf.Clamp01(value * 10f) * 0.1f;
        }
    }
    TransformSyncData tr = new TransformSyncData();
    public Vector3 sPos = Vector3.zero;
    public Quaternion sRot = Quaternion.identity;
    public void SetTargetTransform(TransformSyncData tr)
    {
        sPos = transform.localPosition;
        sRot = transform.localRotation;

        this.tr = tr;
        CurLerpTime = 0f;
    }

    private void LateUpdate()
    {
        //actSyncLogic();
        LazySync();
        CurLerpTime += Time.deltaTime;
    }
    private Action actSyncLogic = () => { };
    
    //private void Reckoning()
    //{
    //    transform.localPosition = tr.curPos + tr.posSpd * CurLerpTime + 0.5f * tr.posAcc * CurLerpTime * CurLerpTime;
    //    var euler = tr.curRot + tr.rotSpd * CurLerpTime + 0.5f * tr.rotAcc * CurLerpTime * CurLerpTime;
    //    var q = new Quaternion()
    //    {
    //        eulerAngles = euler
    //    };
    //    transform.localRotation = q;
    //}
    
    private void LazySync()
    {
        transform.localPosition = Vector3.Lerp(sPos, tr.curPos, curLerpTime * 10f);
        transform.localRotation = Quaternion.Lerp(sRot, new Quaternion() { eulerAngles = tr.curRot }, curLerpTime * 10f);
    }
}
