using System.Collections.Generic;

namespace BBCollection.BBObjects
{
    public class AdvancedFilterList
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

        public AdvancedFilterList(string _label, string _optionname, List<FilterItem> _optionsList, string _lastOption)
        {
            label = _label;
            optionsname = _optionname;
            optionsList = _optionsList;
            lastOption = _lastOption;
        }

        public AdvancedFilterList() { }
    }
}
