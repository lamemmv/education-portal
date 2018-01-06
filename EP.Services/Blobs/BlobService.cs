﻿using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Enums;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private readonly IRepository<Blob> _blobs;

        public BlobService(MongoDbContext dbContext)
        {
            _blobs = dbContext.Blobs;
        }

        public async Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size)
        {
            FilterDefinition<Blob> filter = string.IsNullOrWhiteSpace(id) ?
                Builders<Blob>.Filter.Exists(e => e.Parent, false) :
                Builders<Blob>.Filter.Eq(e => e.Parent, id);

            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.Name)
                .Include(e => e.ContentType);

            return await _blobs.GetPagedListAsync(filter, projection: projection, skip: page, take: size);
        }

        public async Task<Blob> GetByIdAsync(string id)
        {
            return await _blobs.GetByIdAsync(id);
        }

        public async Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id)
        {
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.VirtualPath);

            var entity = await _blobs.GetByIdAsync(id, projection);

            return new EmbeddedBlob { Id = entity.Id, VirtualPath = entity.VirtualPath };
        }

        public bool IsFile(Blob entity)
        {
            return entity != null &&
                !string.IsNullOrEmpty(entity.FileExtension) &&
                !string.IsNullOrEmpty(entity.ContentType) &&
                !string.IsNullOrEmpty(entity.VirtualPath);
        }

        public async Task<ApiServerResult> CreateDirectoryAsync(string name, string parent)
        {
            var response = await ValidateDirectory(name, parent);

            if (!response.IsOK())
            {
                return response;
            }

            var parentEntity = (Blob)response.Result;
            var ancestors = parentEntity.Ancestors ?? new List<string>();
            ancestors.Add(parent);

            var entity = new Blob
            {
                Name = name,
                Parent = parent,
                Ancestors = ancestors,
                CreatedOn = DateTime.UtcNow
            };

            await _blobs.CreateAsync(entity);

            return ApiServerResult.Created(entity.Id);
        }

        public async Task<ApiServerResult> CreateFileAsync(string parent, IFormFile[] files)
        {


            //Blob entity;
            //IList<string> ids = new List<string>();

            //foreach (var file in files)
            //{
            //    entity = BuildFileEntity(file);
            //    var activityLog = GetCreatedActivityLog(entity.GetType(), entity);

            //    await Task.WhenAll(
            //        _blobService.CreateAsync(entity),
            //        file.SaveAsAsync(entity.PhysicalPath),
            //        _activityLogService.CreateAsync(SystemKeyword.CreateBlob, activityLog));

            //    ids.Add(entity.Id);
            //}

            //return ApiResponse.Created(string.Join(',', ids));
            return null;
        }

        public async Task<ApiServerResult> UpdateDirectoryAsync(string id, string name, string parent)
        {
            var response = await ValidateDirectory(name, parent);

            if (!response.IsOK())
            {
                return response;
            }

            var update = Builders<Blob>.Update
                .Set(e => e.Name, name)
                .CurrentDate(e => e.UpdatedOn);

            var result = await _blobs.UpdatePartiallyAsync(id, update);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return ApiServerResult.NotFound();
            }

            if (IsFile(entity))
            {
                if (File.Exists(entity.PhysicalPath))
                {
                    File.Delete(entity.PhysicalPath);
                }
            }
            else
            {
                if (await HasChildren(id))
                {
                    return ApiServerResult.ServerError(ApiStatusCode.Blob_HasChildren, $"The {id} has sub directories or files.");
                }

                // System Directory.
                if (string.IsNullOrEmpty(entity.Parent))
                {
                    return ApiServerResult.ServerError(ApiStatusCode.Blob_SystemDirectory, $"The {id} is system directory.");
                }
            }

            var result = await _blobs.DeleteAsync(id);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
        }

        private async Task<ApiServerResult> ValidateDirectory(string name, string parent)
        {
            if (await IsExistence(parent, name))
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"{name} is existed.");
            }

            var parentEntity = await GetByIdAsync(parent);

            if (parentEntity == null)
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, $"The {parent} is invalid.");
            }

            return ApiServerResult.OK(parentEntity);
        }

        private async Task<bool> IsExistence(string parent, string name)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, parent) &
                Builders<Blob>.Filter.Eq(e => e.Name, name.ToLowerInvariant());
            var projection = Builders<Blob>.Projection.Include(e => e.Id);
            var entity = await _blobs.GetSingleAsync(filter, projection);

            return entity != null;
        }

        private async Task<bool> HasChildren(string id)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, id);

            return await _blobs.CountAsync(filter) > 0;
        }

        //private Blob BuildFileEntity(IFormFile file, string parentPhysicalPath)
        //{
        //    string contentType = file.ContentType;
        //    string firstMimeType = GetFirstMimeType(contentType);
        //    string publicBlobPath = GetPublicBlobPath(parentPhysicalPath, firstMimeType);

        //    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
        //    string name = Path.GetFileNameWithoutExtension(fileName);
        //    string extension = Path.GetExtension(fileName);
        //    string newFileName = $"{name}_{RandomUtils.Numberic(randomSize)}{extension}";

        //    return new Blob
        //    {
        //        Name = newFileName,
        //        FileExtension = extension.ToLowerInvariant(),
        //        ContentType = contentType,
        //        VirtualPath = string.IsNullOrWhiteSpace(firstMimeType) ?
        //            $"{_publicBlob}/{newFileName}" :
        //            $"{_publicBlob}/{firstMimeType}/{newFileName}",
        //        PhysicalPath = Path.Combine(publicBlobPath, newFileName),
        //        CreatedOn = DateTime.UtcNow
        //    };
        //}

        //private static string GetFirstMimeType(string contentType)
        //{
        //    if (string.IsNullOrWhiteSpace(contentType))
        //    {
        //        return null;
        //    }

        //    var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

        //    if (types.Length == 0 || string.IsNullOrWhiteSpace(types[0]))
        //    {
        //        return null;
        //    }

        //    return types[0];
        //}

        //private static string GetPublicBlobPath(string parentPhysicalPath, string firstMimeType)
        //{
        //    string publicBlobPath = string.IsNullOrWhiteSpace(firstMimeType) ?
        //        Path.Combine(webRootPath, publicBlob) :
        //        Path.Combine(webRootPath, publicBlob, firstMimeType);

        //    if (!Directory.Exists(publicBlobPath))
        //    {
        //        Directory.CreateDirectory(publicBlobPath);
        //    }

        //    return publicBlobPath;
        //}
    }
}
