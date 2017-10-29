using System;
using Funkmap.Common.Redis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Common.Tests
{
    [TestClass]
    public class AbstractSerializerTest
    {
        [TestMethod]
        public void SerializeAbstract()
        {
            var serializer = new NewtonSerializer();

            var one = new ConcreteOne() {Property = "one", ValueInt = 1};
            var second = new ConcreteSecond() {Property = "second", ValueDate = DateTime.Now};

            var jsonOne = serializer.Serialize(one);
            var jsonSecond = serializer.Serialize(second);

            var abstractOne = serializer.Deserialize<Abstract>(jsonOne);
            var abstractSecond = serializer.Deserialize<Abstract>(jsonSecond);

            Assert.AreEqual(one.Property, abstractOne.Property);
            Assert.AreEqual(second.Property, abstractSecond.Property);

            var concreteWithInnerAbstract = new ConcreteWithInnerAbstract() {Result = true, Abstract = one};

            var jsonConcreteWithInnerAbstract = serializer.Serialize(concreteWithInnerAbstract);
            var newConcreteWithInnerAbstract = serializer.Deserialize<ConcreteWithInnerAbstract>(jsonConcreteWithInnerAbstract);

            Assert.AreEqual(newConcreteWithInnerAbstract.Abstract.Property, concreteWithInnerAbstract.Abstract.Property);

        }
    }

    public abstract class Abstract
    {
        public string Property { get; set; }
    }

    public class ConcreteOne : Abstract
    {
        public int ValueInt { get; set; }
    }

    public class ConcreteSecond : Abstract
    {
        public DateTime ValueDate { get; set; }
    }

    public class ConcreteWithInnerAbstract
    {
        public bool Result { get; set; }
        public Abstract Abstract { get; set; }
    }
}
