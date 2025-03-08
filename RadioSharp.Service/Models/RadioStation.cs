namespace RadioSharp.Service.Models
{
    public class RadioStation
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

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
