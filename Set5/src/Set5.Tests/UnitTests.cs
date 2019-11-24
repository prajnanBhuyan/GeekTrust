using Engine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Problem1;
using Problem2;
using Engine.Interfaces;

namespace Set5.Tests
{
    class TestConsoleMethods : IConsoleMethods
    {
        public string SimulatedInput { private get; set; }
        public string SimulatedOutput { get; private set; }

        public void WriteLine(string message)
        {
            SimulatedOutput = message;
        }

        public string ReadLine()
        {
            return SimulatedInput;
        }
    }


    [TestFixture]
    public class Problem1Tests
    {
        TestConsoleMethods consoleMethods;

        [OneTimeSetUp]
        public void Problem1TestsInitialise()
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
        [Test]
        [Category("AGoldenCrownTest")]
        [Category("CustomInputValidator")]
        [TestCaseSource("InputDataValidFormat")]
        [Description("Checks that the AGoldenCrownValidator function returns true for a valid custom input message")]
        public void When_ValidInput_Expect_True(string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();

            // Act
            var actual = aGoldenCrown.AGoldenCrownValidator(input);

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
        [Test]
        [Category("AGoldenCrownTest")]
        [Category("CustomInputValidator")]
        [TestCaseSource("InputDataInvalidFormat")]
        [Description("Checks that the AGoldenCrownValidator function returns false for a invalid custom input message")]
        public void When_InvalidInput_Expect_False(string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();

            // Act
            var actual = aGoldenCrown.AGoldenCrownValidator(input);

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
        [Test]
        [Category("AGoldenCrownTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("InputDataInValidKingdom")]
        [Description("Checks that the AGoldenCrownAction function returns the appropriate error message")]
        public void When_InvalidKingdom_Expect_InvalidKingdomMessage(string invalidKingdomName, string input)
        {
            // Arrange
            var aGoldenCrown = new AGoldenCrown();
            var testGameEngine = new GameEngine(consoleMethods);
            input = string.Format(input, invalidKingdomName);
            var expected = string.Format(Utility.InvalidKingdomMessage, invalidKingdomName);

            // Act
            var actual = aGoldenCrown.AGoldenCrownAction(input, testGameEngine);

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
        [Test]
        [Category("AGoldenCrownTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("InputDataInvalidMessage")]
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
            aGoldenCrown.AGoldenCrownAction(input, testGameEngine);

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
        [Test]
        [Category("AGoldenCrownTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("InputDataValidKingdomAndMessage")]
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
            aGoldenCrown.AGoldenCrownAction(input, testGameEngine);

            // Assert
            Assert.That(testGameEngine.AllKingdoms[competingKingdom].Allies.Contains(kingdom) &&
                        testGameEngine.AllKingdoms[kingdom].Allies.Contains(competingKingdom));
        }


        [Test]
        [Category("AGoldenCrownTest")]
        [Category("FindingRuler")]
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
                aGoldenCrown.AGoldenCrownAction(@$"{kingdom}, ""{GameEngine.Emblems[kingdom]}""", testGameEngine);

            // Assert
            Assert.That(testGameEngine.RulingKingdom != null &&
                        testGameEngine.RulingKingdom.Name == competingKingdom &&
                        testGameEngine.RulingKingdom.Allies.Intersect(listOfKingdoms).Count() == reqRightToRule);
        }
    }


    [TestFixture]
    public class Problem2Tests
    {
        // Since the FindRulerByBallot function in the BreakerOfChains class is protected
        // we create a public method in this derived class so as to test its functionalities
        private class BreakerOfChainsTester : BreakerOfChains
        {
            public BreakerOfChainsTester(IConsoleMethods consoleMethods) : base(consoleMethods)
            {
            }

            public bool FindRulerByBallotCaller(ISoutheros southeros, int maxRounds, int messagesToChoose)
            {
                return base.FindRulerByBallot(southeros, maxRounds, messagesToChoose);
            }
        }
        
        TestConsoleMethods consoleMethods;

        [OneTimeSetUp]
        public void Problem2TestsInitialise()
        {
            consoleMethods = new TestConsoleMethods();
        }


        public static IEnumerable<TestCaseData> InputDataValidFormat()
        {
            // For the input to be considered valid, it should contain the words/phrases
            //  1. enter
            //  2. kingdoms competing
            yield return new TestCaseData(@"Enter the kingdoms competing to be the ruler:");
            yield return new TestCaseData(@"enter kingdoms competing");
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputValidator")]
        [TestCaseSource("InputDataValidFormat")]
        [Description("Checks that the BreakerOfChainsValidator function returns true for a valid custom input message")]
        public void When_ValidInput_Expect_True(string input)
        {
            // Arrange
            var breakerOfChains = new BreakerOfChains(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            var actual = breakerOfChains.BreakerOfChainsValidator(input);

            // Assert
            Assert.IsTrue(actual, $"Failed for [{input}]");
        }


        public static IEnumerable<TestCaseData> InputDataInvalidFormat()
        {
            // For the input to be considered valid, it should contain the words/phrases
            //  1. enter
            //  2. kingdoms competing
            yield return new TestCaseData("Enter the kingdoms wanting to be the ruler:");
            yield return new TestCaseData("Enter the kingdom competing to be the ruler:");
            yield return new TestCaseData("Kingdoms competing to be the ruler:");
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputValidator")]
        [TestCaseSource("InputDataInvalidFormat")]
        [Description("Checks that the BreakerOfChainsValidator function returns false for a invalid custom input message")]
        public void When_InvalidInput_Expect_False(string input)
        {
            // Arrange
            var breakerOfChains = new BreakerOfChains(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            var actual = breakerOfChains.BreakerOfChainsValidator(input);

            // Assert
            Assert.IsFalse(actual, $"Failed for [{input}]");
        }


        public static IEnumerable<TestCaseData> InputDataTooFewCompetitors()
        {
            // Test by passing each kingdom as the lone competitor
            foreach (Kingdoms kingdom in Enum.GetValues(typeof(Kingdoms))) yield return new TestCaseData(kingdom.ToString());
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("InputDataTooFewCompetitors")]
        [Description("Checks that the BreakerOfChainsAction function returns the appropriate error message on too few competitors")]
        public void When_NoCompetitor_Expect_NoCompetitorMessage(string input)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();
            consoleMethods.SimulatedInput = input;
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Act
            var actual = breakerOfChains.BreakerOfChainsAction(input, testGameEngine);

            // Assert
            Assert.AreEqual(Utility.NoCompetingKingdomsMessage, actual);
        }


        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputAction")]
        [Description("Checks that the BreakerOfChainsAction function returns the appropriate error message on all kingdoms competing")]
        public void When_AllCompeting_Expect_AllCompetingMessage()
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            consoleMethods.SimulatedInput = string.Join(" ", Enum.GetNames(typeof(Kingdoms)));
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Act
            var actual = breakerOfChains.BreakerOfChainsAction(string.Empty, testGameEngine);

            // Assert
            Assert.AreEqual(Utility.TooManyKingdomsMessage, actual);
        }


        public static IEnumerable<TestCaseData> OneValidAndOneInvalidKingdom()
        {
            // Test by kingdoms in pairs with an invalid character added to the second kingdom
            // This function will yeild results like:
            // Space _Land_ 
            // Land _Water_
            // Water _Ice_ etc.

            var totalKingdoms = Enum.GetValues(typeof(Kingdoms)).Length;
            for (int i = 0; i < totalKingdoms; i++) yield return new TestCaseData($"{(Kingdoms)i} _{((Kingdoms)((i + 1) % totalKingdoms))}_");
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("OneValidAndOneInvalidKingdom")]
        [Description("Checks that the BreakerOfChainsAction function returns the appropriate error messages when we pass one valid and one invalid kingdom")]
        public void When_OneValidOneInvalid_Expect_InvalidAndTooFewMessage(string input)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();
            consoleMethods.SimulatedInput = input;
            var breakerOfChains = new BreakerOfChains(consoleMethods);
            var invalidKingdom = input.Split(' ')[1];
            var expected = string.Format(Utility.InvalidKingdomMessage, invalidKingdom);
            expected += Utility.NoCompetingKingdomsMessage;

            // Act
            var actual = breakerOfChains.BreakerOfChainsAction(string.Empty, testGameEngine);

            // Assert
            Assert.AreEqual(expected, actual);
        }


        public static IEnumerable<TestCaseData> ValidKingdomInThrees()
        {
            // Test kingdoms in threes. This function will yeild results like:
            // Space Land Water
            // Land Water Ice
            // Water Ice Air etc.

            var totalKingdoms = Enum.GetValues(typeof(Kingdoms)).Length;
            for (int i = 0; i < totalKingdoms; i++) yield return new TestCaseData($"{(Kingdoms)i} {((Kingdoms)((i + 1) % totalKingdoms))} {((Kingdoms)((i + 2) % totalKingdoms))}");
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("CustomInputAction")]
        [TestCaseSource("ValidKingdomInThrees")]
        [Description("Checks that the BreakerOfChainsAction function either finds a ruler or returns the maximum rounds output")]
        public void When_ValidKingdoms_Expect_RulerCrowned(string input)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();
            consoleMethods.SimulatedInput = input;
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Act
            var actual = breakerOfChains.BreakerOfChainsAction(string.Empty, testGameEngine);

            // Assert
            Assert.That(testGameEngine.RulingKingdom != null ||
                        actual == Utility.BallotTookTooLongMessage);
        }


        public static IEnumerable<TestCaseData> ListOfMaxBallotRounds()
        {
            yield return new TestCaseData(0);
            yield return new TestCaseData(10);
            yield return new TestCaseData(100);
        }
        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("BallotProcess")]
        [TestCaseSource("ListOfMaxBallotRounds")]
        [Description("Checks that the FindRulerByBallot function stops after the specified number of rounds even if no king found")]
        public void When_RoundsExceed_Expect_NoRuler(int maxRounds)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.AllKingdoms[Kingdoms.Air].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Ice].IsCompeting = true;
            var messagesToChoose = 6;
            Utility.listOfMessages.Clear();
            Utility.listOfMessages.Add("123456789");
            var breakerOfChains = new BreakerOfChainsTester(consoleMethods);

            // Act
            var actual = breakerOfChains.FindRulerByBallotCaller(testGameEngine, maxRounds, messagesToChoose);
            // Explicitly call Utility class' static constructor so as to revert changes to listOfMessages
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Utility).TypeHandle);

            // Assert
            Assert.False(actual);
        }


        [Test]
        [Category("BreakerOfChainsTest")]
        [Category("BallotProcess")]
        [Description("Checks that the FindRulerByBallot function throws an Argument exception if messagesToChoose is 0")]
        public void When_MessagesToSendZero_Expect_ArgumentException()
        {
            // Arrange
            var maxRounds = 100;
            var messagesToChoose = 0;
            var testGameEngine = new GameEngine(consoleMethods);
            var breakerOfChains = new BreakerOfChainsTester(consoleMethods);

            // Assert
            var ex = Assert.Throws<ArgumentException>(() => breakerOfChains.FindRulerByBallotCaller(testGameEngine, maxRounds, messagesToChoose));
            Assert.That(ex.ParamName, Is.EqualTo("messagesToChoose"));
        }
    }



    public class EngineAndModelTests
    {
        TestConsoleMethods consoleMethods;

        [OneTimeSetUp]
        public void EngineAndModelTestsInitialise()
        {
            consoleMethods = new TestConsoleMethods();
        }


        // Check engine constructor error for emblem

        // Test Execute function exits when it should and not when it souldn't

        // Test the process input function for who is the ruler

        // Test the process input function for who are the allies

        // Test the process input function for a custom input scenario

        // Test Kingdom's SendMessage function

        // Test Kingdom's AccepAlliance function

        // Test Kingdom's BreakAlliances function
    }
}