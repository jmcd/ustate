using System;
using System.Collections.Generic;
using System.Linq;

namespace ustate
{
    public class State
    {
        public readonly Machine Machine;
        public readonly string Name;
        public readonly List<Condition> Conditions = new List<Condition>();
        public Condition DefaultCondition;

        public State(Machine machine, string name)
        {
            Machine = machine;
            Name = name;
        }

        public Condition On(char c)
        {
            return On(x => x == c);
        }

        public Condition On(Func<char, bool> charMatch)
        {
            var condition = new Condition(this, charMatch);
            Conditions.Add(condition);
            return condition;
        }

        public Condition Default()
        {
            DefaultCondition = new Condition(this);
            return DefaultCondition;
        }

        public Condition GetCondtion(char c)
        {
            return Conditions.Where(x => x.CharMatch(c)).SingleOrDefault() ?? DefaultCondition;
        }
    }
}