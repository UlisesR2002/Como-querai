using UnityEngine;

public static class Utilities 
{
    #region Vectors and angles
    public static float AngleOfVector(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static Vector2 VectorOfAngle(float angle)
    {
        return (Quaternion.Euler(0, 0, angle) * Vector2.right).normalized;
    }

    public static Vector2 Rotate(Vector2 vector, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }
    public static Vector2 Rotate(Vector2 vector, Quaternion rotation)
    {
        float degrees = rotation.eulerAngles.z;
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }
    public static Vector2 Rotate(Vector2 vector, Transform transform)
    {
        float degrees = transform.rotation.eulerAngles.z;
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector;
    }

    public static bool IsVectorOpositive(Vector2 vector1, Vector2 vector2)
    {
        float angle = Vector2.SignedAngle(vector1, vector2);
        return (angle == -180) || (angle == 180);
    }

    public static bool IsVectorOpositive(float angle1, float angle2)
    {
        float angle = Vector2.SignedAngle(VectorOfAngle(angle1), VectorOfAngle(angle2));
        return (angle == -180) || (angle == 180);
    }

    public static Vector2 VectorsMiddle(Vector2 vector1, Vector2 vector2)
    {
        return Vector3.MoveTowards(vector1, vector2, Vector3.Distance(vector1, vector2)/2f);
    }
    #endregion

    #region Math
    public static bool RandomBool()
    {
        float i;
        do
        {
            i = Random.Range(-1f, 1f);
        } while (i == 0);

        return i > 0;
    }

    public static int RandomSign()
    {
        float i;
        do
        {
            i = Random.Range(-1f, 1f);
        } while (i == 0);

        if (i > 0)
        {
            return 1;
        }
        return -1;
    }
    #endregion

    #region Array and list
    public static int MoveIndexInArray<T>(int actualIndex, T[] array, float movement)
    {
        int aux = actualIndex;

        int dir = System.MathF.Sign(movement);
        
        if(dir == 0)
            return aux;

        aux += dir;

        int length = array.Length;

        if (aux < 0)
            aux = length - 1;

        if (aux >= length)
            aux = 0;

        return aux;
    }
    #endregion

    #region Debug
    public static void DebugLog(object o, bool show = true)
    {
        if (show)
        {
            Debug.Log(o);
        }
    }
    #endregion
}
