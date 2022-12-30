﻿using FluentAssertions;
using MongoDB.Bson;
using NSubstitute;
using TaskoMask.BuildingBlocks.Contracts.Events;
using TaskoMask.BuildingBlocks.Contracts.Helpers;
using TaskoMask.BuildingBlocks.Contracts.Resources;
using TaskoMask.BuildingBlocks.Domain.Exceptions;
using TaskoMask.BuildingBlocks.Domain.Resources;
using TaskoMask.Services.Boards.Write.Application.UseCases.Boards.AddBoard;
using TaskoMask.Services.Boards.Write.Domain.Events.Boards;
using TaskoMask.Services.Boards.Write.Domain.ValueObjects.Boards;
using TaskoMask.Services.Boards.Write.UnitTests.Fixtures;
using Xunit;

namespace TaskoMask.Services.Boards.Write.UnitTests.UseCases.Boards
{
    public class AddBoardTests : TestsBaseFixture
    {

        #region Fields

        private AddBoardUseCase _addBoardUseCase;

        #endregion

        #region Ctor

        public AddBoardTests()
        {
        }

        #endregion

        #region Test Methods


        [Fact]
        public async Task Board_Is_Added()
        {
            //Arrange
            var addBoardRequest = new AddBoardRequest(projectId: ObjectId.GenerateNewId().ToString(), "Test_Name", "Test_Description");

            //Act
            var result = await _addBoardUseCase.Handle(addBoardRequest, CancellationToken.None);

            //Assert
            result.Message.Should().Be(ContractsMessages.Create_Success);
            result.EntityId.Should().NotBeNull();
            var addedBoard = Boards.FirstOrDefault(u => u.Id == result.EntityId);
            addedBoard.Should().NotBeNull();
            addedBoard.Name.Value.Should().Be(addBoardRequest.Name);
            await InMemoryBus.Received(1).PublishEvent(Arg.Any<BoardAddedEvent>());
            await MessageBus.Received(1).Publish(Arg.Any<BoardAdded>());
        }


        [InlineData("test", "test")]
        [InlineData("تست", "تست")]
        [Theory]
        public async Task Board_Is_Not_Added_When_Name_And_Description_Are_The_Same(string name, string description)
        {
            //Arrange
            var expectedBoard = Boards.FirstOrDefault();
            var addBoardRequest = new AddBoardRequest(expectedBoard.Id, name, description);
            var expectedMessage = DomainMessages.Equal_Name_And_Description_Error;

            //Act
            Func<Task> act = async () => await _addBoardUseCase.Handle(addBoardRequest, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<DomainException>().Where(e => e.Message.Equals(expectedMessage));
        }



        [InlineData("Th")]
        [InlineData("This is a Test This is a Test This is a Test This is a Test This is a Test This is a Test This is a Test This is a Test")]
        [Theory]
        public async Task Board_Is_Not_Added_When_Name_Lenght_Is_Less_Than_Min_Or_More_Than_Max(string name)
        {
            //Arrange
            var expectedBoard = Boards.FirstOrDefault();
            var addBoardRequest = new AddBoardRequest(expectedBoard.Id, name, "Test_Description");
            var expectedMessage = string.Format(ContractsMetadata.Length_Error, nameof(BoardName), DomainConstValues.Board_Name_Min_Length, DomainConstValues.Board_Name_Max_Length);

            //Act
            Func<Task> act = async () => await _addBoardUseCase.Handle(addBoardRequest, CancellationToken.None);

            //Assert
            await act.Should().ThrowAsync<DomainException>().Where(e => e.Message.Equals(expectedMessage));
        }






        #endregion

        #region Fixture

        protected override void TestClassFixtureSetup()
        {
            _addBoardUseCase = new AddBoardUseCase(BoardAggregateRepository, MessageBus, InMemoryBus,BoardValidatorService);
        }

        #endregion
    }
}
