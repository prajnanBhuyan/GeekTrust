using Engine;
using Engine.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Tests
{
    class VehicleTests
    {
        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests whether the appropriate exceptions are being thrown on initialising Vehicle with incorrect data")]
        public void When_InvalidOrbitInitialize_Expect_Exception()
        {
            var testSuitableWeather = new List<Weathers> { Weathers.Sunny };

            var ex1 = Assert.Throws<ArgumentException>(() => new Vehicle("", 0, 100, 1, testSuitableWeather));
            Assert.That(ex1.ParamName, Is.EqualTo("name"));

            var ex2a = Assert.Throws<ArgumentOutOfRangeException>(() => new Vehicle("TestVehicle", 0, 0, 1, testSuitableWeather));
            Assert.That(ex2a.ParamName, Is.EqualTo("speed"));
            Assert.That(ex2a.ActualValue, Is.EqualTo(0));

            var ex2b = Assert.Throws<ArgumentOutOfRangeException>(() => new Vehicle("TestVehicle", 0, -10, 1, testSuitableWeather));
            Assert.That(ex2b.ParamName, Is.EqualTo("speed"));
            Assert.That(ex2b.ActualValue, Is.EqualTo(-10));

            var ex3 = Assert.Throws<ArgumentOutOfRangeException>(() => new Vehicle("TestVehicle", 0, 100, -1, testSuitableWeather));
            Assert.That(ex3.ParamName, Is.EqualTo("timePerCrater"));
            Assert.That(ex3.ActualValue, Is.EqualTo(-1));

            var ex4 = Assert.Throws<ArgumentException>(() => new Vehicle("TestVehicle", 0, 100, 1, new List<Weathers>()));
            Assert.That(ex4.ParamName, Is.EqualTo("suitableWeathers"));
        }

        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests that the GetTimeRequired function uses the max speed of the vehicle when it is not being capped by the TrafficSpeed")]
        public void When_NoTrafficSpeedLimit_Expect_MaxVehicleSpeed()
        {
            var testDistance = 100;
            var testVehicleSpeed = 10;
            var testTrafficSpeed = 50;
            var expected = decimal.Divide(testDistance, testVehicleSpeed);
            var testOrbit = new Orbit("TestOrbit", testDistance, 0);
            var testVehicle = new Vehicle("TestVehicle", 1, testVehicleSpeed, 1, new List<Weathers>() { Weathers.Sunny });

            testOrbit.SetTrafficSpeed(testTrafficSpeed);
            var actual = testVehicle.GetTimeRequired(testOrbit);

            Assert.AreEqual(expected, actual);
        }

        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests that the GetTimeRequired function uses the TrafficSpeed when it is lower than the speed of the vehicle")]
        public void When_TrafficSpeedLimit_Expect_TrafficSpeed()
        {
            var testDistance = 100;
            var testVehicleSpeed = 10;
            var testTrafficSpeed = 5;
            var expected = decimal.Divide(testDistance, testTrafficSpeed);
            var testOrbit = new Orbit("TestOrbit", testDistance, 0);
            var testVehicle = new Vehicle("TestVehicle", 1, testVehicleSpeed, 1, new List<Weathers>() { Weathers.Sunny });

            testOrbit.SetTrafficSpeed(testTrafficSpeed);
            var actual = testVehicle.GetTimeRequired(testOrbit);

            Assert.AreEqual(expected, actual);
        }

        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests that the GetTimeRequired function returns the correct amount of time required for a given scenarios")]
        public void When_TimePerCrater60Mins_Expect_1Hour()
        {
            var testDistance = 10;
            var testVehicleSpeed = 10;
            var testTrafficSpeed = 10;
            var testNoOfCraters = 1;
            var testTimePerCraters = 60;
            var expected = 2;
            var testOrbit = new Orbit("TestOrbit", testDistance, testNoOfCraters);
            var testVehicle = new Vehicle("TestVehicle", 1, testVehicleSpeed, testTimePerCraters, new List<Weathers>() { Weathers.Sunny });

            testOrbit.SetTrafficSpeed(testTrafficSpeed);
            var actual = testVehicle.GetTimeRequired(testOrbit);

            Assert.AreEqual(expected, actual);
        }

        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests whether the HasPrecedenceOver function returns true when a vehicle has a higher precedence value")]
        public void When_HigherPrecedenceValue_Expect_True()
        {
            var testVehicle1 = new Vehicle("BIKE1", 2, 10, 2, new List<Weathers>() { Weathers.Sunny });
            var testVehicle2 = new Vehicle("BIKE2", 1, 10, 2, new List<Weathers>() { Weathers.Sunny });

            Assert.IsTrue(testVehicle1.HasPrecedenceOver(testVehicle2));
        }

        [Test, Category("ModelTests"), Category("VehicleModelTest")]
        [Description("Tests whether the HasPrecedenceOver function returns false when a vehicle has a lower precedence value")]
        public void When_LowerOrEqualPrecedenceValue_Expect_False()
        {
            var testVehicle1 = new Vehicle("BIKE1", 2, 10, 2, new List<Weathers>() { Weathers.Sunny });
            var testVehicle2 = new Vehicle("BIKE2", 1, 10, 2, new List<Weathers>() { Weathers.Sunny });
            var testVehicle3 = new Vehicle("BIKE3", 1, 10, 2, new List<Weathers>() { Weathers.Sunny });

            // Returns false when precedence is higher ...
            Assert.IsFalse(testVehicle2.HasPrecedenceOver(testVehicle1));
            // ... or equal
            Assert.IsFalse(testVehicle2.HasPrecedenceOver(testVehicle3));
        }
    }
}
