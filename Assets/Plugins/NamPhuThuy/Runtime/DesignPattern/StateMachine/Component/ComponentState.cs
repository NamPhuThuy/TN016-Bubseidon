using UnityEngine;
using System.Collections;

namespace NamPhuThuy
{
    public class ComponentState : MonoBehaviour 
    {
        //Public Properties:
        /// <summary>
        /// Gets a value indicating whether this instance is the first state in this state machine.
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return transform.GetSiblingIndex () == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is the last state in this state machine.
        /// </summary>
        public bool IsLast
        {
            get
            {
                return transform.GetSiblingIndex () == transform.parent.childCount - 1;
            }
        }

        /// <summary>
        /// Gets or sets the state machine.
        /// </summary>
        public ComponentStateMachine ComponentStateMachine
        {
            get
            {
                if (_componentStateMachine == null)
                {
                    _componentStateMachine = transform.parent.GetComponent<ComponentStateMachine>();
                    if (_componentStateMachine == null)
                    {
                        Debug.LogError("States must be the child of a StateMachine to operate.");
                        return null;
                    }
                }

                return _componentStateMachine;
            }
        }

        //Private Variables:
        ComponentStateMachine _componentStateMachine;

        //Public Methods
        /// <summary>
        /// Changes the state.
        /// </summary>
        public void ChangeState(int childIndex)
        {
            ComponentStateMachine.ChangeState(childIndex);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        public void ChangeState (GameObject state)
        {
            ComponentStateMachine.ChangeState (state.name);
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        public void ChangeState (string state)
        {
            if (ComponentStateMachine == null) return;
            ComponentStateMachine.ChangeState (state);
        }

        /// <summary>
        /// Change to the next state if possible.
        /// </summary>
        public GameObject Next ()
        {
            return ComponentStateMachine.Next ();
        }

        /// <summary>
        /// Change to the previous state if possible.
        /// </summary>
        public GameObject Previous ()
        {
            return ComponentStateMachine.Previous ();
        }

        /// <summary>
        /// Exit the current state.
        /// </summary>
        public void Exit ()
        {
            ComponentStateMachine.Exit ();
        }
    }
}