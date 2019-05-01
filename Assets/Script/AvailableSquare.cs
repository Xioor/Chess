using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableSquare : MonoBehaviour
{
    Vector2Int m_GridLocation;

   public void SetInfo(Vector2Int location)
   {
       m_GridLocation = location;
   }

   public Vector2Int GetInfo()
   {
       return m_GridLocation;
   }
}
