using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetReceptionistAccessControl
{
    public class GetReceptionistAccessControlQuery : IRequest<Result<ReceptionistAccessControlDto>>
    {
    }
    public class ReceptionistStatisticsDto
    {
        public int TotalReceptionists { get; set; }

        public int ActiveReceptionists { get; set; }

        public int InactiveReceptionists { get; set; }
    }
    public class ReceptionistListItemDto
    {
        public string Id { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }

        public string LastLoginDate { get; set; }
    }
    public class ReceptionistAccessControlDto
    {
        public ReceptionistStatisticsDto Statistics { get; set; }
            = new();

        public List<ReceptionistListItemDto> Receptionists { get; set; }
            = new();
    }
}
