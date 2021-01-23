using DG;
using System;
using System.Collections.Generic;

namespace QoL_Mods.Data_Classes
{
    public class WakeUpGroup
    {
        public List<WakeUpTaunt> WakeUpTaunts { get; set; }
        public String Name { get; set; }

        private readonly Style defaultStyle;

        public WakeUpGroup(String name)
        {
            WakeUpTaunts = new List<WakeUpTaunt>();
            Name = name;
            defaultStyle = new Style(Name, FightStyleEnum.American);
    }

    public void AddWakeUpMove(Skill skill, DamageState damageState)
        {
            //If the skill already exists under this damage state, do nothing
            foreach (WakeUpTaunt group in WakeUpTaunts)
            {
                var currentSkill = group.GetSkill(damageState);
                if (currentSkill == null)
                {
                    continue;
                }

                L.D("Current Skill: " + currentSkill.SkillName + " vs " + skill.SkillName);

                if (currentSkill.SkillName.Equals(skill.SkillName))
                {
                    L.D(skill + " already exists under " + damageState);
                    return;
                }
            }

            //Skill is not a duplicate, add it to the last group. Else, we need to make a new group
            WakeUpTaunt lastTaunt;

            if (WakeUpTaunts.Count == 0)
            {
                WakeUpTaunts.Add(new WakeUpTaunt(defaultStyle));
            }

            lastTaunt = WakeUpTaunts[WakeUpTaunts.Count - 1];

            if (lastTaunt.GetSkill(damageState) == null)
            {
                L.D("Append Wakeup Move");
                lastTaunt.AddWakeUpMove(skill, damageState);
            }
            else
            {
                L.D("Add new Wakeup Taunt");
                WakeUpTaunt newTaunt = new WakeUpTaunt(lastTaunt.StyleItem);
                newTaunt.AddWakeUpMove(skill, damageState);
                WakeUpTaunts.Add(newTaunt);
            }
        }

        public void RemoveWakeUpMove(Skill skill, DamageState damageState)
        {
            foreach (WakeUpTaunt group in WakeUpTaunts)
            {
                if (group.LocateSkill(skill, damageState) != null)
                {
                    group.RemoveWakeUpMove(damageState);
                    if (group.IsGroupEmpty())
                    {
                        WakeUpTaunts.Remove(group);
                    }
                    return;
                }
            }
        }

        public String SaveWakeUpData()
        {
            String data = "";
            foreach (WakeUpTaunt group in WakeUpTaunts)
            {
                data = group.SaveWakeUpData();
            }
            return data;
        }

        public void LoadWakeUpData(String data, HashSet<Skill> validSkills = null, Modversion version = Modversion.v1)
        {
            Style style = defaultStyle;
            WakeUpTaunt newTaunt = new WakeUpTaunt(style);
            Name = newTaunt.LoadWakeUpData(data, validSkills);
            WakeUpTaunts.Add(newTaunt);
        }

        public String GetTauntDataName(String data)
        {
            String[] information = data.Split('|');
            return information[0].Split(';')[0];
        }

        public override string ToString()
        {
            return Name;
        }

    }
}