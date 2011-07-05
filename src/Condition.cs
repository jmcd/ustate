using System;

namespace ustate
{
    public class Condition
    {
        public readonly State State;
        public readonly Func<char, bool> CharMatch;
        public TransitionAction TransitionAction;

        public Condition(State state, Func<char, bool> charMatch)
        {
            State = state;
            CharMatch = charMatch;
        }

        public Condition(State state) : this(state, null) {}

        public TransitionAction Exec(Action<char> action)
        {
            TransitionAction = new TransitionAction(this, action);
            return TransitionAction;
        }

        public Transition Go(State nextState)
        {
            return Exec(null).Go(nextState);
        }
    }
}