using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Globalization;
using TagAPI.Models;
using TagAPI.Repositories.Interfaces;
using Tag = TagAPI.Models.Tag;

namespace TagAPI.Repositories.Impl
{
    public class TagRepo : ITagRepo
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _tableName = "TagsTelepeaje";

        public TagRepo(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public async Task<Tag> CrearTagAsync(Tag tag)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    // Atributos recibidos del cliente                  
                    { "PlacaVehiculo", new AttributeValue { S = tag.PlacaVehiculo } },
                    
                    // Atributos internos controlados por el sistema
                    { "NumeroTag", new AttributeValue { S = tag.NumeroTag } },
                    { "Saldo", new AttributeValue { N = tag.Saldo.ToString(CultureInfo.InvariantCulture) } },
                    { "Estado", new AttributeValue { S = tag.Estado } },
                    { "FechaRegistro", new AttributeValue { S = tag.FechaRegistro.ToString("O") } }
                }
            };

            await _dynamoDbClient.PutItemAsync(request);
            return tag;
        }
    }
}
