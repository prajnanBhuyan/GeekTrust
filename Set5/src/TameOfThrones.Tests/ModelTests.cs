using Engine;
using Engine.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TameOfThrones.Tests
{
    class ModelTests
    {
        TestConsoleMethods consoleMethods;


        [OneTimeSetUp]
        public void ModelTestsInitialise()
        {
            consoleMethods = new TestConsoleMethods();
        }


        public static IEnumerable<TestCaseData> GetKingdomsTwoAtATime()
        {
            var totalKingdoms = Enum.GetValues(typeof(Kingdoms)).Length;
            for (int i = 0; i < totalKingdoms; i++) yield return new TestCaseData((Kingdoms)i, (Kingdoms)((i + 1) % totalKingdoms));
        }
        [Test, Category("ModelTests"), Category("KingdomModelTest"), TestCaseSource("GetKingdomsTwoAtATime")]
        [Description("Tests the Kingdom class' SendMessage and AccepAlliance function")]
        public void When_SendValidMessage_Expect_AllyAdded(Kingdoms sendingKingdom, Kingdoms receivingKingdom)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            var message = new Message(sendingKingdom, receivingKingdom, testGameEngine.AllKingdoms[receivingKingdom].Emblem);

            // Act
            testGameEngine.AllKingdoms[sendingKingdom].SendMessage(testGameEngine.AllKingdoms[receivingKingdom], message);

            // Assert
            Assert.AreEqual(testGameEngine.AllKingdoms[sendingKingdom].Allies.FirstOrDefault(), receivingKingdom);
            Assert.AreEqual(testGameEngine.AllKingdoms[receivingKingdom].Allies.FirstOrDefault(), sendingKingdom);
        }


        public static IEnumerable<TestCaseData> TestDataSubListOfAllies()
        {
            var totalKingdoms = Enum.GetValues(typeof(Kingdoms)).Length;
            for (int i = 0; i < totalKingdoms; i++) yield return new TestCaseData(new List<Kingdoms> { (Kingdoms)i, (Kingdoms)((i + 1) % totalKingdoms), (Kingdoms)((i + 2) % totalKingdoms), (Kingdoms)((i + 3) % totalKingdoms) });
        }
        [Test, Category("ModelTests"), Category("KingdomModelTest"), TestCaseSource("TestDataSubListOfAllies")]
        [Description("Tests the Kingdom class' BreakAlliances function")]
        public void When_BreakAlliances_Expect_AllAlliesDropped(List<Kingdoms> testData)
        {
            // Arrange
            var testKingdom = testData[0];
            var allies = testData.Skip(1);
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.AllKingdoms[testKingdom].Allies.AddRange(allies);

            // Act
            testGameEngine.AllKingdoms[testKingdom].BreakAlliances();

            // Assert
            Assert.Zero(testGameEngine.AllKingdoms[testKingdom].Allies.Count);
        }


        [Test, Category("ModelTests"), Category("HighPriestTest")]
        [Description("Checks that the HighPriest constructor throws an Argument exception if messagesToChoose or maxRounds is less than 1")]
        public void When_InvalidInitializer_Expect_ArgumentException()
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);

            // Assert
            var ex1 = Assert.Throws<ArgumentException>(() => new HighPriest(0, 100, consoleMethods));
            Assert.That(ex1.ParamName, Is.EqualTo("messagesToChoose"));
            var ex2 = Assert.Throws<ArgumentException>(() => new HighPriest(100, 0, consoleMethods));
            Assert.That(ex2.ParamName, Is.EqualTo("maxRounds"));
        }


        [Test, Category("ModelTests"), Category("HighPriestTest")]
        [Description("Checks that the FindRulerByBallot function throws an Argument exception if messagesToChoose is 0")]
        public void When_KingCrowned_Expect_True()
        {
            // Arrange
            // Setting all kingdoms except water to compete
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.AllKingdoms[Kingdoms.Air].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Fire].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Ice].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Land].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Space].IsCompeting = true;
            // Updating message list to only contain valid message for water
            // So that we are sure to have a winner in the very first round
            Utility.listOfMessages.Clear();
            Utility.listOfMessages.Add("octopus");
            var highPriest = new HighPriest(6, 100, consoleMethods);

            // Act
            highPriest.HoldBallot(testGameEngine);

            // Explicitly call Utility class' static constructor so as to revert changes to listOfMessages
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Utility).TypeHandle);

            // Assert
            Assert.NotNull(testGameEngine.RulingKingdom);
        }


        [Test, Category("ModelTests"), Category("HighPriestTest")]
        [Description("Checks that the FindRulerByBallot function throws an Argument exception if messagesToChoose is 0")]
        public void When_NoKing_Expect_False()
        {
            // Arrange
            // Setting all kingdoms except water to compete
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.AllKingdoms[Kingdoms.Air].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Fire].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Ice].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Land].IsCompeting = true;
            testGameEngine.AllKingdoms[Kingdoms.Space].IsCompeting = true;
            // Updating message list to only contain invalid messages so
            // that we are sure to not have a winner
            Utility.listOfMessages.Clear();
            Utility.listOfMessages.Add("no_winner_expected");
            var highPriest = new HighPriest(6, 100, consoleMethods);

            // Act
            highPriest.HoldBallot(testGameEngine);

            // Explicitly call Utility class' static constructor so as to revert changes to listOfMessages
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Utility).TypeHandle);

            // Assert
            Assert.Null(testGameEngine.RulingKingdom);
        }
    }
}
