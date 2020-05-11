using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExampleBackendApiWeb.DataManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ExampleBackendApiWeb.Enums;

namespace ExampleBackendApiWeb.DataManager.Tests
{
    [TestClass()]
    public class DataManagerClassTests
    {
        DataManagerClass testObj;

        [TestInitialize]
        public void Setup()
        {
            var initialData = new Dictionary<int, string>();
            initialData.Add(1, "foo");
            initialData.Add(2, "bar");
            initialData.Add(4, "baz");
            initialData.Add(5, null);
            testObj = new DataManagerClass(initialData);
        }

        [TestMethod()]
        public void ConstructorDefaultTest()
        {
            var testObjNoConstructor = new DataManagerClass();
            Assert.IsNotNull(testObjNoConstructor);
            Assert.IsNotNull(testObjNoConstructor.resourceRepository);
        }

        [TestMethod()]
        public void CreateTest()
        {
            ModifyDataResultEnum operationResult;

            Assert.IsTrue(testObj.CreateWithNewId("testval", out operationResult) > 0);
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);
        }
    }
}