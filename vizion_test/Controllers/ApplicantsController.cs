using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using vizion_test.Model;
using vizion_test.Services;

namespace vizion_test.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicantsController : ControllerBase
{
    private readonly ApplicantsService _applicantsService;
    private readonly AzureStorageService _azureStorageService;

    public ApplicantsController(ApplicantsService applicantsService,
        AzureStorageService azureStorageService)
    {
        _applicantsService = applicantsService;
        _azureStorageService = azureStorageService;
    }

    [HttpGet]
    public async Task<List<Applicants>> Get() =>
        await _applicantsService.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Applicants>> Get(string id)
    {
        var applicant = await _applicantsService.GetAsync(id);

        if (applicant is null)
        {
            return NotFound();
        }

        return applicant;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Applicants newApplicant)
    {
        if (string.IsNullOrEmpty(newApplicant.Id))
        {
            newApplicant.Id = Convert.ToString(ObjectId.GenerateNewId());
        }
        await _applicantsService.CreateAsync(newApplicant);

        if (!string.IsNullOrEmpty(newApplicant.ResumeUrl))
            await _azureStorageService.UploadFileToBlobStorage(newApplicant.Id, newApplicant.ResumeUrl);

        return CreatedAtAction(nameof(Get), new { id = newApplicant.Id }, newApplicant);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Applicants updatedApplicant)
    {
        var applicant = await _applicantsService.GetAsync(id);

        if (applicant is null)
        {
            return NotFound();
        }

        updatedApplicant.Id = applicant.Id;

        if (!string.IsNullOrEmpty(updatedApplicant.ResumeUrl))
            await _azureStorageService.UploadFileToBlobStorage(updatedApplicant.Id, updatedApplicant.ResumeUrl);

        await _applicantsService.UpdateAsync(id, updatedApplicant);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var applicant = await _applicantsService.GetAsync(id);

        if (applicant is null)
        {
            return NotFound();
        }

        await _applicantsService.RemoveAsync(applicant.Id ?? "");

        return NoContent();
    }
}
