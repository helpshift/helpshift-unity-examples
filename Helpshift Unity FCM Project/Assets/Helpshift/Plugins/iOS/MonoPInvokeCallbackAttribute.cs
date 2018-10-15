#if UNITY_IOS
using System;

[System.AttributeUsage(System.AttributeTargets.Method)]
public sealed class MonoPInvokeCallbackAttribute : System.Attribute
{
    private Type type;
    public MonoPInvokeCallbackAttribute(Type t)
    {
        type = t;
    }
}

#endif