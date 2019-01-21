using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyQueue_Implementation.Core.Generator;
using MyQueue_Implementation.Core.MyCollections;

namespace MyQueue_Implementation.Test
{
    [TestClass]
    public class MyQueueUnitTest
    {
        [TestMethod]
        public void MyQueueTestMethodWith3PhoneNumbers()
        {
            int input = 3;
            string[] phoneNumbers =
            {
                "+77777777", "+88888888", "+99999999","+1000000", "+3333333"
            };
            MyQueue myQueue = new MyQueue();
            PersonGenerator generator = new PersonGenerator();
            Random _random = new Random();
            for (int i = 0; i < phoneNumbers.Length; i++)
            {
                myQueue.Enqueue(generator.GenerateSingle(_random.Next(1, i + 1000)));
            }
            var aNElements = myQueue.GetNElements(input);
            Assert.AreEqual(3, aNElements.Length);
            Debug.WriteLine("Test passed successfully");
        }
    }
}
