using System;
using System.Collections.Generic;

namespace xmltosql_bukovsky
{
    struct Individual
    {
        public Individual(
            string dataId,
            DateTime registrationDate,
            string name,
            string surname,
            string surname2,
            List<string> aliases,
            List<DateTime> datesOfBirth,
            List<Place> placesOfBirth,
            List<Document> documents,
            string comment
        )
        {
            Id = dataId;
            ListedOn = registrationDate;
            FirstName = name;
            SecondName = surname;
            ThirdName = surname2;
            Aliases = aliases;
            DatesOfBirth = datesOfBirth;
            PlacesOfBirth = placesOfBirth;
            Documents = documents;
            Commentary = comment;
        }

        public string Id { get; }
        public DateTime ListedOn { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string ThirdName { get; }
        public List<string> Aliases { get; }
        public List<DateTime> DatesOfBirth { get; }
        public List<Place> PlacesOfBirth { get; }
        public List<Document> Documents { get; }
        public string Commentary { get; }
    }

    struct Place
    {
        public Place(string city, string country)
        {
            City = city;
            Country = country;
        }

        public string City { get; }
        public string Country { get; }
    }

    struct Document
    {
        public Document(string type, string type2, string number)
        {
            Type = type;
            Type2 = type2;
            Number = number;
        }

        public string Type { get; }
        public string Type2 { get; }
        public string Number { get; }
    }

    struct Entity
    {
        public Entity(
            string dataId,
            DateTime registrationDate,
            List<Address> addresses,
            List<string> aliases,
            string name,
            string comment
        )
        {
            Id = dataId;
            ListedOn = registrationDate;
            Addresses = addresses;
            Aliases = aliases;
            Name = name;
            Commentary = comment;
        }

        public string Id { get; }
        public DateTime ListedOn { get; }
        public List<Address> Addresses { get; }
        public List<string> Aliases { get; }
        public string Name { get; }
        public string Commentary { get; }
    }
    struct Address
    {
        public Address(string street, string city, string zipCode, string country)
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
            Country = country;
        }

        public string Street { get; }
        public string City { get; }
        public string Country { get; }
        public string ZipCode { get; }
    }
}
