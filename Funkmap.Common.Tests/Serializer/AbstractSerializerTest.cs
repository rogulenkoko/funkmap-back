using System;
using Funkmap.Common.Redis;
using Funkmap.Common.Redis.Options;
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

            var options = new SerializerOptions() {HasAbstractMember = true};

            var jsonOne = serializer.Serialize(one, options);
            var jsonSecond = serializer.Serialize(second, options);

            var abstractOne = serializer.Deserialize<Abstract>(jsonOne, options);
            var abstractSecond = serializer.Deserialize<Abstract>(jsonSecond, options);

            Assert.AreEqual(one.Property, abstractOne.Property);
            Assert.AreEqual(second.Property, abstractSecond.Property);

            var concreteWithInnerAbstract = new ConcreteWithInnerAbstract() {Result = true, Abstract = one};

            var jsonConcreteWithInnerAbstract = serializer.Serialize(concreteWithInnerAbstract, options);
            var newConcreteWithInnerAbstract = serializer.Deserialize<ConcreteWithInnerAbstract>(jsonConcreteWithInnerAbstract, options);

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
