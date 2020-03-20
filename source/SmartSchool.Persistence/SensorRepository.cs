using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SensorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddRange(List<Sensor> sensors) => _dbContext
            .AddRange(sensors);
        public IEnumerable<Sensor> GetAllSensors() => _dbContext.Sensors
            .Include(s => s.Measurements);
        public Sensor GetSensorByLocationAndName(string location, string name) => _dbContext
            .Sensors
            .Where(s => s.Location == location)
            .Where(s => s.Name == name)
            .Single();

    }
}