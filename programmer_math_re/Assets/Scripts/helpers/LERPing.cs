using UnityEngine;

public class LERPing
{
    public static Vector3 LERP_VECTORs(float _T_VALUE, Vector3 _p1, Vector3 _p2)
    {
        Vector3 _result = (1f - _T_VALUE) * _p1 + _T_VALUE * _p2;

        return _result;
    }

    public static Vector3 LERP_4_WAY_VECTORs(float _T_VALUE, Vector3 _s1, Vector3 _s2, Vector3 _e1, Vector3 _e2)
    {
        Vector3 _result = Vector3.zero;

        Vector3 _a = LERP_VECTORs(_T_VALUE, _s1, _s2);
        Vector3 _b = LERP_VECTORs(_T_VALUE, _s2, _e1);
		Vector3 _c = LERP_VECTORs(_T_VALUE, _e1, _e2);

        Vector3 _A = LERP_VECTORs(_T_VALUE, _a, _b);
        Vector3 _B = LERP_VECTORs(_T_VALUE, _b, _c);

        _result = LERP_VECTORs(_T_VALUE, _A, _B);

		return _result;
    }
}
