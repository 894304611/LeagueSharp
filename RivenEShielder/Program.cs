﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace RivenEShielder
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CustomEvents.Game.OnGameLoad += EShielder.OnLoad;
        }
    }
}
