﻿namespace RadioSharp.App.Models
{
    internal class RadioStation
    {
        public string Name { get; set; }
        public string[] Streams { get; set; }

        public RadioStation(string name, string stream)
        {
            Name = name;
            Streams = [stream];
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            RadioStation other = (RadioStation)obj;
            return Name == other.Name;
        }
    }
}
