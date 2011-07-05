using System;

namespace ustate
{
    public class TransitionAction
    {
        public Action<char> Action;
        public readonly Condition Condition;
        public Transition Transition;

        public TransitionAction(Condition condition, Action<char> action)
        {
            Action = action;
            Condition = condition;
            Transition = Go(Condition.State);
        }

        public Transition Go(State nextState)
        {
            Transition = new Transition(this, nextState);
            return Transition;
        }

        public Condition On(char c)
        {
            return Transition.On(c);
        }

        public Condition On(Func<char, bool> charMatch)
        {
            return Transition.On(charMatch);
        }

        public void Execute(char c)
        {
            if (Action != null) Action(c);
        }

        public Condition Default()
        {
            return Transition.Default();
        }
    }
}