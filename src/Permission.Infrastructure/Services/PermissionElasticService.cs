using Azure.Core;
using Azure;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MediatR;
using Permissions.Infrastructure.Services.Blocks;

namespace Permissions.Infrastructure.Services
{
    public class PermissionElasticService: IPermissionElasticService
    {
        private readonly IElasticClient _elasticClient;
        public PermissionElasticService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public async Task<bool> CreatePermission(PermissionBlock request)
        {
            VerifyCreateIndex<PermissionBlock>();
            var response = _elasticClient.Index<PermissionBlock>(new IndexRequest<PermissionBlock>(request));

            if (!response.IsValid)
            {
                //($"No se puso actualizar en ES el permiso con ID : {request.PermisoId}");
            }
            return true;
        }
        public async Task<bool> UpdatePermission(PermissionBlock request)
        {
            VerifyCreateIndex<PermissionBlock>();
            var response = await _elasticClient.UpdateAsync<PermissionBlock>(request.Id, d => d.Doc(request).Refresh(Refresh.True));
            if (!response.IsValid)
            {
                //($"No se puso actualizar en ES el permiso con ID : {request.PermisoId}");
            }
            return true;
        }

        private bool VerifyCreateIndex<T>() where T : class
        {
            var existsResponse = _elasticClient.Indices.Exists(Indices.Index<T>());
            if (!existsResponse.Exists)
            {
                var createResponse = _elasticClient.Indices.Create(Indices.Index<T>(), c => c
                                     .Settings(se => se
                                        .NumberOfReplicas(0)
                                     )
                                     .Map<T>(m => m
                                        .AutoMap())
                                     );
                return true;
            }
            return true;
        }
    }
}
