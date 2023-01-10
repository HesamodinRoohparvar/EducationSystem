using EducationSystem.Application.Common.Excensions;
using EducationSystem.Application.Common.Interfaces;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Resources;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Application.Admins.Terms.Command
{
    #region command 

    public record CreateTermCommand(
        string Title,
        string Description):IRequest<CreateTermCommandResponce>;

    #endregion

    #region responce

    public record CreateTermCommandResponce(int Id);

    #endregion

    #region validator  

    public class CreateTermCommandValidator : AbstractValidator<CreateTermCommand>
    {
        public CreateTermCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(25)
                .WithName(Resource.Title);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithName(Resource.Description);
        }
    }

    #endregion

    #region handler

    public class CreateTermCommandHandler : IRequestHandler<CreateTermCommand, CreateTermCommandResponce>
    {
        private readonly IAppDbContext _dbContext;

        public CreateTermCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateTermCommandResponce> Handle(CreateTermCommand request, CancellationToken cancellationToken)
        {
            var isTitleDuplicated = await _dbContext.Terms
                .AnyAsync(x=>x.Title == request.Title);

            if (isTitleDuplicated)
            {
                throw new DuplicateException(Resource.DuplicateTitle);
            }

            var entiry = new Term
            {
                Title = request.Title,
                Description = request.Title
            };

            _dbContext.Terms.Add(entiry);

            await _dbContext.SaveChangesAsync();

            return new CreateTermCommandResponce(entiry.Id);
        }
    }

    #endregion
}
