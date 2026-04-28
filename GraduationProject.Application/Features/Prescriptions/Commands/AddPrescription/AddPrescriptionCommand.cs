using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Prescriptions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Prescriptions.Commands.AddPrescription
{
    public class AddPrescriptionCommand:IRequest<Result<int>>
    {
        public int PatientId { get; set; }
        

        public List<CreatePrescriptionItemDto> Items { get; set; } = new();
    }
}
