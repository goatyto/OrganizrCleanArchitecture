using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class EntityTests
    {
        public static IEnumerable<object[]> TransientEqualityTestData = new List<object[]>
        {
            new object[] {new EntityStub(Guid.NewGuid()), new EntityStub()},
            new object[] {new EntityStub(), new EntityStub()}
        };

        [Fact]
        public void IsTransient_DefaultId_ReturnsTrue()
        {
            var entity = new EntityStub();

            entity.IsTransient().Should().Be(true);
        }

        [Fact]
        public void IsTransient_NonDefaultId_ReturnsFalse()
        {
            var entity = new EntityStub(Guid.NewGuid());

            entity.IsTransient().Should().Be(false);
        }

        [Fact]
        public void Equals_NonEntityObject_ReturnsFalse()
        {
            var entity = new EntityStub(Guid.NewGuid());
            var compareObject = new CompareObjectDummy();

            entity.Equals(compareObject).Should().Be(false);
        }

        [Fact]
        public void Equals_DifferentEntitesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var entity1 = new EntityStub(entityId);
            var entity2 = new CompareEntityDummy(entityId);

            entity1.Equals(entity2).Should().Be(false);
            entity2.Equals(entity1).Should().Be(false);
        }

        [Fact]
        public void Equals_SameEntityReference_ReturnsTrue()
        {
            var entity1 = new EntityStub(Guid.NewGuid());
            var entity2 = entity1;

            entity1.Equals(entity2).Should().Be(true);
            entity2.Equals(entity1).Should().Be(true);
        }

        [Theory, MemberData(nameof(TransientEqualityTestData))]
        public void Equals_TransientEntity_ReturnsFalse(EntityStub entity1, EntityStub entity2)
        {
            entity1.Equals(entity2).Should().Be(false);
            entity2.Equals(entity1).Should().Be(false);
        }

        [Fact]
        public void Equals_EntitiesWithSameIds_ReturnsTrue()
        {
            var entityId = Guid.NewGuid();

            var entity1 = new EntityStub(entityId);
            var entity2 = new EntityStub(entityId);

            entity1.Equals(entity2).Should().Be(true);
            entity2.Equals(entity1).Should().Be(true);
        }

        [Fact]
        public void GetHashCode_EntitiesWithSameIds_EqualHashCodes()
        {
            var entityId = Guid.NewGuid();

            var entity1 = new EntityStub(entityId);
            var entity2 = new EntityStub(entityId);

            entity1.GetHashCode().Should().Be(entity2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_TransientEntities_RandomHashCodes()
        {
            var entity1 = new EntityStub();
            var entity2 = new EntityStub();

            entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
        }

        [Fact]
        public void EqualityOperator_DifferentEntitesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var entity1 = new EntityStub(entityId);
            var entity2 = new CompareEntityDummy(entityId);

            (entity1 == entity2).Should().Be(false);
            (entity2 == entity1).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_SameEntityReference_ReturnsTrue()
        {
            var entity1 = new EntityStub(Guid.NewGuid());
            var entity2 = entity1;

            (entity1 == entity2).Should().Be(true);
            (entity2 == entity1).Should().Be(true);
        }

        [Theory, MemberData(nameof(TransientEqualityTestData))]
        public void EqualityOperator_TransientEntity_ReturnsFalse(EntityStub entity1, EntityStub entity2)
        {
            (entity1 == entity2).Should().Be(false);
            (entity2 == entity1).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_EntitiesWithSameIds_ReturnsTrue()
        {
            var entityId = Guid.NewGuid();

            var entity1 = new EntityStub(entityId);
            var entity2 = new EntityStub(entityId);

            (entity1 == entity2).Should().Be(true);
            (entity2 == entity1).Should().Be(true);
        }

        [Fact]
        public void EqualityOperator_NullLeftOperand_ReturnsFalse()
        {
            EntityStub entity1 = null;
            EntityStub entity2 = new EntityStub();

            (entity1 == entity2).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_NullOperands_ReturnsTrue()
        {
            EntityStub entity1 = null;
            EntityStub entity2 = null;

            (entity1 == entity2).Should().Be(true);
        }

        [Fact]
        public void InequalityOperator_EntitiesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var entity1 = new EntityStub(entityId);
            var entity2 = new EntityStub(entityId);

            (entity1 != entity2).Should().Be(false);
        }

        [Fact]
        public void InequalityOperator_EntitiesWithDifferentIds_ReturnsTrue()
        {
            var entity1 = new EntityStub(Guid.NewGuid());
            var entity2 = new EntityStub(Guid.NewGuid());

            (entity1 != entity2).Should().Be(true);
        }

        public class EntityStub : Entity<Guid>
        {
            public EntityStub(Guid id = default(Guid))
            {
                Id = id;
            }
        }

        private class CompareObjectDummy
        {

        }

        private class CompareEntityDummy : Entity<Guid>
        {
            public CompareEntityDummy(Guid id = default(Guid))
            {
                Id = id;
            }
        }
    }
}
