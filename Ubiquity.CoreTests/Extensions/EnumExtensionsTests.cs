using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ubiquity.Core.Extensions.Tests
{
    internal enum TestEnum
    { 
        [System.ComponentModel.Description("SampleText1")]
        Test1
    }


    [TestClass()]
    public class EnumExtensionsTests
    {
        [TestMethod()]
        public void GetDescriptionTest()
        {
            TestEnum _enum = TestEnum.Test1;
            string _expected = "SampleText1";
            string _actual = _enum.GetDescription();
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod()]
        public void GetAttributeDescriptionTest()
        {
            TestEnum _enum = TestEnum.Test1;
            string _expected = "SampleText1";
            string _actual = _enum.GetAttribute<System.ComponentModel.DescriptionAttribute>().Description;
            Assert.AreEqual(_expected, _actual);
        }
    }
}