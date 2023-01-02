using Application.Drawings.Dto;
using Common;
using Common.Exceptions;
using Domain.Entities.Benaa;
using Domain.Enums;
using FluentValidation;
using Infrastructure.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Drawings.Commands
{

    public class AddConditionResult
    {
        public double ConditionId { get; set; }
        public ConditionStatus Status { get; set; }
    }

    public class CreateDrawingLogCommand : IRequest<Result>
    {
        public string DrwaingId { get; set; }
        public int SuccessNo { get; set; }
        public int FailNo { get; set; }
        public int OtherNo { get; set; }
        public ConditionStatus Result { get; set; }
        public List<AddConditionResult> ConditionResults { get; set; }
    }

    public class CreateDrawingLogValidator : AbstractValidator<CreateDrawingLogCommand>
    {
        public CreateDrawingLogValidator()
        {
            RuleFor(r => r.DrwaingId).NotEmpty().NotNull()
                  .WithMessage("DrwaingId is Required");

            RuleFor(r => r.SuccessNo).NotNull()
                  .WithMessage("SuccessNo is Required");

            RuleFor(r => r.FailNo).NotNull()
                  .WithMessage("FailNo is Required");

            RuleFor(r => r.OtherNo).NotNull()
                  .WithMessage("OtherNo is Required");

            RuleFor(r => r.Result).NotNull()
                  .WithMessage("Result is Required");
        }
    }

    public class CreateDrawingLogHandler : IRequestHandler<CreateDrawingLogCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IXmlService xmlService;

        public CreateDrawingLogHandler(IAppDbContext context, IXmlService xmlService)
        {
            _context = context;
            this.xmlService = xmlService;
        }

        public async Task<Result> Handle(CreateDrawingLogCommand request, CancellationToken cancellationToken)
        {
            var drawing = await _context.tblDrawings.FirstOrDefaultAsync(e => e.Id == request.DrwaingId);
            if (drawing == null) return Result.Failure(ApiExceptionType.BadRequest);

            var conditions = await _context.tblConditionsMap
                    .Include(e => e.Condition)
                    .Where(e => e.AlBaladiaID == drawing.BaladiaId && e.BuildingTypeID == drawing.BuildingTypeId)
                    .ToListAsync();

            var drawingLog = request.Adapt<DrawingLog>();

            var logResults = request.ConditionResults
                                .Select(s =>
                                {
                                    var conditionMap = conditions.FirstOrDefault(e => e.ConditionID == s.ConditionId);

                                    var parameters = xmlService.GetNodes(conditionMap.ParametersValues);
                                    var description = conditionMap.Condition.Description;
                                    foreach (var item in parameters)
                                    {
                                        description = description.Replace(item.Name, item.Value);
                                    }
                                    return new ConditionResult
                                    {
                                        ConditionId = conditionMap.ConditionID,
                                        CurrentCondition = description,
                                        Status = s.Status
                                    };
                                  }).ToHashSet();
            //var logResults = conditions
            //                    .Select(s =>
            //                    {
            //                        var parameters = xmlService.GetNodes(s.ParametersValues);
            //                        var description = s.Condition.Description;
            //                        foreach (var item in parameters)
            //                        {
            //                            description = description.Replace(item.Name, item.Value);
            //                        }
            //                        return new ConditionResult
            //                        {
            //                            ConditionId = s.ConditionID,
            //                            CurrentCondition = description,
            //                            Status = request.ConditionResults.FirstOrDefault(e => e.ConditionId == s.ConditionID).Status
            //                        };
            //                    }).ToHashSet();

            drawingLog.Results = logResults;

            await _context.CreateAsync(drawingLog, cancellationToken);

             _context.tblDrawings.Update(drawing);

            return Result.Successed(drawingLog.Adapt<DrwaingPluginDto>());
        }
    }

}
