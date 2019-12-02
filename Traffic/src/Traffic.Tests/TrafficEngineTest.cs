using Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Engine.Models;

namespace Traffic.Tests
{
    class TrafficEngineTest
    {
        [Test, Category("TrafficEngineTest")]
        [Description("Tests whether the appropriate exceptions are being thrown on initialising TrafficEngine with incorrect data")]
        public void When_InvalidEngineInitialize_Expect_Exception()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => new TrafficEngine("InvalidWeather", new string[] { "10", "15" }));
            Assert.That(ex1.ParamName, Is.EqualTo("weather"));

            var ex2 = Assert.Throws<ArgumentException>(() => new TrafficEngine("SUNNY", new string[] { "1M0", "-15" }));
            Assert.That(ex2.ParamName, Is.EqualTo("trafficSpeeds"));

            var ex3 = Assert.Throws<ArgumentOutOfRangeException>(() => new TrafficEngine("SUNNY", new string[] { "-10", "15" }));
            Assert.That(ex3.ParamName, Is.EqualTo("trafficSpeeds"));
            Assert.That(ex3.ActualValue, Is.EqualTo(-10));
        }

        public static IEnumerable<TestCaseData> SampleIO()
        {
            yield return new TestCaseData("RAINY", new string[] { "40", "25" }, "CAR ORBIT2");
            yield return new TestCaseData("SUNNY", new string[] { "12", "10" }, "TUKTUK ORBIT1");
        }
        [Test, Category("TrafficEngineTest"), TestCaseSource("SampleIO")]
        [Description("Tests whether the appropriate exceptions are being thrown on initialising Orbit with incorrect data")]
        public void When_SampleInput_Expect_SampleOutput(string weather, string[] trafficSpeeds, string expected)
        {
            var testTrafficEngine = new TrafficEngine(weather, trafficSpeeds);

            var actual = testTrafficEngine.FindFastestRouteAndVehicle();

            Assert.That(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        [Test, Category("TrafficEngineTest")]
        [Description("Tests that in case of a tie, the vehicle with higher precedence is selected")]
        public void When_Tie_Expect_HigherPrecedenceChoosen()
        {
            var testWeather = "SUNNY";
            var testTrafficSpeeds = new string[] { "100", "100" };
            var lowerPrecedenceVehicle = new Vehicle("LowerPrecedence", 0, 10, 1, new List<Weathers>() { Weathers.Sunny });
            var higherPrecedenceVehicle = new Vehicle("HigherPrecedence", 1, 10, 1, new List<Weathers>() { Weathers.Sunny });
            var expected = $"{higherPrecedenceVehicle.Name} ORBIT1";

            var backup = TrafficEngine.Vehicles;

            TrafficEngine.Vehicles.Clear();
            TrafficEngine.Vehicles.Add(lowerPrecedenceVehicle);
            TrafficEngine.Vehicles.Add(higherPrecedenceVehicle);

            var testTrafficEngine = new TrafficEngine(testWeather, testTrafficSpeeds);

            // restore
            TrafficEngine.Vehicles = backup;

            var actual = testTrafficEngine.FindFastestRouteAndVehicle();

            Assert.That(string.Compare(expected, actual, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
