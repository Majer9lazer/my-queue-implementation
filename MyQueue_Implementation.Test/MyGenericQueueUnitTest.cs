using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyQueue_Implementation.Core.MyGenericCollections;
using MyQueue_Implementation.Modeling.MyEntities;

namespace MyQueue_Implementation.Test
{
    [TestClass]
    public class MyGenericQueueUnitTest
    {
        [TestMethod]
        public void EnqueueTestMethod()
        {
            MyGenericQueue<Person> genericQueue = new MyGenericQueue<Person>();

            genericQueue.Enqueue(new Person(1, "+77051648241"));
            genericQueue.Enqueue(new Person(2, "+77051648252"));
            genericQueue.Enqueue(new Person(3, "+77051648255"));
            genericQueue.Enqueue(new Person(4, "+77051648244"));
            genericQueue.Enqueue(new Person(0, "+77051648233"));
            genericQueue.SortBy(d => d.PhoneNumber);

            Assert.IsTrue(genericQueue[0].Id == 0, genericQueue[0].PhoneNumber);
        }

        [TestMethod]
        public void InputTestData()
        {
            int input = 3;

            MyGenericQueue<Person> genericQueue = new MyGenericQueue<Person>();
            genericQueue.Enqueue(new Person(0, "+77051648233"));
            genericQueue.Enqueue(new Person(1, "+77051648241"));
            genericQueue.Enqueue(new Person(2, "+77051648252"));
            genericQueue.Enqueue(new Person(3, "+77051648255"));
            genericQueue.Enqueue(new Person(4, "+77051648244"));

            Person[] persons = genericQueue.GetNElements(input);
            Assert.AreEqual(input, persons.Length, $"persons.Length = {persons.Length}");
        }
    }
}
