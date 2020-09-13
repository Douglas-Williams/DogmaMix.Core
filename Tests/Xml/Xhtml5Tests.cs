using System;
using System.Collections.Generic;
using System.Text;
using DogmaMix.Core.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DogmaMix.Core.Tests.Xml
{
    [TestClass]
    public class Xhtml5Tests
    {
        [TestMethod]
        public void EntitiesDtd()
        {
            StringAssert.Contains(Xhtml5.EntitiesDtd, "<!ENTITY starf \"&#9733;\">");
        }
    }
}
