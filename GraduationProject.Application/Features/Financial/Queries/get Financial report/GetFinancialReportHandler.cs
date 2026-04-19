using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Financial.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Financial.Queries.get_Financial_report
{
	public class GetFinancialReportHandler : IRequestHandler<GetFinancialReportQuery, Result<FinancialReportResponse>>
	{
		private readonly IFinancialRepository _repo;
		public GetFinancialReportHandler(IFinancialRepository repo) => _repo = repo;

		public async Task<Result<FinancialReportResponse>> Handle(GetFinancialReportQuery request, CancellationToken cancellationToken)
		{
			try
			{
				// 1. جلب البيانات من الريبوزيتوري
				var data = await _repo.GetDoctorFinancialReportAsync(request.DoctorId, request.StartDate, request.EndDate);

				// 2. التحقق من وجود بيانات
				if (data == null)
				{
					// نستخدم ميثود NotFound الجديدة. 
					// لاحظ أن الكلاس المعدل يأخذ 'error' كأول باراميتر في ميثود NotFound
					return Result<FinancialReportResponse>.NotFound("No financial data found for the selected range.");
				}

				// 3. في حالة النجاح
				// نستخدم ميثود Success التي تأخذ الكائن 'data' فقط وتعتبر الـ StatusCode هو 200 تلقائياً
				return Result<FinancialReportResponse>.Success(data);
			}
			catch (Exception)
			{
				// 4. في حالة حدوث خطأ غير متوقع
				// نستخدم ميثود Failure (القديمة المحدثة) التي تأخذ Status و Error 
				// أو نستخدم الميثود الجديدة InternalError التي اقترحناها في التعديل السابق
				return Result<FinancialReportResponse>.Failure(ResultStatus.Failure, "An unexpected error occurred while generating the financial report.");
			}
		}
	}
}
