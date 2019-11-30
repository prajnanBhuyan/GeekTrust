using Engine;
using Engine.Factory;
using Engine.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TameOfThrones.Tests
{
    [TestFixture]
    class BreakerOfChainsTests
    {
        TestConsoleMethods consoleMethods;

        [OneTimeSetUp]
        public void BreakerOfChainsTestsInitialise()
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
        [Test, Category("BreakerOfChainsTest"), Category("CustomInputValidator"), TestCaseSource("InputDataValidFormat")]
        [Description("Checks that the BreakerOfChainsValidator function returns true for a valid custom input message")]
        public void When_ValidInput_Expect_True(string input)
        {
            // Arrange
            var breakerOfChains = new BreakerOfChains(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            var actual = breakerOfChains.CustomInputValidator(input);

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
        [Test, Category("BreakerOfChainsTest"), Category("CustomInputValidator"), TestCaseSource("InputDataInvalidFormat")]
        [Description("Checks that the BreakerOfChainsValidator function returns false for a invalid custom input message")]
        public void When_InvalidInput_Expect_False(string input)
        {
            // Arrange
            var breakerOfChains = new BreakerOfChains(consoleMethods);
            // we cure the input as the user input is being converted to lower and 
            // trimmed when being read from the console in the actual application
            input = input.ToLower().Trim();

            // Act
            var actual = breakerOfChains.CustomInputValidator(input);

            // Assert
            Assert.IsFalse(actual, $"Failed for [{input}]");
        }


        public static IEnumerable<TestCaseData> InputDataTooFewCompetitors()
        {
            // Test by passing each kingdom as the lone competitor
            foreach (Kingdoms kingdom in Enum.GetValues(typeof(Kingdoms))) yield return new TestCaseData(kingdom.ToString());
        }
        [Test, Category("BreakerOfChainsTest"), Category("CustomInputAction"), TestCaseSource("InputDataTooFewCompetitors")]
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
            var actual = breakerOfChains.CustomInputAction(input, testGameEngine);

            // Assert
            Assert.AreEqual(Utility.NoCompetingKingdomsMessage, actual);
        }


        [Test, Category("BreakerOfChainsTest"), Category("CustomInputAction")]
        [Description("Checks that the BreakerOfChainsAction function returns the appropriate error message on all kingdoms competing")]
        public void When_AllCompeting_Expect_AllCompetingMessage()
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            consoleMethods.SimulatedInput = string.Join(" ", Enum.GetNames(typeof(Kingdoms)));
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Act
            var actual = breakerOfChains.CustomInputAction(string.Empty, testGameEngine);

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
        [Test, Category("BreakerOfChainsTest"), Category("CustomInputAction"), TestCaseSource("OneValidAndOneInvalidKingdom")]
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
            var actual = breakerOfChains.CustomInputAction(string.Empty, testGameEngine);

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
        [Test, Category("BreakerOfChainsTest"), Category("CustomInputAction"), TestCaseSource("ValidKingdomInThrees")]
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
            var actual = breakerOfChains.CustomInputAction(string.Empty, testGameEngine);

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
        [Test, Category("BreakerOfChainsTest"), Category("BallotProcess"), TestCaseSource("ListOfMaxBallotRounds")]
        [Description("Checks that the FindRulerByBallot function stops after the specified number of rounds even if no king found")]
        public void When_RoundsExceed_Expect_NoRuler(int maxRounds)
        {
            // Arrange
            Utility.listOfMessages.Clear();
            Utility.listOfMessages.Add("123456789");
            var testGameEngine = new GameEngine(consoleMethods);
            consoleMethods.SimulatedInput = $"{Kingdoms.Air} {Kingdoms.Ice}";
            var breakerOfChains = new BreakerOfChains(consoleMethods);

            // Act
            var actual = breakerOfChains.CustomInputAction(string.Empty, testGameEngine);

            // Explicitly call Utility class' static constructor so as to revert changes to listOfMessages
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Utility).TypeHandle);

            // Assert
            Assert.That(testGameEngine.RulingKingdom == null &&
                        actual == Utility.BallotTookTooLongMessage);
        }
    }
}
