using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyQueue_Implementation.Core.Generators;
using MyQueue_Implementation.Modeling.Interfaces;

namespace MyQueue_Implementation.Test
{
    /// <summary>
    /// Summary description for StrNumberGeneratorUnitTest
    /// </summary>
    [TestClass]
    public class StrNumberGeneratorUnitTest
    {
        private readonly IStrNumberGenerator _generator;


        public StrNumberGeneratorUnitTest()
        {
            _generator = new StrNumberGenerator("###-###-##-##");
        }


        [TestMethod]
        public void GenerateSingle_Test()
        {
            var result = _generator.GenerateSingle();

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GenerateSingleByFormat_Test()
        {
            var result = _generator.GenerateByCustomFormat("+7(705)-###-##-##", '#');

            Assert.IsNotNull(result);
        }
    }
}
