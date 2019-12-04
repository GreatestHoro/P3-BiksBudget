using System;
using System.Collections.Generic;
using System.Text;

namespace BBCollection.BBObjects
{
    public class AdvancedFilterList // Is not used yet
    {
        public string label { get; set; }
        public string optionsname { get; set; }
        public List<FilterItem> optionsList { get; set; }
        public string lastOption { get; set; }

        public AdvancedFilterList(string _label, string _optionname, List<FilterItem> _optionsList)
        {
            label = _label;
            optionsname = _optionname;
            optionsList = _optionsList;
        }

        public AdvancedFilterList()
        {

        }
    }
}
