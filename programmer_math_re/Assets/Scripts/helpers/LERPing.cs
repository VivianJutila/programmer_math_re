using UnityEngine;

public class LERPing
{
    public static Vector3 LERP_VECTORs(float _T_VALUE, Vector3 _p1, Vector3 _p2)
    {
        Vector3 _result = (1f - _T_VALUE) * _p1 + _T_VALUE * _p2;

        return _result;
    }
}
