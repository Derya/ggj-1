﻿using System;

public static class Util
{


    public static T[] removeAt<T>(this T[] source, int index)
    {
        T[] dest = new T[source.Length - 1];
        if (index > 0)
            Array.Copy(source, 0, dest, 0, index);

        if (index < source.Length - 1)
            Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

        return dest;
    }

    public static float Lerp(float rangeBegin, float rangeEnd, float min, float max, float x)
    {
        //aaaaa
        return 0f;
    }

}