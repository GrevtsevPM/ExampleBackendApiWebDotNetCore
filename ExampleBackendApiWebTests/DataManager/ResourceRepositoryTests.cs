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
    public class ResourceRepositoryTests
    {
        ResourceRepository<int> testObj;

        [TestInitialize]
        public void Setup()
        {
            var initialData = new Dictionary<int, string>();
            initialData.Add(1, "foo");
            initialData.Add(2, "bar");
            initialData.Add(4, "baz");
            initialData.Add(5, null);
            testObj = new ResourceRepository<int>(initialData);
        }

        [TestMethod()]
        public void ConstructorDefaultTest()
        {
            var testObjNoConstructor = new ResourceRepository<int>();
            Assert.IsNotNull(testObjNoConstructor);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            Assert.AreNotEqual(0, testObj.GetAll().Count());
            Assert.AreNotEqual(0, testObj.GetAll().First().value.Length);
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            ModifyDataResultEnum operationResult;
            Assert.AreNotEqual(0, testObj.GetById(2, out operationResult).Length);
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);

            Assert.IsNull(testObj.GetById(10, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, operationResult);
        }

        [TestMethod()]
        public void GetSubstringTest()
        {
            ModifyDataResultEnum operationResult;

            Assert.IsNull(testObj.GetSubstring(10, 1, 3, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, operationResult);

            Assert.IsNull(testObj.GetSubstring(5, 1, 3, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrStringNull, operationResult);

            Assert.IsNull(testObj.GetSubstring(1,1,3, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrIndexOutside, operationResult);

            Assert.AreEqual("oo",testObj.GetSubstring(1, 1, 2, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);
        }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.AreEqual(ModifyDataResultEnum.ErrConflict, testObj.Create(1));
            Assert.AreEqual(ModifyDataResultEnum.Success, testObj.Create(12,"teststr"));
        }

        [TestMethod()]
        public void ReplaceItemTest()
        {
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, testObj.ReplaceItem(10,"newval"));
            Assert.AreEqual(ModifyDataResultEnum.Success, testObj.ReplaceItem(1, "newval"));
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, testObj.Delete(10));
            Assert.AreEqual(ModifyDataResultEnum.Success, testObj.Delete(1));
        }

        [TestMethod()]
        public void UpdateInsertSubstrTest()
        {
            ModifyDataResultEnum operationResult;

            Assert.IsNull(testObj.UpdateInsertSubstr(10,"", UpdateResourceCommandTypeEnum.InsertAtStart, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, operationResult);

            Assert.IsNull(testObj.UpdateInsertSubstr(5, "", UpdateResourceCommandTypeEnum.InsertAtStart, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrStringNull, operationResult);

            Assert.IsNull(testObj.UpdateInsertSubstr(1, "", UpdateResourceCommandTypeEnum.InsertAtIndex, out operationResult, 3));
            Assert.AreEqual(ModifyDataResultEnum.ErrIndexOutside, operationResult);

            Assert.AreEqual("aafoo", testObj.UpdateInsertSubstr(1, "aa", UpdateResourceCommandTypeEnum.InsertAtStart, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);

            Assert.AreEqual("aafooaa", testObj.UpdateInsertSubstr(1, "aa", UpdateResourceCommandTypeEnum.InsertAtEnd, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);

            Assert.AreEqual("aaaafooaa", testObj.UpdateInsertSubstr(1, "aa", UpdateResourceCommandTypeEnum.InsertAtIndex, out operationResult, 2));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);
        }

        [TestMethod()]
        public void UpdateRemoveSubstrTest()
        {
            ModifyDataResultEnum operationResult;

            Assert.IsNull(testObj.UpdateRemoveSubstr(10, 1, 2, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, operationResult);

            Assert.IsNull(testObj.UpdateRemoveSubstr(1, 1, 3, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrIndexOutside, operationResult);

            Assert.AreEqual("f", testObj.UpdateRemoveSubstr(1, 1, 2, out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);
        }

        [TestMethod()]
        public void UpdateReplaceSubstr()
        {
            ModifyDataResultEnum operationResult;

            Assert.IsNull(testObj.UpdateReplaceSubstr(10, "oo", "aa", out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.ErrNotFound, operationResult);

            Assert.AreEqual("faa", testObj.UpdateReplaceSubstr(1, "oo", "aa", out operationResult));
            Assert.AreEqual(ModifyDataResultEnum.Success, operationResult);
        }
    }
}