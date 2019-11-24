﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class SortoptionsList
    {
        public List<Options> optionsList = new List<Options>()
        {
            new Options("Price (Low to High)", true),
            new Options("Price (High to Low)", false),
            new Options("Alphabetical order (A - Z)", false),
            new Options("Alphabetical order (Z - A)", false),
            new Options("Store (A - Z)", false),
            new Options("Store (Z - A", false)
        };

        public List<Options> GetList()
        {
            return optionsList;
        }
    }
    public class Options
    {
        public Options(string _filterName, bool _isEnabled)
        {
            filterName = _filterName;
            isEnabled = _isEnabled;
        }

        public string filterName { get; set; }
        public bool isEnabled { get; set; }

    }
}