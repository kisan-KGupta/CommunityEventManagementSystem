using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Tests.MocksRepo
{
    public class MockActivityRepository : IActivityRepository
    {
        private readonly List<Activity> _activities = new();

        public int AddCallCount { get; private set; }
        public int UpdateCallCount { get; private set; }
        public int DeleteCallCount { get; private set; }
        public Activity? LastAddedActivity { get; private set; }

        public void SeedActivities(params Activity[] activities)
        {
            _activities.Clear();
            _activities.AddRange(activities);
        }

        public Task<List<Activity>> GetAllAsync()
        {
            return Task.FromResult(_activities.ToList());
        }

        public Task<Activity?> GetByIdAsync(int id)
        {
            var target = _activities.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(target);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            var exists = _activities.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task AddAsync(Activity activity)
        {
            AddCallCount++;
            activity.Id = _activities.Count > 0 ? _activities.Max(a => a.Id) + 1 : 1;
            LastAddedActivity = activity;
            _activities.Add(activity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Activity activity)
        {
            UpdateCallCount++;
            var index = _activities.FindIndex(a => a.Id == activity.Id);
            if (index >= 0)
            {
                _activities[index] = activity;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            DeleteCallCount++;
            _activities.RemoveAll(a => a.Id == id);
            return Task.CompletedTask;
        }
    }
}
