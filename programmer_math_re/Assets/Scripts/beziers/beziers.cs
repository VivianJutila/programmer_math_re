using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class beziers : MonoBehaviour
{
    [SerializeField]
    [UnityEngine.Range(0f, 1f)]
    private float T_VALUE;

    [SerializeField]
    private List<GameObject> bezier_point_list;

    private void OnDrawGizmos()
    {
        GameObject _last_bezier_point = null;
        Vector3 _last_lerp = Vector3.zero;
        Vector3 _last_lerp_2 = Vector3.zero;

        Handles.DrawBezier(bezier_point_list[0].transform.position, bezier_point_list[bezier_point_list.Count - 1].transform.position, bezier_point_list[1].transform.position, bezier_point_list[bezier_point_list.Count - 2].transform.position, Color.gold, null, 1f);

        foreach(GameObject _bezier_point in bezier_point_list)
        {
            if (_last_bezier_point != null)
            {
                Handles.DrawLine(_bezier_point.transform.position, _last_bezier_point.transform.position);

                Gizmos.color = Color.coral;
                Vector3 _current_lerp = LERPing.LERP_VECTORs(T_VALUE, _last_bezier_point.transform.position, _bezier_point.transform.position);
                Gizmos.DrawSphere(_current_lerp, 0.1f);

                if (_last_lerp != Vector3.zero) 
                {
                    Handles.DrawLine(_current_lerp, _last_lerp);
                    Gizmos.DrawSphere(LERPing.LERP_VECTORs(T_VALUE, _last_lerp, _current_lerp), 0.1f);

                    if (_last_lerp_2 != Vector3.zero)
                    {
                        Handles.DrawLine(LERPing.LERP_VECTORs(T_VALUE, _last_lerp, _current_lerp), _last_lerp_2);

                        Gizmos.DrawSphere(LERPing.LERP_VECTORs(T_VALUE, _last_lerp_2, LERPing.LERP_VECTORs(T_VALUE, _last_lerp, _current_lerp)), 0.1f);
                    }
                    _last_lerp_2 = LERPing.LERP_VECTORs(T_VALUE, _last_lerp, _current_lerp);
                }
                
                _last_lerp = _current_lerp;
            }
            _last_bezier_point = _bezier_point;
        }
    }
}
