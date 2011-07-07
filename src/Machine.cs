using System.Collections.Generic;

namespace ustate
{
    public class Machine
    {
        public readonly List<State> States = new List<State>();
        public State CurrentState;

        public State NewState()
        {
            return NewState("State" + States.Count);
        }

        public State NewState(string name)
        {
            var state = new State(this, name);
            States.Add(state);
            return state;
        }

        public void Accept(char c)
        {
            if (CurrentState == null) CurrentState = States[0];

            var condition = CurrentState.GetCondtion(c);
            condition.TransitionAction.Execute(c);
            var nextState = condition.TransitionAction.Transition.NextState;

            //Console.WriteLine(CurrentState.Name + " -> " +nextState.Name);

            CurrentState = nextState;
        }
    }
}