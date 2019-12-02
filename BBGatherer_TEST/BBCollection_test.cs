using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBGatherer_TEST
{
    [TestClass]
    class BBCollection_Test
    {
        string _MySqlTestWithDB = @"server=localhost;database=" + _DatabaseName + ";userid=root;password=BiksBudget123";
        string _MySqlTestWithoutDB = @"server=localhost;userid=root;password=BiksBudget123";

    }
}
