namespace DevJobs.API.Controllers
{
    using DevJobs.API.Models;
    using DevJobs.API.Persistence;
    using DevJobs.API.Entities;
    using Microsoft.AspNetCore.Mvc;
    using DevJobs.API.Persistence.Repositories;

    [Route("api/job-vacancies/{id}/applications")]
    [ApiController]
    public class JobApplicationsController : ControllerBase
    {

        private readonly IJobVacancyRepository _repository;
        public JobApplicationsController(IJobVacancyRepository repository)
        {

            _repository = repository;
        }


        /// <summary>
        /// Cadastrar nome e email do candidato e o id da vaga desejada.
        /// </summary>
        /// <remarks>
        /// "Nome, Email e Id:"
        /// </remarks>
        /// <param name="id">Cadastro de Dados, nome e email.</param>
        /// <returns>Cadastro feito com sucesso.</returns>
        /// <response code= "201">Sucesso.</response>
        /// <response code= "400">Dados-Não-Cadastrados.</response>
        [HttpPost]

        public IActionResult Post(int id, AddJobApplicationInputModel model)
        {

            var jobVacancy = _repository.GetById(id);


            if (jobVacancy == null)
                return NotFound();

            var application = new JobApplication(
                model.ApplicantName,
                model.ApplicantEmail,
                id
            );

            _repository.AddApplication(application);

            return NoContent();
        }

    }
}
