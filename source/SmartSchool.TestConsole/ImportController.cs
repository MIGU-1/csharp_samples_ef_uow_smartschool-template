using System;
using System.Collections.Generic;
using SmartSchool.Core.Entities;
using Utils;
using System.Linq;
using SmartSchool.Persistence;
using SmartSchool.Core.Contracts;

namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static IEnumerable<Measurement> ReadFromCsv()
        {
            string[][] data = MyFile.ReadStringMatrixFromCsv(Filename, true);
            return ReadAllMeasurements(data);
        }
        private static IEnumerable<Measurement> ReadAllMeasurements(string[][] data)
        {
            List<Sensor> sensors = ReadAllSensors(data).ToList();
            List<Measurement> measurements = new List<Measurement>();

            for (int i = 0; i < data.Length; i++)
            {
                DateTime dateTime = DateTime.Parse($"{data[i][0]} {data[i][1]}");
                string[] locAndName = data[i][2].Split('_');
                string location = locAndName[0];
                string name = locAndName[1];
                double value = Convert.ToDouble(data[i][3]);

                Sensor sensor = sensors.Where(s => s.Location == location).Where(s => s.Name == name).SingleOrDefault();

                Measurement newMeasurement = new Measurement
                {
                    Sensor = sensor,
                    Time = dateTime,
                    Value = value
                };

                sensor.Measurements.Add(newMeasurement);
                measurements.Add(newMeasurement);
            }

            return measurements;
        }
        private static IEnumerable<Sensor> ReadAllSensors(string[][] data)
        {
            List<Sensor> sensors = new List<Sensor>();

            for (int i = 0; i < data.Length; i++)
            {
                string[] locAndName = data[i][2].Split('_');
                string location = locAndName[0];
                string name = locAndName[1];

                Sensor newSensor = new Sensor
                {
                    Name = name,
                    Location = location,
                    Measurements = new List<Measurement>(),
                };

                Sensor tmpsensor = sensors
                    .Where(s => s.Location == location)
                    .Where(s => s.Name == name)
                    .SingleOrDefault();

                if (tmpsensor == null)
                {
                    sensors.Add(newSensor);
                }

                tmpsensor = null;
            }

            return sensors;
        }
    }
}
