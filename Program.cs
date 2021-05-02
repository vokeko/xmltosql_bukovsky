using System;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace xmltosql_bukovsky
{

    class Program
    {
        private const string SourceUrl = "http://scsanctions.un.org/resources/xml/en/consolidated.xml";
        private const string ConnectionString = "Data Source=HAL-9000;Initial Catalog=SampleDB;Integrated Security=True";
        private const string DateTimeFormat = "yyyy-MM-dd";

        static void Main(string[] args)
        {
            SynchronizeFromXml(SourceUrl);

        }

        private static void SynchronizeFromXml(string url)
        {
            XmlDocument xmlDocument = StringToXmlDoc(url);

            List<Individual> individuals = GetIndividualsFromXmlDoc(xmlDocument);
            List<Entity> entities = GetEntitiesFromXmlDoc(xmlDocument);

            SqlConnection connection;
            connection = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            connection.Open();

            EmptyAllTables(connection, adapter);
            Flush(connection, adapter, individuals, entities);
        }

        private static XmlDocument StringToXmlDoc(string url)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            return xmlDoc;
        }

        private static List<Individual> GetIndividualsFromXmlDoc(XmlDocument xmlDoc)
        {
            XmlNodeList xmlIndividuals = xmlDoc.GetElementsByTagName("INDIVIDUAL");

            List<Individual> individuals = new List<Individual>();

            List<string> aliases;
            List<DateTime> datesOfBirth;
            List<Place> placesOfBirth;
            List<Document> documents;

            foreach (XmlNode xmlIndividual in xmlIndividuals)
            {
                aliases = new List<string>();
                datesOfBirth = new List<DateTime>();
                placesOfBirth = new List<Place>();
                documents = new List<Document>();

                foreach (XmlElement xmlAlias in xmlIndividual.SelectNodes("INDIVIDUAL_ALIAS"))
                {
                    string alias = xmlAlias.SelectSingleNode("ALIAS_NAME")?.InnerText;
                    if (alias is null || alias.Length == 0)
                    {
                        continue;
                    }

                    aliases.Add(alias);
                }

                foreach (XmlElement xmlDate in xmlIndividual.SelectNodes("INDIVIDUAL_DATE_OF_BIRTH"))
                {
                    DateTime? date = ParseDateTime(xmlDate.SelectSingleNode("DATE")?.InnerText);
                    if (date is null)
                    {
                        continue;
                    }

                    datesOfBirth.Add((DateTime)date);
                }

                foreach (XmlElement xmlDocument in xmlIndividual.SelectNodes("INDIVIDUAL_DOCUMENT"))
                {
                    if (xmlDocument.IsEmpty)
                    {
                        continue;
                    }

                    documents.Add(new Document(
                        xmlDocument.SelectSingleNode("TYPE_OF_DOCUMENT")?.InnerText,
                        xmlDocument.SelectSingleNode("TYPE_OF_DOCUMENT2")?.InnerText,
                        xmlDocument.SelectSingleNode("NUMBER")?.InnerText
                    ));
                }

                foreach (XmlElement xmlPlaceOfBirth in xmlIndividual.SelectNodes("INDIVIDUAL_PLACE_OF_BIRTH"))
                {
                    if (xmlPlaceOfBirth.IsEmpty)
                    {
                        continue;
                    }

                    placesOfBirth.Add(new Place(
                        xmlPlaceOfBirth.SelectSingleNode("CITY")?.InnerText,
                        xmlPlaceOfBirth.SelectSingleNode("COUNTRY")?.InnerText
                    ));
                }

                individuals.Add(new Individual(
                    xmlIndividual.SelectSingleNode("DATAID").InnerText,
                    (DateTime)ParseDateTime(xmlIndividual.SelectSingleNode("LISTED_ON")?.InnerText),
                    xmlIndividual.SelectSingleNode("FIRST_NAME")?.InnerText,
                    xmlIndividual.SelectSingleNode("SECOND_NAME")?.InnerText,
                    xmlIndividual.SelectSingleNode("THIRD_NAME")?.InnerText,
                    aliases,
                    datesOfBirth,
                    placesOfBirth,
                    documents,
                    xmlIndividual.SelectSingleNode("COMMENTS1")?.InnerText
                ));
            }

            return individuals;
        }

        private static List<Entity> GetEntitiesFromXmlDoc(XmlDocument xmlDoc)
        {
            XmlNodeList xmlEntities = xmlDoc.GetElementsByTagName("ENTITY");

            List<Entity> entities = new List<Entity>();

            List<string> aliases;
            List<Address> addresses;

            foreach (XmlNode xmlEntity in xmlEntities)
            {
                aliases = new List<string>();
                addresses = new List<Address>();

                foreach (XmlElement xmlAlias in xmlEntity.SelectNodes("ENTITY_ALIAS"))
                {
                    string alias = xmlAlias.SelectSingleNode("ALIAS_NAME")?.InnerText;
                    if (alias is null || alias.Length == 0)
                    {
                        continue;
                    }
                    aliases.Add(alias);
                }

                foreach (XmlElement xmlPlace in xmlEntity.SelectNodes("ENTITY_ADDRESS"))
                {
                    if (xmlPlace.IsEmpty)
                    {
                        continue;
                    }
                    addresses.Add(new Address(
                        xmlPlace.SelectSingleNode("STREET")?.InnerText,
                        xmlPlace.SelectSingleNode("CITY")?.InnerText,
                        xmlPlace.SelectSingleNode("ZIP_CODE")?.InnerText,
                        xmlPlace.SelectSingleNode("COUNTRY")?.InnerText
                    ));
                }
                entities.Add(new Entity(
                xmlEntity.SelectSingleNode("DATAID").InnerText,
                (DateTime)ParseDateTime(xmlEntity.SelectSingleNode("LISTED_ON")?.InnerText),
                addresses,
                aliases,
                xmlEntity.SelectSingleNode("FIRST_NAME")?.InnerText,
                xmlEntity.SelectSingleNode("COMMENTS1")?.InnerText
                ));
            }
            return entities;
        }

        private static void Flush(SqlConnection connection, SqlDataAdapter adapter, List<Individual> individuals, List<Entity> entities)
        {
            foreach (Individual individual in individuals)
            {
                InsertIntoTable("individual", connection, adapter, new string[] { "id", "firstName", "secondName", "thirdName", "listedOn", "commentary" }, new string[] {
                    individual.Id,
                    individual.FirstName,
                    individual.SecondName,
                    individual.ThirdName,
                    individual.ListedOn.ToString(DateTimeFormat),
                    individual.Commentary
                });

                foreach (DateTime dateOfBirth in individual.DatesOfBirth)
                {
                    InsertIntoTable("dateOfBirth", connection, adapter, new string[] { "individualId", "dateOfBirth" }, new string[] {
                        individual.Id,
                        dateOfBirth.ToString(DateTimeFormat)
                    });
                }

                foreach (Place placeOfBirth in individual.PlacesOfBirth)
                {
                    InsertIntoTable("placeOfBirth", connection, adapter, new string[] { "individualId", "city", "country" }, new string[] {
                        individual.Id,
                        placeOfBirth.City,
                        placeOfBirth.Country
                    });
                }

                foreach (string alias in individual.Aliases)
                {
                    InsertIntoTable("individualAlias", connection, adapter, new string[] { "individualId", "alias" }, new string[] {
                        individual.Id,
                        alias,
                    });
                }

                foreach (Document document in individual.Documents)
                {
                    InsertIntoTable("document", connection, adapter, new string[] { "individualId", "documentType", "documentType2", "documentNumber" }, new string[] {
                        individual.Id,
                        document.Type,
                        document.Type2,
                        document.Number
                    });
                }
            }

            foreach (Entity entity in entities)
            {
                InsertIntoTable("entity", connection, adapter, new string[] { "id", "name", "listedOn", "commentary" }, new string[] {
                    entity.Id,
                    entity.Name,
                    entity.ListedOn.ToString(DateTimeFormat),
                    entity.Commentary,
                });

                foreach (string alias in entity.Aliases)
                {
                    InsertIntoTable("entityAlias", connection, adapter, new string[] { "entityId", "alias" }, new string[]{
                        entity.Id,
                        alias
                    });
                }

                foreach (Address address in entity.Addresses)
                {
                    InsertIntoTable("entityAddress", connection, adapter, new string[] { "entityId", "street", "city", "zipCode", "country" }, new string[]{
                        entity.Id,
                        address.Street,
                        address.City,
                        address.ZipCode,
                        address.Country
                    });
                }

            }

            connection.Close();
        }

        private static void InsertIntoTable(string tableName, SqlConnection connection, SqlDataAdapter adapter, string[] parameterNames, string[] values)
        {
            string[] sanitizedValues = values;
            int i = 0;
            foreach (string value in values)
            {
                sanitizedValues[i++] = value?.Replace('\'', '`');
            }

            string columnsJoined = String.Join(",", parameterNames);
            string valuesJoined = String.Join("','", sanitizedValues);
            string query = $"INSERT INTO {tableName} ({columnsJoined}) VALUES ('{valuesJoined}')";

            SqlCommand command = new SqlCommand(query, connection);
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
            command.Dispose();
        }

        private static void EmptyAllTables(SqlConnection connection, SqlDataAdapter adapter)
        {
            string[] queries = {
                "TRUNCATE TABLE dateOfBirth",
                "TRUNCATE TABLE document",
                "TRUNCATE TABLE entityAddress",
                "TRUNCATE TABLE entityAlias",
                "TRUNCATE TABLE individualAlias",
                "TRUNCATE TABLE placeOfBirth",
                "DELETE FROM entity",
                "DELETE FROM individual"
            };

            foreach (string query in queries)
            {
                SqlCommand command;
                command = new SqlCommand(query, connection);
                adapter.InsertCommand = new SqlCommand(query, connection);
                adapter.InsertCommand.ExecuteNonQuery();
                command.Dispose();
            }
        }

        private static DateTime? ParseDateTime(string input)
        {
            if (input is null)
            {
                return null;
            }

            if (input.Length == 4)
            {
                input += "-01-01";
            }

            try
            {
                return DateTime.Parse(input);
            }
            catch (FormatException)
            {
                return null;
            }
        }

    }

}