using DataLayer.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.DataModels
{
    internal class SampleDataModel : DbEntityBase
    {
        public SampleDataModel()
        {
        }

        public SampleDataModel(int i)
        {
            this.Name = "NAME_" + i;
            this.City = "CITY_" + i;
            this.Email = "EMAIL_" + i;
            this.Ids = GenerateRandomIdList(i);
        }

        public string Name { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public List<int> Ids { get; set; }


        private List<int> GenerateRandomIdList(int i)
        {
            Random random = new Random();
            var output = new List<int>();

            for (int j = 0; j < 25; j++)
            {
                output.Add(random.Next(i));
            }

            return output;
        }
    }
}
