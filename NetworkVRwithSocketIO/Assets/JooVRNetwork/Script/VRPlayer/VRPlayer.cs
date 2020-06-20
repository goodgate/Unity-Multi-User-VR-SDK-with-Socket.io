using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour
{
    public int playerIndex = -1;

    public Transform head = null;
    public Transform leftHand = null;
    public Transform rightHand = null;
    public virtual void InitializePlayer(int pPlayerIdx, Vector3 spawnPosition)
    {
        playerIndex = pPlayerIdx;
        transform.position = spawnPosition;
    }
}
