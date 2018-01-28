using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

    public Vector3 startPosition { get { return startGameObject.transform.position; }}

    [SerializeField]
    private GameObject startGameObject;

}
