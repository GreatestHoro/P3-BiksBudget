using System;
using System.Collections.Generic;
using System.Text;

namespace B3_BiksBudget.Webcrawler.Assisting_classes
{
    class AssistingClasses
    {
        CrossRefrenceFuntionality _refs = new CrossRefrenceFuntionality();
        Ingrdient_Determiner _Detminer = new Ingrdient_Determiner();
        StringCleanUpFuntionality _clean = new StringCleanUpFuntionality();

        public CrossRefrenceFuntionality getRefs() 
        {
            return _refs;
        }
        public Ingrdient_Determiner getDetermin() 
        {
            return _Detminer;
        }
        public StringCleanUpFuntionality getCleanFunc() 
        {
            return _clean;
        }
    }
}
