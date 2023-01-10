using EducationSystem.Application.Common.Constans;
using EducationSystem.Application.Common.Interfaces;
using MediatR;

namespace EducationSystem.Application.Admins.Permissions
{
    #region response

    public record PermissionListGroup
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public List<PermissionGroupItem> Children { get; set; }
    }

    public record PermissionGroupItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    #endregion

    #region query

    public record GetPermissionListQuery : IRequest<List<PermissionListGroup>>;

    #endregion

    #region handler

    public class GetPermissionListQueryHandler : IRequestHandler<GetPermissionListQuery, List<PermissionListGroup>>
    {
        private readonly IControllerDiscoveryService _controllerDiscoveryService;

        public GetPermissionListQueryHandler(IControllerDiscoveryService controllerDiscoveryService)
        {
            _controllerDiscoveryService = controllerDiscoveryService;
        }

        public Task<List<PermissionListGroup>> Handle(GetPermissionListQuery request, CancellationToken cancellationToken)
        {
            var result = _controllerDiscoveryService
                .GetAllSecuredControllers(AreaName.Admin, PolicyNames.DynamicPermission)
                .Select(x => new PermissionListGroup
                {
                    Value = x.Id,
                    Label = x.DisplayName,
                    Children = x.Actions
                        .Select(x => new PermissionGroupItem
                        {
                            Value = x.Id,
                            Label = x.DisplayName
                        }).ToList()
                }).ToList();

            return Task.FromResult(result);
        }
    }

    #endregion
}
