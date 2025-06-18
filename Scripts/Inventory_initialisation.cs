//Dominik von Ehrenstein

using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory_initialisation : Node
{
    public static Dictionary<string,string>[] return_inv_dictionaries()
    {
        Dictionary<string,string>[] dictlist = new Dictionary<string, string>[] {new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>()};
        //position 0: initial_effect, 1: continuous_effect, 2: end_effect
        dictlist[0].Add("healing_effect", "instant_healing");
		dictlist[1].Add("healing_effect", "regeneration");
		dictlist[2].Add("healing_effect", "nothing");
		
		dictlist[0].Add("jumpboost", "jumpboost");
		dictlist[1].Add("jumpboost", "nothing");
		dictlist[2].Add("jumpboost", "jumpboost");
        

        return dictlist;
    }

    public static Dictionary<string,string>[] return_inv_dictionaries_version_b()
    {
        Dictionary<string,string> initial_effect = new Dictionary<string, string>();
        Dictionary<string,string> continuous_effect = new Dictionary<string, string>();
        Dictionary<string,string> end_efect = new Dictionary<string, string>();

        initial_effect.Add("healing_effect", "instant_healing");
		continuous_effect.Add("healing_effect", "regeneration");
		end_efect.Add("healing_effect", "healing");
		
		initial_effect.Add("jumpboost", "jumpboost");
		continuous_effect.Add("jumpboost", "nothing");
		end_efect.Add("jumpboost", "jumpboost");
        

        return new Dictionary<string, string>[] {initial_effect, continuous_effect, end_efect};
    }
}
