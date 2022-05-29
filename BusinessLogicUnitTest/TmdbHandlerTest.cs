using BuissnessLogic.Handlers;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace BusinessLogicUnitTest
{
    public class TmdbHandlerTest
    {
        [Theory]
        [InlineData("testEnding")]
        [InlineData("æøå")]
        public void TransformPopularActorsList_ValidList_ReturnsValidList(string s)
        {
            //Arrange
            var arrangedList = new List<FullPerson>()
            {
                new FullPerson {
                   profile_path = s
                }
            };

            var mockedHttpClient = Substitute.For<HttpClient>();
            var uut = new TmdbHandler(mockedHttpClient);

            //Act
            var result = uut.TransformPopularActorsList(arrangedList);

            //assert
            Assert.Equal($"https://image.tmdb.org/t/p/w200{s}", result[0].profile_path);
        }

        [Fact]
        public void TransformPopularActorsList_ValidList_ReturnsValidListFact()
        {
            //Arrange
            var arrangedList = new List<FullPerson>()
            {
                new FullPerson {
                   profile_path = "test"
                }
            };

            var mockedHttpClient = Substitute.For<HttpClient>();
            var uut = new TmdbHandler(mockedHttpClient);

            //Act
            var result = uut.TransformPopularActorsList(arrangedList);

            //assert
            Assert.Equal($"https://image.tmdb.org/t/p/w200test", result[0].profile_path);
        }

    }
}
