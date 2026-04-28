using GraduationProject.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.MedicalRecords.Commands.AddMedicalRecordAttachment
{

    public class AddMedicalRecordAttachmentCommand : IRequest<Result<int>>
    {
        public int MedicalRecordId { get; set; }

        public IFormFile File { get; set; }
    }
}
