using Engine;
using Engine.Models;
using NUnit.Framework;
using System;

namespace Traffic.Tests
{
    class OrbitTests
    {
        [Test, Category("ModelTests"), Category("OrbitModelTest")]
        [Description("Tests whether the appropriate exceptions are being thrown on initialising Orbit with incorrect data")]
        public void When_InvalidOrbitInitialize_Expect_Exception()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => new Orbit("", 100, 10));
            Assert.That(ex1.ParamName, Is.EqualTo("name"));

            var ex2a = Assert.Throws<ArgumentOutOfRangeException>(() => new Orbit("ORBIT1", 0, 10));
            Assert.That(ex2a.ParamName, Is.EqualTo("distance"));
            Assert.That(ex2a.ActualValue, Is.EqualTo(0));

            var ex2b = Assert.Throws<ArgumentOutOfRangeException>(() => new Orbit("ORBIT1", -1, 10));
            Assert.That(ex2b.ParamName, Is.EqualTo("distance"));
            Assert.That(ex2b.ActualValue, Is.EqualTo(-1));

            var ex3 = Assert.Throws<ArgumentOutOfRangeException>(() => new Orbit("ORBIT1", 100, -1));
            Assert.That(ex3.ParamName, Is.EqualTo("craters"));
            Assert.That(ex3.ActualValue, Is.EqualTo(-1));
        }

        [Test, Category("ModelTests"), Category("OrbitModelTest")]
        [Description("Tests whether the UpdateWeatherConditions increases the number of craters on recieving a positive ChangeInCraters value")]
        public void When_PositiveChangeInCraters_Expect_Increase()
        {
            var originalCraters = 100;
            var testWeather = new Weather(Weathers.Rainy, 100);
            var testOrbit = new Orbit("TestOrbit", 1000, originalCraters);

            testOrbit.UpdateWeatherConditions(testWeather);

            Assert.AreEqual(testOrbit.Craters, 2 * originalCraters);
        }

        [Test, Category("ModelTests"), Category("OrbitModelTest")]
        [Description("Tests whether the UpdateWeatherConditions decreases the number of craters on recieving a negative ChangeInCraters value")]
        public void When_NegativeChangeInCraters_Expect_Decrease()
        {
            var originalCraters = 100;
            var testWeather = new Weather(Weathers.Sunny, -100);
            var testOrbit = new Orbit("TestOrbit", 1000, originalCraters);

            testOrbit.UpdateWeatherConditions(testWeather);

            Assert.AreEqual(testOrbit.Craters, 0);
        }

        // Test SetTrafficSpeed
        [Test, Category("ModelTests"), Category("OrbitModelTest")]
        [Description("Tests whether the SetTrafficSpeed throws an exception on recieving a negative value for speedOfTraffic")]
        public void When_NegativeSpeed_Expect_ArgumentException()
        {
            var testSpeedOfTraffic = -100;

            var testOrbit = new Orbit("TestOrbit", 1000, 10);

            var ex1 = Assert.Throws<ArgumentOutOfRangeException>(() => testOrbit.SetTrafficSpeed(testSpeedOfTraffic));
            Assert.That(ex1.ParamName, Is.EqualTo("speedOfTraffic"));
            Assert.That(ex1.ActualValue, Is.EqualTo(testSpeedOfTraffic));

            Assert.Zero(testOrbit.SpeedOfTraffic);
        }

        [Test, Category("ModelTests"), Category("OrbitModelTest")]
        [Description("Tests whether the SetTrafficSpeed updates the SpeedOfTraffic on recieving a non-negative value for speedOfTraffic")]
        public void When_PositiveSpeed_Expect_NotNull()
        {
            var testOrbit = new Orbit("TestOrbit", 1000, 10);
            var expected = 100;

            testOrbit.SetTrafficSpeed(expected);

            Assert.AreEqual(expected, testOrbit.SpeedOfTraffic);

            expected = 0;

            testOrbit.SetTrafficSpeed(expected);

            Assert.Zero(testOrbit.SpeedOfTraffic);
        }
    }
}