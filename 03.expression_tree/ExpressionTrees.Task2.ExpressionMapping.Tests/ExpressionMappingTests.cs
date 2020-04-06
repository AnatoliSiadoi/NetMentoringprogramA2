using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo { SomeInt = 1, SomeString = "Some string" };

            // Act
            var res = mapper.Map(foo);

            // Assert
            Assert.IsNotNull(res);
            Assert.IsNotNull(foo);
            Assert.AreEqual(foo.SomeInt, res.SomeInt);
            Assert.AreEqual(foo.SomeString, res.SomeString);
        }
    }
}
