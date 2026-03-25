using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using WorkoutTracker.Api.DTOs;
using WorkoutTracker.Api.Models;
using WorkoutTracker.Api.Repositories;
using WorkoutTracker.Api.Services;

namespace WorkoutTracker.Tests
{
    public class WorkoutSetServiceTests
    {
        private readonly Mock<IWorkoutSetRepository> _setRepo = new();
        private readonly Mock<IWorkoutSessionRepository> _sessionRepo = new();
        private readonly Mock<IExerciseRepository> _exerciseRepo = new();

        private WorkoutSetService CreateService() =>
            new WorkoutSetService(_setRepo.Object, _sessionRepo.Object, _exerciseRepo.Object);

        // 1. Adding a set to a valid session and exercise returns the correct DTO
        [Fact]
        public async Task AddSetToSession_ValidInputs_ReturnsWorkoutSetDto()
        {
            var session = new WorkoutSession { Id = 1 };
            var exercise = new Exercise { Id = 2, Name = "Bench Press" };
            var created = new WorkoutSet { Id = 10, ExerciseId = 2, WorkoutSessionId = 1, Weight = 100, Reps = 8, SetNumber = 1 };

            _sessionRepo.Setup(r => r.GetByIdWithSetsAsync(1)).ReturnsAsync(session);
            _exerciseRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(exercise);
            _setRepo.Setup(r => r.AddAsync(It.IsAny<WorkoutSet>())).ReturnsAsync(created);

            var service = CreateService();
            var result = await service.AddSetToSessionAsync(1, new CreateWorkoutSetDto { ExerciseId = 2, Weight = 100, Reps = 8, SetNumber = 1 });

            Assert.Equal(10, result.Id);
            Assert.Equal("Bench Press", result.ExerciseName);
            Assert.Equal(100, result.Weight);
        }

        // 2. Adding a set to a non-existent session throws KeyNotFoundException
        [Fact]
        public async Task AddSetToSession_SessionNotFound_ThrowsKeyNotFoundException()
        {
            _sessionRepo.Setup(r => r.GetByIdWithSetsAsync(99)).ReturnsAsync((WorkoutSession)null);

            var service = CreateService();

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.AddSetToSessionAsync(99, new CreateWorkoutSetDto { ExerciseId = 1, Weight = 50, Reps = 5, SetNumber = 1 }));
        }

        // 3. Adding a set with a non-existent exercise throws KeyNotFoundException
        [Fact]
        public async Task AddSetToSession_ExerciseNotFound_ThrowsKeyNotFoundException()
        {
            var session = new WorkoutSession { Id = 1 };
            _sessionRepo.Setup(r => r.GetByIdWithSetsAsync(1)).ReturnsAsync(session);
            _exerciseRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Exercise)null);

            var service = CreateService();

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.AddSetToSessionAsync(1, new CreateWorkoutSetDto { ExerciseId = 99, Weight = 50, Reps = 5, SetNumber = 1 }));
        }

        [Fact]
        public async Task DeleteSetAsync_SetNotFound_ThrowsKeyNotFoundException()
        {
            _setRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((WorkoutSet)null);
            var service = CreateService();

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                service.DeleteSetAsync(99));
        }
    }

    public class ExerciseInfoProviderTests
    {
        // 4. Known muscle group returns correct image path
        [Fact]
        public void GetMuscleGroupInfo_KnownGroup_ReturnsInfo()
        {
            var info = WorkoutTracker.Api.Infrastructure.ExerciseInfoProvider.GetMuscleGroupInfo("Chest");

            Assert.NotNull(info);
            Assert.Contains("Pectoralis", string.Join(",", info.MusclesInvolved));
        }

        // 5. Unknown muscle group returns null (no crash)
        [Fact]
        public void GetMuscleGroupInfo_UnknownGroup_ReturnsNull()
        {
            var info = WorkoutTracker.Api.Infrastructure.ExerciseInfoProvider.GetMuscleGroupInfo("InvalidGroup");

            Assert.Null(info);
        }
    }
}
