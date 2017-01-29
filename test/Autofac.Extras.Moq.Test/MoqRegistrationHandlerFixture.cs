﻿using System.Collections.Generic;
using Autofac.Core;
using Xunit;

namespace Autofac.Extras.Moq.Test
{
    public class MoqRegistrationHandlerFixture
    {
        private MoqRegistrationHandler _systemUnderTest;

        public MoqRegistrationHandlerFixture()
        {
            this._systemUnderTest = new MoqRegistrationHandler();
        }

        private interface ISomethingStartable : IStartable
        {
        }

        private interface ITestGenericInterface<T>
        {
        }

        private interface ITestInterface
        {
        }

        [Fact]
        public void RegistrationForConcreteClass_IsHandled()
        {
            var registrations = GetRegistrations<TestConcreteClass>();

            Assert.NotEmpty(registrations);
        }

        [Fact]
        public void RegistrationForSealedConcreteClass_IsNotHandled()
        {
            var registrations = GetRegistrations<TestSealedConcreteClass>();

            Assert.Empty(registrations);
        }

        [Fact]
        public void RegistrationForNonTypedService_IsNotHandled()
        {
            var registrations = this._systemUnderTest.RegistrationsFor(
                new KeyedService(serviceKey: "key", serviceType: typeof(string)),
                registrationAccessor: null);

            Assert.Empty(registrations);
        }

        [Fact]
        public void RegistrationsForAbstractClass_IsHandled()
        {
            var registrations = GetRegistrations<TestAbstractClass>();

            Assert.NotEmpty(registrations);
        }

        [Fact]
        public void RegistrationsForArrayType_IsNotHandled()
        {
            var registrations = GetRegistrations<ITestInterface[]>();

            Assert.Empty(registrations);
        }

        [Fact]
        public void RegistrationsForGenericType_IsHandled()
        {
            var registrations = GetRegistrations<ITestGenericInterface<TestConcreteClass>>();

            Assert.NotEmpty(registrations);
        }

        [Fact]
        public void RegistrationsForIEnumerable_IsNotHandled()
        {
            var registrations = GetRegistrations<IEnumerable<ITestInterface>>();

            Assert.Empty(registrations);
        }

        [Fact]
        public void RegistrationsForInterface_IsHandled()
        {
            var registrations = GetRegistrations<ITestInterface>();

            Assert.NotEmpty(registrations);
        }

        [Fact]
        public void RegistrationsForIStartableType_IsNotHandled()
        {
            var registrations = GetRegistrations<ISomethingStartable>();

            Assert.Empty(registrations);
        }

        private IEnumerable<IComponentRegistration> GetRegistrations<T>()
        {
            return this._systemUnderTest.RegistrationsFor(
                new TypedService(typeof(T)),
                registrationAccessor: null);
        }

        private abstract class TestAbstractClass
        {
        }

        private class TestConcreteClass
        {
        }

        private sealed class TestSealedConcreteClass
        {
        }
    }
}
