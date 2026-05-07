using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.BackgroundJobs.Patients
{
    public interface IPatientStatusService
    {
        Task UpdateInactivePatients(CancellationToken cancellationToken);
    }
}
