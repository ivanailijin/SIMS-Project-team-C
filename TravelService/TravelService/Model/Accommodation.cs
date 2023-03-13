﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TravelService.Serializer;
using static System.Net.Mime.MediaTypeNames;

public enum TYPE { APARTMENT, HOUSE, COTTAGE };

namespace TravelService.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        public TYPE Type { get; set; }

        public int MaxGuestNumber { get; set; }

        public int MinReservationDays { get; set; }

        public int DaysBeforeCancellingReservation { get; set; }

        public List<string> Pictures { get; set; }


        public Accommodation()
        {
            List<string> Pictures = new List<string>();
        }

        public Accommodation(int id, string name, Location location, int locationId, TYPE type, int maxGuestNumber, int minReservationDays, int daysBeforeCancellingReservation, List<string> pictures)
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = locationId;
            Type = type;
            MaxGuestNumber = maxGuestNumber;
            MinReservationDays = minReservationDays;
            DaysBeforeCancellingReservation = daysBeforeCancellingReservation;
            Pictures = pictures;
        }

        public string TypeToCSV()
        {
            if (this.Type == TYPE.APARTMENT)
                return "Apartment";
            else if (this.Type == TYPE.HOUSE)
                return "House";
            else
                return "Cottage";
        }

        public TYPE TypeFromCSV(string type)
        {
            if (string.Equals(type, "Apartment"))
                return TYPE.APARTMENT;
            else if (string.Equals(type, "Cottage"))
                return TYPE.COTTAGE;
            else
                return TYPE.HOUSE;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                TypeToCSV(),
                MaxGuestNumber.ToString(),
                MinReservationDays.ToString(),
                DaysBeforeCancellingReservation.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationId = Convert.ToInt32(values[2]);
            Type = TypeFromCSV(values[3]);
            MaxGuestNumber = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            DaysBeforeCancellingReservation = Convert.ToInt32(values[6]);
        }
    }
}