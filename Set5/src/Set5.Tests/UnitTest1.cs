using Engine;
using NUnit.Framework;
using System;
using static Engine.GameEngine;

namespace Set5.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        static readonly object[] TestMessages =
        {
            // Data where the emblem itself is the message
            new object[] { Kingdoms.Space ,Emblems[(int)Kingdoms.Space], true },
            new object[] { Kingdoms.Land ,Emblems[(int)Kingdoms.Land], true },
            new object[] { Kingdoms.Water ,Emblems[(int)Kingdoms.Water], true },
            new object[] { Kingdoms.Ice ,Emblems[(int)Kingdoms.Ice], true },
            new object[] { Kingdoms.Air ,Emblems[(int)Kingdoms.Air], true },
            new object[] { Kingdoms.Fire ,Emblems[(int)Kingdoms.Fire], true },

            // Data with message shorter than emblem
            new object[] { Kingdoms.Land ,Emblems[(int)Kingdoms.Land].Substring(1), false },
        };

        [Test]
        [TestCaseSource("TestMessages")]
        public void SecretMessageValidator(Kingdoms kingdom, string message, bool expected)
        {
            // Error message to show in case of failure
            var errorMessage = $"SecretMessageValidator failed for:{Environment.NewLine}Kingdom: {kingdom}{Environment.NewLine}Message: {message}";

            // Generating output using the function being tested
            var actual = SecretMessageAccepted(kingdom, message);

            // Comparing values and asserting result
            Assert.AreEqual(expected, actual, errorMessage);
        }
    }
}