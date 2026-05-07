using GraduationProject.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetSupervisingRequestById
{
    public class GetSupervisingRequestByIdQuery:IRequest<Result<SupervisingRequestDto>>
    {
        public int Id { get; set; }
        public GetSupervisingRequestByIdQuery(int id)
        {
            Id = id;
        }
    }
    public class SupervisingRequestDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Status { get; set; }

        public string University { get; set; }

        public string AcademicYear { get; set; }

       public string NationalId { get; set; }

        public string ProofImageUrl { get; set; }
    }
}
