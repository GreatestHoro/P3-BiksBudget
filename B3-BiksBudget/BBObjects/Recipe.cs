﻿using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.BBObjects
{
    class Recipe
    {
        public string _Name;
        public string _description;
        public List<Ingriedient> _ingrediensList;
        public float _PerPerson;


        public Recipe(string name, string description, List<Ingriedient> ingrediensList, float PerPerson)
        {
            _Name = name;
            _description = description;
            _ingrediensList = ingrediensList;
        }
    }
}
