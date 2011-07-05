using System;

namespace ustate
{
    public class Transition
    {
        public readonly TransitionAction TransitionAction;
        public readonly State NextState;

        public Transition(TransitionAction transitionAction, State nextState)
        {
            TransitionAction = transitionAction;
            NextState = nextState;
        }

        public Condition On(char c)
        {
            return TransitionAction.Condition.State.On(c);
        }

        public Condition On(Func<char, bool> charMatch)
        {
            return TransitionAction.Condition.State.On(charMatch);
        }

        public Condition Default()
        {
            return TransitionAction.Condition.State.Default();
        }
    }
}