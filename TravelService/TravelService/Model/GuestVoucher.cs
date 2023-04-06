using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelService.Serializer;

namespace TravelService.Model
{
    public class GuestVoucher: ISerializable
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; } 
        public string Code { get; set; }    
        public bool Used { get; set; }  



        public GuestVoucher()
        {

        }
       public GuestVoucher(int id, string name, int value, string code, bool used)
        {
            Id = id;
            Name = name;
            Value = value;
            Code = code;
            Used = used;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Value.ToString(),
                Code,
                Used.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Value = Convert.ToInt32(values[2]);
            Code = values[3];
            Used = Boolean.Parse(values[4]);


        }


    }
}
