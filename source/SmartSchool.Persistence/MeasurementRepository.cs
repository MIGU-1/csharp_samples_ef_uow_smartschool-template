using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;
        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }
        public IEnumerable<Measurement> GetAllMeasurementsByLocationAndName(string location, string name) => _dbContext.Measurements
            .Include(s => s.Sensor)
            .Where(s => s.Sensor.Location == location)
            .Where(s => s.Sensor.Name == name);
        public IEnumerable<Measurement> GetValidCo2MeasurementsInOffice() => _dbContext.Measurements
            .Include(m => m.Sensor)
            .Where(m => m.Sensor.Location == "office")
            .Where(m => m.Sensor.Name == "co2")
            .Where(m => m.Value > 300)
            .Where(m => m.Value < 5000);
    }
}