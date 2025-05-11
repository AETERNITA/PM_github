/* //Dominik von Ehrenstein

using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class items : Node
{
    private Dictionary<string, string> initial_effect = new Dictionary<string, string>();
	private Dictionary<string, string> continuous_effect = new Dictionary<string, string>();
	private Dictionary<string, string> end_efect = new Dictionary<string, string>();
    private bool initialised = false;

    public List<Dictionary<string, string>> return_init_dicts()
    {
        if(initialised)
        {
            List
        }
        else
        {
            PrintErr("inventory dictionarys not yet initialised");
        }
    }

    public void initialise_inventory_system ()
	{		
		initial_effect.Add("healing_effect", "instant_healing");
		continuous_effect.Add("healing_effect", "regeneration");
		end_efect.Add("healing_effect", "nothing");
		
		initial_effect.Add("jumpboost", "jumpboost");
		continuous_effect.Add("jumpboost", "nothing");
		end_efect.Add("jumpboost", "jumpboost");

        initialised = true;

	}



}
 */