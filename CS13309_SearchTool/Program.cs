using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CS13309_SearchTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 1)
            {
                if (args[0].Equals("retrieve"))
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        String palabraBuscar = args[i].ToLower();
                        Console.WriteLine(args[i]);
                    }
                }
            }
        }
    }
}
