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
                Item = MapearTagADiccionario(tag)
            };

            await _dynamoDbClient.PutItemAsync(request);
            return tag;
        }

        public async Task<IEnumerable<Tag>> ObtenerTagsAsync()
        {
            // Nota: Scan lee toda la tabla. Para tablas enormes, considera usar Query o paginación.
            var request = new ScanRequest { TableName = _tableName };
            var response = await _dynamoDbClient.ScanAsync(request);

            return response.Items.Select(MapearDiccionarioATag);
        }

        // Métodos auxiliares para no repetir código de mapeo
        private Dictionary<string, AttributeValue> MapearTagADiccionario(Tag tag)
        {
            return new Dictionary<string, AttributeValue>
            {
                { "NumeroTag", new AttributeValue { S = tag.NumeroTag } },
                { "PlacaVehiculo", new AttributeValue { S = tag.PlacaVehiculo } },
                { "Saldo", new AttributeValue { N = tag.Saldo.ToString(CultureInfo.InvariantCulture) } },
                { "Estado", new AttributeValue { S = tag.Estado } },
                { "FechaRegistro", new AttributeValue { S = tag.FechaRegistro.ToString("O") } }
            };
        }

        public async Task<Tag?> ObtenerTagPorIdAsync(string numeroTag)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "NumeroTag", new AttributeValue { S = numeroTag } }
                }
            };

            var response = await _dynamoDbClient.GetItemAsync(request);
            if (response.Item == null || response.Item.Count == 0) return null;

            return MapearDiccionarioATag(response.Item);
        }

        public async Task ActualizarTagAsync(Tag tag)
        {
            // PutItem reemplaza el elemento existente completo
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = MapearTagADiccionario(tag)
            };
            await _dynamoDbClient.PutItemAsync(request);
        }

        public async Task EliminarTagAsync(string numeroTag)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "NumeroTag", new AttributeValue { S = numeroTag } }
                }
            };
            await _dynamoDbClient.DeleteItemAsync(request);
        }

        private Tag MapearDiccionarioATag(Dictionary<string, AttributeValue> item)
        {
            return new Tag
            {
                NumeroTag = item["NumeroTag"].S,
                PlacaVehiculo = item["PlacaVehiculo"].S,
                Saldo = decimal.Parse(item["Saldo"].N, CultureInfo.InvariantCulture),
                Estado = item["Estado"].S,
                FechaRegistro = DateTime.Parse(item["FechaRegistro"].S, null, DateTimeStyles.RoundtripKind)
            };
        }
    }
}
