using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RefreshAssetDB : MonoBehaviour
{
    //[ContextMenuItem()]
   public void REFRESH()
    {
         AssetDatabase.Refresh();
    }
}
