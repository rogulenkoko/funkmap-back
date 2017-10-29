using System;
using Funkmap.Common.Redis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Common.Tests.Serializer
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var foo = new Foo() {String = "foo",DateTime = DateTime.Now,Int = 2};
            var serializer = new NewtonSerializer();
            var jsonFoo = serializer.Serialize(foo);
            var newFoo = serializer.Deserialize<Foo>(jsonFoo);
            Assert.AreEqual(newFoo.String, foo.String);
            Assert.AreEqual(newFoo.Int, foo.Int);
            Assert.AreEqual(newFoo.DateTime, foo.DateTime);

            var jsonNull = serializer.Serialize(null);
            var deserealizedFooNull = serializer.Deserialize<Foo>(jsonNull);
            var deserealizedNull = serializer.Deserialize<Foo>(null);
            Assert.IsNull(deserealizedNull);
            Assert.IsNull(deserealizedFooNull);

        }
    }

    public class Foo
    {
        public string String { get; set; }
        public int Int { get; set; }
        public DateTime DateTime { get; set; }
    }
}
