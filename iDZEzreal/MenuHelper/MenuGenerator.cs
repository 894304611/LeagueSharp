﻿using DZLib;
using DZLib.MenuExtensions;
using LeagueSharp.Common;

namespace iDZEzreal.MenuHelper
{
    class MenuGenerator
    {
        public static void Generate()
        {
            var rootMenu = Variables.Menu;
            var owMenu = new Menu("[Ez] Ow", "ezreal.orbwalker");
            {
                Variables.Orbwalker = new Orbwalking.Orbwalker(owMenu);
                rootMenu.AddSubMenu(owMenu);
            }

            var comboMenu = new Menu("[Ez] Combo","ezreal.combo");
            {
                comboMenu.AddBool("ezreal.combo.q", "Use Q", true);
                comboMenu.AddBool("ezreal.combo.w", "Use W", true);
                comboMenu.AddBool("ezreal.combo.r", "Use R", true);
                comboMenu.AddSlider("ezreal.combo.r.min", "Min Enemies", 2, 1, 5);
                rootMenu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu("[Ez] Harass", "ezreal.mixed");
            {
                mixedMenu.AddBool("ezreal.mixed.q", "Use Q", true);
                mixedMenu.AddBool("ezreal.mixed.w", "Use W", true);
                rootMenu.AddSubMenu(mixedMenu);
            }


            rootMenu.AddToMainMenu();
        }
    }
}
