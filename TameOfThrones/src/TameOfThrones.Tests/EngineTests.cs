using Engine;
using Engine.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TameOfThrones.Tests
{
    [TestFixture]
    class EngineTests
    {
        TestConsoleMethods consoleMethods;
        MultiIOConsoleMethods multiIOConsoleMethods;


        [OneTimeSetUp]
        public void EngineTestsInitialise()
        {
            consoleMethods = new TestConsoleMethods();
            multiIOConsoleMethods = new MultiIOConsoleMethods();
        }


        public static IEnumerable<TestCaseData> GetKingdomsOneAtATime()
        {
            // Test by passing each kingdom as the lone competitor
            foreach (Kingdoms kingdom in Enum.GetValues(typeof(Kingdoms))) yield return new TestCaseData(kingdom);
        }
        [Test, Category("EngineTests"), TestCaseSource("GetKingdomsOneAtATime")]
        [Description("Checks that the GameEngine constructor throws KeyNotFoundException when an emblem for a kingdom is not defined")]
        public void When_NoEmblemDefined_Expect_KeyNotFoundException(Kingdoms testKingdom)
        {
            // Arrange
            var emblem = GameEngine.Emblems[testKingdom];
            GameEngine.Emblems.Remove(testKingdom);

            // Assert
            var ex1 = Assert.Throws<KeyNotFoundException>(() => new GameEngine(consoleMethods));

            // Revert
            GameEngine.Emblems.Add(testKingdom, emblem);
        }


        public static IEnumerable<TestCaseData> TestInputForExecuteFunction()
        {
            yield return new TestCaseData(new string[]
            {
                "This in an invalid input",     // Will return 'Invalid input'
                "exit"                          // Will exit loop so the Simulated output should contain 'Invalid input'
            },
            "Invalid input");

            yield return new TestCaseData(new string[]
            {
                "Allies of",                    // Will return 'None'
                "exit"                          // Will exit loop so the Simulated output should contain 'None'
            },
            "None");
        }
        [Test, Category("EngineTests"), TestCaseSource("TestInputForExecuteFunction")]
        [Description("Tests that the Execute function exits when it should and not when it souldn not")]
        public void When_ExitEntered_Expect_ExecutionStop(string[] inputs, string expectedOutput)
        {
            // Arrange
            multiIOConsoleMethods.SimulatedMultipleInputs.AddRange(inputs);
            var testGameEngine = new GameEngine(multiIOConsoleMethods);

            // Act
            testGameEngine.Execute();

            // Assert
            Assert.AreEqual(expectedOutput, multiIOConsoleMethods.SimulatedOutput);
        }


        public static IEnumerable<TestCaseData> TestDataForNoRulerAndQuestion()
        {
            yield return new TestCaseData("This is not a valid input", "Invalid input");
            yield return new TestCaseData("Who is the ruler of Southeros?", "None");
            yield return new TestCaseData("who is the ruler", "None");
            yield return new TestCaseData("ruler of southeros", "None");
        }
        [Test, Category("EngineTests"), TestCaseSource("TestDataForNoRulerAndQuestion")]
        [Description("Tests the ProcessInput function to see if it returns a valid response in case there is no ruler")]
        public void When_NoRulerAndValidInput_Expect_None(string input, string expected)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);

            // Act
            var actual = testGameEngine.ProcessInput(input.Trim().ToLower());

            // Assert
            Assert.AreEqual(expected, actual);
        }


        public static IEnumerable<TestCaseData> TestDataForRulerAndQuestion()
        {
            foreach (Kingdoms kingdom in Enum.GetValues(typeof(Kingdoms))) yield return new TestCaseData("Who is the ruler of Southeros?", kingdom);
        }
        [Test, Category("EngineTests"), TestCaseSource("TestDataForRulerAndQuestion")]
        [Description("Tests the ProcessInput function to see if it returns the name of the ruling kingdom in case a ruler has been crowned")]
        public void When_RulerAndValidInput_Expect_KingdomName(string input, Kingdoms rulingKingdom)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.RulingKingdom = testGameEngine.AllKingdoms[rulingKingdom];

            // Act
            var actual = testGameEngine.ProcessInput(input.Trim().ToLower());

            // Assert
            Assert.AreEqual(rulingKingdom.ToString(), actual);
        }


        [Test, Category("EngineTests"), Category("GameEngineTest")]
        [Description("Tests the ProcessInput function to see if it returns the name of the king of the ruling kingdom if the name is known")]
        public void When_RulerValidInputAndKingName_Expect_KingName()
        {
            // Arrange
            var kingName = "King Shan";
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.RulingKingdom = testGameEngine.AllKingdoms[Kingdoms.Space];
            testGameEngine.RulingKingdom.King = kingName;

            // Act
            var actual = testGameEngine.ProcessInput("who is the ruler");

            // Assert
            Assert.AreEqual(kingName, actual);
        }


        public static IEnumerable<TestCaseData> TestDataSubListOfAllies()
        {
            var totalKingdoms = Enum.GetValues(typeof(Kingdoms)).Length;
            for (int i = 0; i < totalKingdoms; i++) yield return new TestCaseData(new List<Kingdoms> { (Kingdoms)i, (Kingdoms)((i + 1) % totalKingdoms), (Kingdoms)((i + 2) % totalKingdoms), (Kingdoms)((i + 3) % totalKingdoms) });
        }
        [Test, Category("EngineTests"), TestCaseSource("TestDataSubListOfAllies")]
        [Description("Tests the ProcessInput function for a valid 'who are the allies of' input and return a list of comma delimited allies")]
        public void When_ValidAlliesOfInput_Expect_ListOfAllies(List<Kingdoms> testData)
        {
            // Arrange
            var rulingKingdom = testData[0];
            var allies = testData.Skip(1);
            var testGameEngine = new GameEngine(consoleMethods);
            testGameEngine.RulingKingdom = testGameEngine.AllKingdoms[rulingKingdom];
            testGameEngine.RulingKingdom.Allies.AddRange(allies);

            // Act
            var actual = testGameEngine.ProcessInput("who are the allies of");

            // Assert
            Assert.AreEqual(string.Join(", ", allies), actual);
        }


        public static IEnumerable<TestCaseData> TestDataForCustomInputAndAction()
        {
            // Test Data 1: Should only execute when string is not null or whitespace            
            //  (a): Should execute as input string has value
            yield return new TestCaseData("NonEmptyString", "Length is: 14");

            //  (b): Should not execute as input string is empty
            yield return new TestCaseData(string.Empty, "Invalid input");

            //  (c): Should not execute as input string is whitespace
            yield return new TestCaseData("          ", "Invalid input");

            // Test Data 2: Should return the length of the input string
            //  (a): Should return 'Length is: 8' for 'Anything'
            yield return new TestCaseData("Anything", "Length is: 8");

            //  (b): Should return 'Length is: 9' for '123456789'
            yield return new TestCaseData("123456789", "Length is: 9");
        }
        [Test, Category("EngineTests"), TestCaseSource("TestDataForCustomInputAndAction")]
        [Description("Tests the ProcessInput function using a simple CustomInputValidator and a simple CustomInputAction")]
        public void When_ValidCustomInput_Expect_CustomAction(string customInput, string expectedOutput)
        {
            // Arrange
            var testGameEngine = new GameEngine(consoleMethods)
            {
                CustomInputValidator = (string input) => { return !string.IsNullOrWhiteSpace(input); },
                CustomInputAction = (string input, ISoutheros southeros) => { return $"Length is: {input.Length}"; }
            };

            // Act
            var actual = testGameEngine.ProcessInput(customInput);

            // Assert
            Assert.AreEqual(expectedOutput, actual);
        }
    }
}
