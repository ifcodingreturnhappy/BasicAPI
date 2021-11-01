using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer;
using DataLayer.Abstractions;
using DataLayer.LiteDB;
using Tests.DataModels;
using Xunit;

namespace Tests
{
    public class DBCommunicator_TESTS
    {
        string[] connectionString_WORKS = new string[]
        {
            @"D:\Desktop\TesteDB\" + "MYDB.db"
        };
        string[] connectionString_DOESNT_WORK = new string[]
        {
            @"D:\Desktop\TesteDB\",
            @"D:\Desktop\TesteDB\" + "MYDB.sql",
            @"MYDB.db"
        };


        [Fact]
        public void CAN_INIT_DBCommunicator()
        {
            // Check for correct instantiation when valid connection string is passed
            foreach (var conString in connectionString_WORKS)
            {
                // Write
                var exception = Record.Exception(() => new LiteDBWrite(conString));
                Assert.Null(exception);

                // Read
                exception = Record.Exception(() => new LiteDBRead(conString));
                Assert.Null(exception);
                
                // Delete
                exception = Record.Exception(() => new LiteDBDelete(conString));
                Assert.Null(exception);
            }

            // Check for exception when invalid connection string is passed
            foreach (var conString in connectionString_DOESNT_WORK)
            {
                Assert.ThrowsAny<Exception>(() =>
                {
                    // Write
                    var writer = new LiteDBWrite(conString);

                    // Read
                    var reader = new LiteDBRead(conString);

                    // Delete
                    var deleter = new LiteDBDelete(conString);
                });
            }
        }
 

        [Fact]
        public void CanUpsert()
        {
            IDBWriter writer = new LiteDBWrite(connectionString_WORKS.First());

            var sampleSize = 10;
            var sample = GetSampleData(10);

            var result = writer.Write(sample, true);

            Assert.Equal(sampleSize, result);
        }

        [Fact]
        public async void CanUpsertAsync()
        {

        }

        [Fact]
        public void CanRead()
        {
            // Make sure db has data to work with
            MakeDbNotEmpty();

            // Test read
            IDBReader reader = new LiteDBRead(connectionString_WORKS.First());

            var result = reader.ReadAll<SampleDataModel>();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void EmptyWhenReadingNonExistingData()
        {
            IDBReader reader = new LiteDBRead(connectionString_WORKS.First());

            var result = reader.ReadAll<NonExistingSampleDataModel>();

            Assert.Empty(result);
        }

        [Fact]
        public async void CanReadAsync()
        {

        }

        [Fact]
        public async void EmptyWhenReadingNonExistingDataAsync()
        {

        }

        [Fact]
        public async void CanGetCount()
        {

        }

        [Fact]
        public async void CanGetCountAsync()
        {

        }


        [Fact]
        public void CanDeleteAll()
        {
            // Make sure db has data to work with
            MakeDbNotEmpty();

            // Test delete
            IDBDeleter deleter = new LiteDBDelete(connectionString_WORKS.First());

            var result = deleter.DeleteAll<SampleDataModel>();

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CanDeleteAllAsync()
        {

        }

        [Fact]
        public void CanDeleteSingle()
        {
            // Make sure db has data to work with
            MakeDbNotEmpty();

            // Test delete
            IDBDeleter deleter = new LiteDBDelete(connectionString_WORKS.First());

            var idToDelete = 1;
            var result = deleter.Delete(new SampleDataModel { Id = idToDelete });

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CanDeleteSingleAsync()
        {

        }

        private void MakeDbNotEmpty()
        {
            IDBReader reader = new LiteDBRead(connectionString_WORKS.First());
            if (reader.GetCount<SampleDataModel>() == 0)
            {
                IDBWriter writer = new LiteDBWrite(connectionString_WORKS.First());

                var sampleSize = 10;
                var sample = GetSampleData(sampleSize);

                writer.Write(sample, true);
            }
        }

        private List<SampleDataModel> GetSampleData(int sampleSize)
        {
            var output = new List<SampleDataModel>();
            for (int i = 0; i < sampleSize; i++)
            {
                output.Add(new SampleDataModel(i));
            }
            return output;
        }
    }
}
