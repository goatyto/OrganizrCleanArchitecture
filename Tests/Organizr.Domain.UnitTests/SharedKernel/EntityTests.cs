using System;
using System.Collections.Generic;
using FluentAssertions;
using Organizr.Domain.SharedKernel;
using Xunit;

namespace Organizr.Domain.UnitTests.SharedKernel
{
    public class EntityTests
    {
        [Fact]
        public void AddDomainEvent_ValidEvent_EventAddedToList()
        {
            var sut = new EntityStub();
            var initialDomainEventCount = sut.DomainEvents.Count;
            var domainEvent = new DomainEventStub();

            sut.AddDomainEvent(domainEvent);

            sut.DomainEvents.Should().HaveCount(initialDomainEventCount + 1).And
                .ContainSingle(@event => @event == domainEvent);
        }

        [Fact]
        public void RemoveDomainEvent_ValidEvent_EventRemovedToList()
        {
            var sut = new EntityStub();
            var domainEvent = new DomainEventStub();

            sut.AddDomainEvent(domainEvent);
            var initialDomainEventCount = sut.DomainEvents.Count;

            sut.RemoveDomainEvent(domainEvent);

            sut.DomainEvents.Should().HaveCount(initialDomainEventCount -1).And
                .NotContain(@event => @event == domainEvent);
        }

        [Fact]
        public void ClearDomainEvents_DomainEventListCleared()
        {
            var sut = new EntityStub();
            var domainEvent = new DomainEventStub();

            sut.AddDomainEvent(domainEvent);
            
            sut.ClearDomainEvents();

            sut.DomainEvents.Should().BeEmpty();
        }

        public static IEnumerable<object[]> TransientEqualityTestData = new List<object[]>
        {
            new object[] {new EntityStub(Guid.NewGuid()), new EntityStub()},
            new object[] {new EntityStub(), new EntityStub()}
        };

        [Fact]
        public void IsTransient_DefaultId_ReturnsTrue()
        {
            var sut = new EntityStub();

            sut.IsTransient().Should().Be(true);
        }

        [Fact]
        public void IsTransient_NonDefaultId_ReturnsFalse()
        {
            var sut = new EntityStub(Guid.NewGuid());

            sut.IsTransient().Should().Be(false);
        }

        [Fact]
        public void Equals_NonEntityObject_ReturnsFalse()
        {
            var sut = new EntityStub(Guid.NewGuid());
            var compareObject = new CompareObjectDummy();

            sut.Equals(compareObject).Should().Be(false);
        }

        [Fact]
        public void Equals_DifferentEntitesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var sut1 = new EntityStub(entityId);
            var sut2 = new CompareEntityDummy(entityId);

            sut1.Equals(sut2).Should().Be(false);
            sut2.Equals(sut1).Should().Be(false);
        }

        [Fact]
        public void Equals_SameEntityReference_ReturnsTrue()
        {
            var sut1 = new EntityStub(Guid.NewGuid());
            var sut2 = sut1;

            sut1.Equals(sut2).Should().Be(true);
            sut2.Equals(sut1).Should().Be(true);
        }

        [Theory, MemberData(nameof(TransientEqualityTestData))]
        public void Equals_TransientEntity_ReturnsFalse(EntityStub sut1, EntityStub sut2)
        {
            sut1.Equals(sut2).Should().Be(false);
            sut2.Equals(sut1).Should().Be(false);
        }

        [Fact]
        public void Equals_EntitiesWithSameIds_ReturnsTrue()
        {
            var entityId = Guid.NewGuid();

            var sut1 = new EntityStub(entityId);
            var sut2 = new EntityStub(entityId);

            sut1.Equals(sut2).Should().Be(true);
            sut2.Equals(sut1).Should().Be(true);
        }

        [Fact]
        public void GetHashCode_EntitiesWithSameIds_EqualHashCodes()
        {
            var entityId = Guid.NewGuid();

            var sut1 = new EntityStub(entityId);
            var sut2 = new EntityStub(entityId);

            sut1.GetHashCode().Should().Be(sut2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_TransientEntities_RandomHashCodes()
        {
            var sut1 = new EntityStub();
            var sut2 = new EntityStub();

            sut1.GetHashCode().Should().NotBe(sut2.GetHashCode());
        }

        [Fact]
        public void EqualityOperator_DifferentEntitesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var sut1 = new EntityStub(entityId);
            var sut2 = new CompareEntityDummy(entityId);

            (sut1 == sut2).Should().Be(false);
            (sut2 == sut1).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_SameEntityReference_ReturnsTrue()
        {
            var sut1 = new EntityStub(Guid.NewGuid());
            var sut2 = sut1;

            (sut1 == sut2).Should().Be(true);
            (sut2 == sut1).Should().Be(true);
        }

        [Theory, MemberData(nameof(TransientEqualityTestData))]
        public void EqualityOperator_TransientEntity_ReturnsFalse(EntityStub sut1, EntityStub sut2)
        {
            (sut1 == sut2).Should().Be(false);
            (sut2 == sut1).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_EntitiesWithSameIds_ReturnsTrue()
        {
            var entityId = Guid.NewGuid();

            var sut1 = new EntityStub(entityId);
            var sut2 = new EntityStub(entityId);

            (sut1 == sut2).Should().Be(true);
            (sut2 == sut1).Should().Be(true);
        }

        [Fact]
        public void EqualityOperator_NullLeftOperand_ReturnsFalse()
        {
            EntityStub sut1 = null;
            EntityStub sut2 = new EntityStub();

            (sut1 == sut2).Should().Be(false);
        }

        [Fact]
        public void EqualityOperator_NullOperands_ReturnsTrue()
        {
            EntityStub sut1 = null;
            EntityStub sut2 = null;

            (sut1 == sut2).Should().Be(true);
        }

        [Fact]
        public void InequalityOperator_EntitiesWithSameIds_ReturnsFalse()
        {
            var entityId = Guid.NewGuid();
            var sut1 = new EntityStub(entityId);
            var sut2 = new EntityStub(entityId);

            (sut1 != sut2).Should().Be(false);
        }

        [Fact]
        public void InequalityOperator_EntitiesWithDifferentIds_ReturnsTrue()
        {
            var sut1 = new EntityStub(Guid.NewGuid());
            var sut2 = new EntityStub(Guid.NewGuid());

            (sut1 != sut2).Should().Be(true);
        }

        public class EntityStub : Entity<Guid>
        {
            public EntityStub(Guid id = default(Guid), IEnumerable<IDomainEvent> domainEvents =  null)
            {
                Identity = id;

                if(domainEvents != null)
                    foreach(var domainEvent in domainEvents) 
                        AddDomainEvent(domainEvent);
            }
        }

        private class CompareObjectDummy
        {

        }

        private class CompareEntityDummy : Entity<Guid>
        {
            public CompareEntityDummy(Guid id = default(Guid))
            {
                Identity = id;
            }
        }

        public class DomainEventStub : IDomainEvent
        {

        }
    }
}
