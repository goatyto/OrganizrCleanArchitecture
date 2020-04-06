using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class ValueObjectTests
    {
        [Fact]
        public void EqualityOperator_EqualValueObjects_ReturnsTrue()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, prop2);

            (valueObject1 == valueObject2).Should().Be(true);
            (valueObject2 == valueObject1).Should().Be(true);
        }

        [Fact]
        public void EqualityOperator_SingleOperandNullReference_ReturnsFalse()
        {
            ValueObjectStub valueObject1 = null;
            ValueObjectStub valueObject2 = new ValueObjectStub("prop1", "prop2");

            (valueObject1 == valueObject2).Should().Be(false);
            (valueObject2 == valueObject1).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_BothOperandsNullReference_ReturnsTrue()
        {
            ValueObjectStub valueObject1 = null;
            ValueObjectStub valueObject2 = null;

            (valueObject1 == valueObject2).Should().Be(true);
            (valueObject2 == valueObject1).Should().Be(true);
        }

        [Fact]
        public void InequalityOperator_EqualValueObjects_ReturnsFalse()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, prop2);

            (valueObject1 != valueObject2).Should().Be(false);
            (valueObject2 != valueObject1).Should().Be(false);
        }

        [Fact]
        public void InequalityOperator_SingleOperandNullReference_ReturnsTrue()
        {
            ValueObjectStub valueObject1 = null;
            ValueObjectStub valueObject2 = new ValueObjectStub("prop1", "prop2");

            (valueObject1 != valueObject2).Should().Be(true);
            (valueObject2 != valueObject1).Should().Be(true);
        }

        [Fact]
        public void InequalityOperator_BothOperandsNullReference_ReturnsFalse()
        {
            ValueObjectStub valueObject1 = null;
            ValueObjectStub valueObject2 = null;

            (valueObject1 != valueObject2).Should().Be(false);
            (valueObject2 != valueObject1).Should().Be(false);
        }

        [Fact]
        public void Equals_EqualValueObjects_ReturnsTrue()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, prop2);

            valueObject1.Equals(valueObject2).Should().Be(true);
            valueObject2.Equals(valueObject1).Should().Be(true);
        }

        [Fact]
        public void Equals_DifferentValueObjects_ReturnsFalse()
        {
            var valueObject1 = new ValueObjectStub("prop11", "prop12");
            var valueObject2 = new ValueObjectStub("prop21", "prop22");

            valueObject1.Equals(valueObject2).Should().Be(false);
            valueObject2.Equals(valueObject1).Should().Be(false);
        }

        [Fact]
        public void Equals_DifferentTypesOfValueObject_ReturnsFalse()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var prop3 = "prop3";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub2(prop1, prop2, prop3);

            valueObject1.Equals(valueObject2).Should().Be(false);
            valueObject2.Equals(valueObject1).Should().Be(false);
        }

        [Fact]
        public void Equals_NonValueObject_ReturnsFalse()
        {
            var valueObject = new ValueObjectStub("prop1", "prop2");
            var dummyObject = new CompareObjectDummy();

            valueObject.Equals(dummyObject).Should().Be(false);
        }

        [Fact]
        public void Equals_NullReference_ReturnsFalse()
        {
            ValueObjectStub valueObject1 = new ValueObjectStub("prop1", "prop2");
            ValueObjectStub valueObject2 = null;

            valueObject1.Equals(valueObject2).Should().Be(false);
        }

        [Fact]
        public void Equals_NullParameterOtherValueObject_ReturnsFalse()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, null);

            valueObject1.Equals(valueObject2).Should().Be(false);
            valueObject2.Equals(valueObject1).Should().Be(false);
        }

        [Fact]
        public void Equals_DifferentListParameters_ReturnsFalse()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, prop2, new[] { "item1" });

            valueObject1.Equals(valueObject2).Should().Be(false);
            valueObject2.Equals(valueObject1).Should().Be(false);
        }

        [Fact]
        public void GetHashCode_ValueObjectsSameValues_ReturnsIdenticalHashCode()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject1 = new ValueObjectStub(prop1, prop2);
            var valueObject2 = new ValueObjectStub(prop1, prop2);

            valueObject1.GetHashCode().Should().Be(valueObject2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ValueObjectsDifferentValues_ReturnsFalse()
        {
            var valueObject1 = new ValueObjectStub("prop11", "prop12");
            var valueObject2 = new ValueObjectStub("prop21", "prop22");

            valueObject1.GetHashCode().Should().NotBe(valueObject2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_NullParameters_ReturnsZero()
        {
            var valueObject1 = new ValueObjectStub(null, null);

            valueObject1.GetHashCode().Should().Be(0);
        }

        [Fact]
        public void GetCopy_CreatesCopy()
        {
            var prop1 = "prop1";
            var prop2 = "prop2";
            var valueObject = new ValueObjectStub(prop1, prop2);
            var valueObjectCopy = valueObject.GetCopy();

            valueObjectCopy.Should().Be(valueObject);
        }
    }

    public class ValueObjectStub : ValueObject
    {
        public string Prop1 { get; private set; }
        public string Prop2 { get; private set; }
        public IEnumerable<string> List { get; private set; }

        public ValueObjectStub(string prop1, string prop2, IEnumerable<string> list = null)
        {
            Prop1 = prop1;
            Prop2 = prop2;

            List = list ?? new List<string>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Prop1;
            yield return Prop2;

            foreach (var item in List)
            {
                yield return item;
            }
        }
    }

    public class ValueObjectStub2 : ValueObject
    {
        public string Prop1 { get; private set; }
        public string Prop2 { get; private set; }
        public string Prop3 { get; private set; }

        public ValueObjectStub2(string prop1, string prop2, string prop3)
        {
            Prop1 = prop1;
            Prop2 = prop2;
            Prop3 = prop3;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Prop1;
            yield return Prop2;
            yield return Prop3;
        }
    }

    public class CompareObjectDummy
    {

    }
}
