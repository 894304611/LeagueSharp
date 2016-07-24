﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Core;
using DZAIO_Reborn.Helpers;
using DZAIO_Reborn.Helpers.Entity;
using DZAIO_Reborn.Helpers.Modules;
using DZAIO_Reborn.Plugins.Interface;
using DZLib.Core;
using DZLib.Menu;
using DZLib.MenuExtensions;
using DZLib.Positioning;
using LeagueSharp;
using LeagueSharp.Common;
using SPrediction;

namespace DZAIO_Reborn.Plugins.Champions.Warwick
{
    class Warwick : IChampion
    {
        private float currentELevel = 0;

        public void OnLoad(Menu menu)
        {
            var comboMenu = new Menu(ObjectManager.Player.ChampionName + ": Combo", "dzaio.champion.warwick.combo");
            {
                comboMenu.AddModeMenu(ModesMenuExtensions.Mode.Combo, new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }, new[] { true, true, true, true });
                comboMenu.AddSlider("dzaio.champion.warwick.combo.r.min", "Min Enemies for R", 2, 1, 5);
                menu.AddSubMenu(comboMenu);
            }

            var mixedMenu = new Menu(ObjectManager.Player.ChampionName + ": Mixed", "dzaio.champion.warwick.harrass");
            {
                mixedMenu.AddModeMenu(ModesMenuExtensions.Mode.Harrass, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });
                mixedMenu.AddSlider("dzaio.champion.warwick.mixed.mana", "Min Mana % for Harass", 30, 0, 100);
                menu.AddSubMenu(mixedMenu);
            }

            var farmMenu = new Menu(ObjectManager.Player.ChampionName + ": Farm", "dzaio.champion.warwick.farm");
            {
                farmMenu.AddModeMenu(ModesMenuExtensions.Mode.Laneclear, new[] { SpellSlot.Q, SpellSlot.W }, new[] { true, true });
                farmMenu.AddSlider("dzaio.champion.warwick.farm.mana", "Min Mana % for Farm", 30, 0, 100);
                menu.AddSubMenu(farmMenu);
            }

            var extraMenu = new Menu(ObjectManager.Player.ChampionName + ": Extra", "dzaio.champion.warwick.extra");
            {
                extraMenu.AddBool("dzaio.champion.warwick.extra.autoQKS", "Q KS", true);
            }

            Variables.Spells[SpellSlot.Q].SetTargetted(0.25f, 2000f);
            Variables.Spells[SpellSlot.R].SetSkillshot(0.25f, 175, 700, false, SkillshotType.SkillshotCircle);

        }

        public void RegisterEvents()
        {
            DZInterrupter.OnInterruptableTarget += OnInterrupter;
            DZAntigapcloser.OnEnemyGapcloser += OnGapcloser;
            Orbwalking.AfterAttack += AfterAttack;
        }


        private void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {

        }

        private void OnGapcloser(DZLib.Core.ActiveGapcloser gapcloser)
        {

        }

        private void OnInterrupter(Obj_AI_Hero sender, DZInterrupter.InterruptableTargetEventArgs args)
        {

        }
        public Dictionary<SpellSlot, Spell> GetSpells()
        {
            return new Dictionary<SpellSlot, Spell>
                      {
                                    { SpellSlot.Q, new Spell(SpellSlot.Q, 400f) },
                                    { SpellSlot.W, new Spell(SpellSlot.W, 1250f) },
                                    { SpellSlot.E, new Spell(SpellSlot.E, 670f + (770f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level)) },
                                    { SpellSlot.R, new Spell(SpellSlot.R, 700f) }
                      };
        }

        public List<IModule> GetModules()
        {
            return new List<IModule>()
            {
            };
        }

        public void OnTick()
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level > currentELevel)
            {
                Variables.Spells[SpellSlot.E].Range = 670f +
                                                  (770f * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level);
                currentELevel = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).Level;
            }
        }

        public void OnCombo()
        {
            if (Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo))
            {
                var qTarget = Variables.Spells[SpellSlot.Q].GetTarget();
                if (qTarget.IsValidTarget())
                {
                    Variables.Spells[SpellSlot.Q].CastOnUnit(qTarget);
                }
            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Combo) && ObjectManager.Player.CountEnemiesInRange(Variables.Spells[SpellSlot.W].Range) > 0)
            {
                    Variables.Spells[SpellSlot.W].Cast();
            }
        }

        public void OnMixed()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.warwick.mixed.mana").Value)
            {
                return;
            }

            if (Variables.Spells[SpellSlot.Q].IsEnabledAndReady(ModesMenuExtensions.Mode.Harrass))
            {
                var qTarget = Variables.Spells[SpellSlot.Q].GetTarget();
                if (qTarget.IsValidTarget())
                {
                    Variables.Spells[SpellSlot.Q].CastOnUnit(qTarget);
                }
            }

            if (Variables.Spells[SpellSlot.W].IsEnabledAndReady(ModesMenuExtensions.Mode.Harrass) 
                && ObjectManager.Player.CountEnemiesInRange(Variables.Spells[SpellSlot.W].Range) > 0)
            {
                Variables.Spells[SpellSlot.W].Cast();
            }
        }

        public void OnLastHit()
        { }

        public void OnLaneclear()
        {
            if (ObjectManager.Player.ManaPercent <
                Variables.AssemblyMenu.GetItemValue<Slider>("dzaio.champion.warwick.farm.mana").Value)
            {
                return;
            }

           
        }

    }
}
