using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]public List<Transform> tileList = new List<Transform>();
    Transform[] tileObjects;

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        // fillTiles();
        for (int i = 0; i < tileList.Count; i++){
            Vector3 currentPos = tileList[i].position;
            if(i>0){
                Vector3 prevPos = tileList[i - 1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }
    }
    // void fillTiles(){
    //     tileList.Clear();
    //     tileObjects = GetComponentsInChildren<Transform>();
    //     foreach(Transform tile in tileObjects){
    //         if(tile != transform){
    //             tileList.Add(tile);
    //         }
    //     }
    // }

}
