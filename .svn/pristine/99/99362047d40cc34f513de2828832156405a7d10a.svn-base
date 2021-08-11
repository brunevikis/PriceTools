using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //string code = "SE(VAZ(127) <= 430; MAX(0; VAZ(127) - 90); 340)+VAZ(123)";
            string code = "SE(VAZ(125)<=190;(VAZ(125)*119)/190;SE(VAZ(125)<=209;119;SE(VAZ(125)<=250;VAZ(125)-90;160)))";
            //code =       @"SE(VAZ(125)<=190;(VAZ(125)*119)/190;SE(VAZ(125)<=209;119;SE(VAZ(125)<=250;VAZ(125)-90;160)))";
            //string code = " SE(VAZ(125)!19.0 | VAZ(125)<>50;21;33)";


            var codeA = Regex.Replace(code.ToLower().Trim(), @"vaz\((\d+)\)", @"vaz${1}");
            codeA = Regex.Replace(codeA, @"\be\b", @"and");
            codeA = Regex.Replace(codeA, @"&", @"and");
            codeA = Regex.Replace(codeA, @"\bou\b", @"or");
            codeA = Regex.Replace(codeA, @"\|", @"or");
            codeA = Regex.Replace(codeA, @"\b=\b", @"==");
            codeA = Regex.Replace(codeA, @"\b!\b", @"<>");
            codeA = Regex.Replace(codeA, @"\b~\b", @">=");
            codeA = Regex.Replace(codeA, @"\b@\b", @"<=");

            //var variablesRX = new Regex(@"vaz\((\d+)\)", RegexOptions.IgnoreCase);
            //var codeA =  variablesRX.Replace(code.ToLower().Replace(" ", ""), @"vaz${1}");

            Console.WriteLine(code);
            Console.WriteLine(codeA);



            var transformIF = new Func<string, string>((codeIn) =>
            {

                int condStart = -1;
                var codeOut = codeIn;

                condStart = codeOut.IndexOf("se(");
                if (condStart >= 0)
                {

                    codeOut = codeOut.Remove(condStart, 3);
                    var pCount = 0;
                    var condEnd = -1;
                    for (var i = condStart; i < codeOut.Length; i++)
                    {
                        if (codeOut[i] == '(') pCount++;
                        else if (codeOut[i] == ')') pCount--;
                        if (pCount == 0 && codeOut[i] == ';') { condEnd = i; break; }
                    }

                    codeOut = codeOut.Remove(condEnd, 1);

                    var ifStart = condEnd;

                    var ifEnd = -1;
                    for (var i = ifStart; i < codeOut.Length; i++)
                    {
                        if (codeOut[i] == '(') pCount++;
                        else if (codeOut[i] == ')') pCount--;
                        if (pCount == 0 && codeOut[i] == ';') { ifEnd = i; break; }
                    }

                    codeOut = codeOut.Remove(ifEnd, 1);

                    var elseStart = ifEnd;

                    var elseEnd = -1;
                    for (var i = elseStart; i < codeOut.Length; i++)
                    {
                        if (codeOut[i] == '(') pCount++;
                        else if (pCount == 0 && codeOut[i] == ')') { elseEnd = i; break; }
                        else if (codeOut[i] == ')') pCount--;
                    }

                    codeOut = codeOut.Remove(elseEnd, 1);


                    var condExp = codeOut.Substring(condStart, condEnd - condStart);
                    var ifExp = codeOut.Substring(ifStart, ifEnd - ifStart);
                    var elseExp = codeOut.Substring(elseStart, elseEnd - elseStart);

                    codeOut = codeOut.Remove(condStart, elseEnd - condStart);
                    codeOut = codeOut.Insert(condStart,
                        string.Format("(({0}) if ({1}) else ({2}))", ifExp, condExp, elseExp)
                        );


                    return codeOut;
                }

                return codeOut;
            });


            var cTemp = "";
            do
            {
                cTemp = codeA;
                codeA = transformIF(cTemp);

            } while (cTemp != codeA);

            codeA = codeA.Replace(';', ',');

            Console.WriteLine(codeA);

            ScriptEngine engine = Python.CreateEngine();

            var sSource = engine.CreateScriptSourceFromString(codeA, SourceCodeKind.Expression);
            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("vaz125", 130);

            Console.WriteLine(sSource.Execute(scope));
        }
    }
}

