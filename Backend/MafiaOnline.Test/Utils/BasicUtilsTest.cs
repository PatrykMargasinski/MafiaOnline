using AutoFixture.NUnit3;
using MafiaOnline.BusinessLogic.Utils;
using MafiaOnline.DataAccess.Database;
using MafiaOnline.DataAccess.Entities;
using MafiaOnline.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.Test.Repositories
{
    public class BasicUtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [AutoData]
        public void IsAlphabets_Should_Return_True_If_Text_Contains_Only_Alphabets(BasicUtils sut)
        {
            string text = "abcdefgh";
            Assert.IsTrue(sut.IsAlphabets(text));
        }

        [Test]
        [AutoData]
        public void IsAlphabets_Should_Return_False_If_Text_Doesnt_Contains_Only_Alphabets(BasicUtils sut)
        {
            string text = "abc123";
            Assert.IsFalse(sut.IsAlphabets(text));
        }

        [Test]
        [AutoData]
        public void UppercaseFirst_Should_Make_First_Letter_Of_String_Uppercase(BasicUtils sut)
        {
            string text = "text";
            Assert.AreEqual("Text", sut.UppercaseFirst(text));
        }
    }
}
