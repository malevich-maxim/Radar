using ProtoBuf;
using System;

namespace RadarStorage
{
    [ProtoContract]
    public class RadarRecord
    {
        public RadarRecord()
        {
            //Id = Guid.NewGuid();
        }

        //public Guid Id { get; }
        [ProtoMember(1)]
        public string Number { get; set; }
        [ProtoMember(2)]
        public float Speed { get; set; }
        [ProtoMember(3)]
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{Date} {Number} {Speed}";
        }
    }
}