using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ustate;

namespace Tests
{
    [TestFixture]
    public class ImdbActor
    {
        [Test]
        public void Test()
        {
            var currentToken = new StringBuilder();

            Action<char> newChar = c =>
            {
                currentToken.Append(c);
            };

            Action<char> newCell = c =>
            {
                Console.WriteLine(currentToken);
                currentToken.Length = 0;
            };

            Action<char> finalCell = c => { if (currentToken.Length > 0) Console.WriteLine(currentToken); };

            var machine = new Machine();

            var actorName = machine.NewState("start");
            var beforeTitle = machine.NewState("beforeTitle");
            var title = machine.NewState("title");
            var tv = machine.NewState("tv");
            var movie = machine.NewState("movie");
            var meta = machine.NewState("meta");
            var data = machine.NewState("data");
            var charName = machine.NewState("charName");
            var billing = machine.NewState("billing");


            actorName
                .On('\t').Exec(newCell).Go(beforeTitle)
                .Default().Exec(newChar)
                ;

            beforeTitle
                .On('\t').Go(beforeTitle)
                .Default().Exec(newChar).Go(title)
                ;

            title
                .On('\"').Go(tv)
                .Default().Exec(newChar).Go(movie);

            tv
                .On('\"').Exec(newCell).Go(meta)
                .Default().Exec(newChar);

            movie
                .On('(').Exec(newCell).Go(data)
                .Default().Exec(newChar)
                ;

            meta
                .On('(').Go(data)
                .On('[').Go(charName)
                .On('<').Go(billing)
                .On('\0').Exec(finalCell)
                .Default().Go(meta)
                ;

            data
                .On(')').Exec(newCell).Go(meta)
                .Default().Exec(newChar)
                ;

            charName
                .On(']').Exec(newCell).Go(meta)
                .Default().Exec(newChar)
                ;

            billing
                .On('>').Exec(newCell).Go(meta)
                .Default().Exec(newChar)
                ;



            using (var r = new StreamReader("actors.list.short"))
            {
                var line = default(string);
                
                while ((line = r.ReadLine())!=null)
                {
                    if (line.Length == 0) continue;
                    
                    machine.CurrentState = actorName;
                    foreach (var c in line)
                    {
                        machine.Accept(c);
                    }
                    machine.Accept('\0');
                    Console.WriteLine("---------------------------");
                }

                Console.WriteLine("DONE");
            }



        }
        
    }
}
