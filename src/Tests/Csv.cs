using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ustate;

namespace Tests
{
    [TestFixture]
    public class Csv
    {
        [Test]
        public void Test()
        {
            var currentToken = new StringBuilder();

            Action<char> newChar = c => currentToken.Append(c);
            Action<char> newCell = c =>
            {
                Console.WriteLine(currentToken);
                currentToken.Length = 0;
            };

            Action<char> finalCell = c => { if (currentToken.Length > 0) Console.WriteLine(currentToken); };

            var machine = new Machine();

            var start = machine.NewState("start");
            var cell = machine.NewState("cell");
            var quotedCell = machine.NewState("quotedcell");
            var quotedCellEscape = machine.NewState("quotedcellescape");

            start
                .On(',').Exec(newCell)
                .On('"').Go(quotedCell)
                .On('\0').Exec(finalCell)
                .Default().Exec(newChar).Go(cell)
                ;

            cell
                .On(',').Exec(newCell).Go(start)
                .On('\0').Exec(finalCell)
                .Default().Exec(newChar)
                ;

            quotedCell
                .On('"').Go(quotedCellEscape)
                .On(',').Exec(newChar)
                .On('\0').Exec(finalCell)
                .Default().Exec(newChar)
                ;

            quotedCellEscape
                .On('"').Exec(newChar).Go(quotedCell)
                .On(',').Exec(newCell).Go(start)
                .On('\0').Exec(finalCell)
                ;

            var subjects = new[]
            {
                "text,\"some double quoted \"\"text\"\"\",some single quoted 'text',\"some text, with a comma\"",
                "this row features cells with special characters as the first item,\"\"\"text\"\"\",'text featuring excel-escaped single-quote',\",text\"",
                "\"Mega cell, features all commas, and \"\"quotes\"\" that might break the parser's logic\",,,",
                "The next cell is going to be blank,,\"This is the third cell, we skipped one!\",",
            };

            foreach (var subject in subjects)
            {
                machine.CurrentState = start;
                foreach (var c in subject)
                {
                    machine.Accept(c);
                }
                machine.Accept('\0');
                Console.WriteLine("---------------------------");
            }

        }
        
    }
}
