using UnityEngine;
using System.Collections;
using System;


[AttributeUsage(AttributeTargets.Method)]
public class TransitionAttribute : Attribute
{
    public short m_to;
    public short m_from;
    public TransitionAttribute(short _from, short _to)
    {
        m_to = _to;
        m_from = _from;
    }
}
[AttributeUsage(AttributeTargets.Method)]
public class TransitionOverrideAttribute : Attribute
{
    public short m_to;
    public short m_from;
    public TransitionOverrideAttribute(short _from, short _to)
    {
        m_to = _to;
        m_from = _from;
    }
}
[AttributeUsage(AttributeTargets.Method)]
public class UpdateAttribute : Attribute
{
    public short m_state;
    public UpdateAttribute(short _state)
    {
        m_state = _state;
    }
}
