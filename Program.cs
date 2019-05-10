using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrentPrincipalDNF
{
    class Program
    {
        static Random random = new Random();

        static AsyncLocal<string> _AsyncLocalName = new AsyncLocal<string>();

        static void Main(string[] args)
        {
            for (var i = 0; i < 50; i++)
            {
                Task.Factory.StartNew((o) => Test(o), i.ToString());
            }
            Console.ReadLine();
        }

        static async void Test(object state)
        {
            var name = (string)state;
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(name), new string[] { });
            _AsyncLocalName.Value = name;
            Debug.Assert(name == _AsyncLocalName.Value);                    //always true
            Debug.Assert(name == Thread.CurrentPrincipal.Identity.Name);    //always true
            Debug.Assert(Thread.CurrentPrincipal == ClaimsPrincipal.Current);
            await Task.Delay(random.Next(1000));
            Debug.Assert(name == _AsyncLocalName.Value);                    //always true
            Debug.Assert(Thread.CurrentPrincipal == ClaimsPrincipal.Current);
            Debug.Assert(name == Thread.CurrentPrincipal.Identity.Name);    //may fail, even more, Thread.CurrentPrincipal may equal null

        }
    }
}
