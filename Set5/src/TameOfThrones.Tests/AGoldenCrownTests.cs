using Engine;
using Engine.Factory;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TameOfThrones.Tests
{
    [TestFixture]
    class AGoldenCrownTests

    {
        TestConsoleMethods consoleMethods;

        [OneTimeSetUp]
        public void AGoldenCrownTestsInitialise()
        {
            consoleMethods = new TestConsoleMethods();
        }

        // The only competing kingdom as per the problem is the SpaceKingdom. This should 
        // be the same as the value in AGoldenCrownAction for valid testing
        readonly Kingdoms competingKingdom = Kingdoms.Space;
        // Required right to rule is the number of kingdoms the competingKingdom kingdom
        // needs to be allied with in order to be crowned as the ruler. This too should 
        // be the same as the value in AGoldenCrownAction for valid testing
        readonly int reqRightToRule = 3;

        public static IEnumerable<TestCaseData> InputDataValidFormat()
        {
            // For the input to be considered valid, it should contain:
            //  1. Some text at the beginning [Target kingdom]
            //  2. Some text following (1) enclosed in double quotes with atleast one character between them
            yield return new TestCaseData(@"air, ""owl""");
            yield return new TestCaseData(@"air""owl""");
            yield return new TestCaseData(@"air""o""w""l""");
            yield return new TestCaseData(@"   air  , "".;][ ow]apsd svdf [] l""");
            yield return new TestCaseData(@"air, ""1""");
            yield return new TestCaseData(@"qwewqe""qwueqwie""");
        }
        [Test, Category("AGoldenCrownTest"), Category("CustomInputValidator"), TestCaseSource("InputDataValidFormat")]
        [Description("Checks that the AGoldenCrownValidator function returns true for a valid custom input message")]
        public void When_ValidInput_Expect_True(string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();

            // Act
            var actual = aGoldenCrown.CustomInputValidator(input);

            // Assert
            Assert.IsTrue(actual, $"Failed for [{input}]");
        }


        public static IEnumerable<TestCaseData> InputDataInvalidFormat()
        {
            // For the input to be considered valid, it should contain:
            //  1. Some text at the beginning [Target kingdom]
            //  2. Some text following (1) enclosed in double quotes with atleast one character between them
            yield return new TestCaseData(@"""owl""");
            yield return new TestCaseData(@"air, owl");
            yield return new TestCaseData(@"air owl""");
            yield return new TestCaseData(@"air""owl");
            yield return new TestCaseData(@"air, 'owl'");
            yield return new TestCaseData(@"air, """"");
        }
        [Test, Category("AGoldenCrownTest"), Category("CustomInputValidator"), TestCaseSource("InputDataInvalidFormat")]
        [Description("Checks that the AGoldenCrownValidator function returns false for a invalid custom input message")]
        public void When_InvalidInput_Expect_False(string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();

            // Act
            var actual = aGoldenCrown.CustomInputValidator(input);

            // Assert
            Assert.IsFalse(actual, $"Failed for [{input}]");
        }


        public static IEnumerable<TestCaseData> InputDataInValidKingdom()
        {
            // For the Kingdom to be considered valid, it should match with a value from the Kingdoms enum
            yield return new TestCaseData("A1r", @"{0}, ""Let's swing the sword together""");
            yield return new TestCaseData("L@nd", @"{0}, ""Die or play the tame of thrones""");
            yield return new TestCaseData("Ice!", @"{0}, ""Ahoy! Fight for me with men and money""");
            yield return new TestCaseData("Water_", @"{0}, ""Summer is coming""");
            yield return new TestCaseData("Firey", @"{0}, ""Drag on Martin!""");
            yield return new TestCaseData("Spa(e", @"{0}, ""Go risk it all""");
        }
        [Test, Category("AGoldenCrownTest"), Category("CustomInputAction"), TestCaseSource("InputDataInValidKingdom")]
        [Description("Checks that the AGoldenCrownAction function returns the appropriate error message")]
        public void When_InvalidKingdom_Expect_InvalidKingdomMessage(string invalidKingdomName, string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();
            var testGameEngine = new GameEngine(consoleMethods);
            input = string.Format(input, invalidKingdomName);
            var expected = string.Format(Utility.InvalidKingdomMessage, invalidKingdomName);

            // Act
            var actual = aGoldenCrown.CustomInputAction(input, testGameEngine);

            // Assert
            Assert.AreEqual(expected, actual);
        }


        public static IEnumerable<TestCaseData> InputDataInvalidMessage()
        {
            // For the Kingdom to be considered valid, it should match with a value from the Kingdoms enum
            yield return new TestCaseData(@"Air, ""Ow1""", Kingdoms.Air);
            yield return new TestCaseData(@"Land, ""Pan|)a""", Kingdoms.Land);
            yield return new TestCaseData(@"Ice, ""Mamm0th""", Kingdoms.Ice);
            yield return new TestCaseData(@"Water, ""Octopu5""", Kingdoms.Water);
            yield return new TestCaseData(@"Fire, ""Dr@gon""", Kingdoms.Fire);
            // Although Gorilla is the correct emblem animal of the space kingdom, 
            // we don't expect the kingdom to form an alliance with itself
            yield return new TestCaseData(@"Space, ""Gorilla""", Kingdoms.Space);
        }
        [Test, Category("AGoldenCrownTest"), Category("CustomInputAction"), TestCaseSource("InputDataInvalidMessage")]
        [Description("Checks that no alliance is formed when the correct kingdom but invalid message is passed")]
        public void When_ValidKingdomInvalidMessage_Expect_NoAlliance(string input, Kingdoms kingdom)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();
            var testGameEngine = new GameEngine(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            aGoldenCrown.CustomInputAction(input, testGameEngine);

            // Assert
            Assert.That(!testGameEngine.AllKingdoms[competingKingdom].Allies.Contains(kingdom) &&
                        !testGameEngine.AllKingdoms[kingdom].Allies.Contains(competingKingdom));
        }


        public static IEnumerable<TestCaseData> InputDataValidKingdomAndMessage()
        {
            // For the Kingdom to be considered valid, it should match with a value from the Kingdoms enum
            yield return new TestCaseData(@"Air, ""Owl""", Kingdoms.Air);
            yield return new TestCaseData(@"Land, ""Panda""", Kingdoms.Land);
            yield return new TestCaseData(@"Ice, ""Mammoth""", Kingdoms.Ice);
            yield return new TestCaseData(@"Water, ""Octopus""", Kingdoms.Water);
            yield return new TestCaseData(@"Fire, ""Dragon""", Kingdoms.Fire);
            //yield return new TestCaseData(@"Space, ""Gorilla""", Kingdoms.Space);
        }
        [Test, Category("AGoldenCrownTest"), Category("CustomInputAction"), TestCaseSource("InputDataValidKingdomAndMessage")]
        [Description("Checks that an alliance is formed when the correct kingdom and a valid message is passed")]
        public void When_ValidKingdomAndMessage_Expect_AllianceMade(string input, Kingdoms kingdom)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();
            var testGameEngine = new GameEngine(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            aGoldenCrown.CustomInputAction(input, testGameEngine);

            // Assert
            Assert.That(testGameEngine.AllKingdoms[competingKingdom].Allies.Contains(kingdom) &&
                        testGameEngine.AllKingdoms[kingdom].Allies.Contains(competingKingdom));
        }


        [Test, Category("AGoldenCrownTest"), Category("FindingRuler")]
        [Description("Checks that an alliance is formed when the correct kingdom and a valid message is passed")]
        public void When_WinRequiredAllies_Expect_ToBecomeRuler()
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();
            var testGameEngine = new GameEngine(consoleMethods);
            var listOfKingdoms = new List<Kingdoms>(reqRightToRule)
            {
                // Enter an reqRightToRule number of valid enums here
                Kingdoms.Air,
                Kingdoms.Land,
                Kingdoms.Ice,
            };

            // Act
            foreach (var kingdom in listOfKingdoms)
                aGoldenCrown.CustomInputAction(@$"{kingdom}, ""{GameEngine.Emblems[kingdom]}""", testGameEngine);

            // Assert
            Assert.That(testGameEngine.RulingKingdom != null &&
                        testGameEngine.RulingKingdom.Name == competingKingdom &&
                        testGameEngine.RulingKingdom.Allies.Intersect(listOfKingdoms).Count() == reqRightToRule);
        }
    }
}
