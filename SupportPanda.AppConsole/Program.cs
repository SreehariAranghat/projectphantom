﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.AppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Amme, Mookambike");
            Console.WriteLine("Om Mookambikayaye Namaha");

            DbSetup.SetUp();

        }
    }
}
