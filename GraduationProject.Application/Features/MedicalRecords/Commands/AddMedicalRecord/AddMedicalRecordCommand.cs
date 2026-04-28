using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecord
{
    public class AddMedicalRecordCommand : IRequest<Result<int>>
    {
        public int AppointmentId { get; set; }
        public string Title { get; set; }


        public string? Diagnosis { get; set; }

        public string? Notes { get; set; }

       
    }
}
