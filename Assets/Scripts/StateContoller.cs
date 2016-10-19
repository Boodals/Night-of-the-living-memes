using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;


/// <summary>
/// To use the state controller correctly:
///     -Define some const short _name_ variables for state names. 
///      These will be used like flags so use 1,2,4,8 etc so you can combine.
///      
///     -Add the [Update(state)] attribute to a function with signature void _name_() 
///      with state = the state this update function is for
///      
///     -Add the [Transition(from, to)] attribute to a function with signature void _name_() 
///      This function will get called when transitioning from state "from" to state "to".
///      
///     -call constructor with object that is defining the transition and update functions.
///     -call update to call the right update function.
///     -call transition to transition to target state.
/// </summary>
public class StateContoller
{
    public delegate void BasicFnct();
    //key - function
    protected List<TransPair> m_transitions;
    protected Dictionary<short, BasicFnct> m_updates;

    protected short m_curState;

    //protected List<TransPair> m_transitions;
    public const short ANY_STATE = short.MaxValue;

    protected struct TransPair
    {
        public int m_key;
        public BasicFnct m_fnct;
    }
    public short currentState()
    {
        return m_curState;
    }
    public StateContoller(object _obj)
    {
        m_curState = ANY_STATE;
        m_transitions = new List<TransPair>();
        m_updates = new Dictionary<short, BasicFnct>();

        MethodInfo[] mthds = _obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        // search all fields and find the attribute [TransitionAttribute]
        for (int i = 0; i < mthds.Length; i++)
        {
            TransitionAttribute attribute = Attribute.GetCustomAttribute(mthds[i], typeof(TransitionAttribute)) as TransitionAttribute;
            // if we detect a RememberMe attribute, generate field for dynamic class
            if (attribute != null)
            {
                TransPair p;
                p.m_key = getKey(attribute.m_from, attribute.m_to);
               
                //IF it already exists do not overrwrite! derrived fncts are handled first!
                int index;
                if (tryGetValue(p.m_key, out p.m_fnct, out index))
                {
                    //DO NO OVERWRITE
                    ////if so, overwrite it
                    ////m_transitions[index] = p;
                }
                else
                {
                    //else add it
                    p.m_fnct = Delegate.CreateDelegate(typeof(BasicFnct), _obj, mthds[i]) as BasicFnct;
                    m_transitions.Add(p);
                }
                
            }
            else //otherwise check for state update fnct
            {
                UpdateAttribute updateAttribute = Attribute.GetCustomAttribute(mthds[i], typeof(UpdateAttribute)) as UpdateAttribute;
                if (updateAttribute != null)
                {
                    //seems like derrived class gets looked at first so we want first thing to not get overwritten...
                    BasicFnct fnc = null;
                    if (!m_updates.TryGetValue(updateAttribute.m_state, out fnc))
                    {
                        m_updates[updateAttribute.m_state] = Delegate.CreateDelegate(typeof(BasicFnct), _obj, mthds[i]) as BasicFnct;
                    }
                }

            }
        }
    }

    public void update()
    {
        m_updates[m_curState]();
    }
    public void transition(short _to)
    { 
        //look for transition for forward call
        int key = getKey(m_curState, _to);
        BasicFnct trans = null;

        //if there's a transition function, call it!
        //@@implementt they "higher key = lower priority" system
        if (tryGetValue(key, out trans))
        {
            trans();
        }
        
        //change curstate to target state
        m_curState = _to;
    }

    private bool tryGetValue(int _key, out BasicFnct _fnct)
    {
        bool result = false;
        _fnct = null;

        for (int i = 0; i < m_transitions.Count; ++i)
        {
            TransPair p = m_transitions[i];
            if ((getTo(p.m_key) & getTo(_key)) > 0 && (getFrom(p.m_key) & getFrom(_key)) > 0)
            {
                _fnct = p.m_fnct;
                result = true;
                break;
            }
        }
    

        return result;
    }

    private bool tryGetValue(int _key, out BasicFnct _fnct, out int _index)
    {
        bool result = false;
        _fnct = null;
        _index = -1;

        for (int i = 0; i < m_transitions.Count; ++i)
        {
            TransPair p = m_transitions[i];
            if ((getTo(p.m_key) & getTo(_key)) > 0 && (getFrom(p.m_key) & getFrom(_key)) > 0)
            {
                _fnct = p.m_fnct;
                result = true;
                _index = i;
                break;
            }
        }

        return result;
    }
    private int getKey(short _from, short _to)
    {
        int key = (int)_to | ((int)_from << 15);
        return key;
    }

    private short getFrom(int _key)
    {
        return (short)(_key >> 15);
    }
    private short getTo(int _key)
    {
        return (short)(_key & short.MaxValue);
    }
}
