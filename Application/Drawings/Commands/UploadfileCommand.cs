using Application.Drawings.Dto;
using Common;
using Domain.Entities.Benaa;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Drawings.Commands
{
    public class UploadfileCommand : IRequest<Result>
    {

        public IFormFile FileStrs { get; set; }



    }


    public class UploadfileCommandHandler : IRequestHandler<UploadfileCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IAuditService auditService;
        private readonly IHostingEnvironment env;

        public UploadfileCommandHandler(IAppDbContext context, IAuditService auditService, IHostingEnvironment env)
        {
            _context = context;
            this.auditService = auditService;
            this.env = env;
        }

        public async Task<Result> Handle(UploadfileCommand request, CancellationToken cancellationToken)
        {

            string path = Path.Combine(env.WebRootPath, "Files");
            CreateFolders(path);
            var imageUrl = Path.Combine(path, request.FileStrs.FileName);

            await request.FileStrs.CopyToAsync(new FileStream(imageUrl, FileMode.Create));

            return Result.Successed();
        }
        private void CreateFolders(string foldersPath)
        {
            var exists = Directory.Exists(foldersPath);
            if (!exists)
                Directory.CreateDirectory(foldersPath);
        }
    }
}
