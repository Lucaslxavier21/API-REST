namespace DevJobs.API.Controllers {
    using DevJobs.API.Models;
    using DevJobs.API.Persistence;
    using DevJobs.API.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using DevJobs.API.Persistence.Repositories;
    using Serilog;

    [Route("api/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase {

        private readonly IJobVacancyRepository _repository;
        public JobVacanciesController(IJobVacancyRepository repository) {
            
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() {

            var JobVacancies = _repository.GetAll();
            return Ok(JobVacancies);
        }


        /// <summary>
        /// Buscar uma vaga de Dev através de um identificador Id.
        /// </summary>
        /// <remarks>
        /// "Id da vaga."
        /// </remarks>
        /// <param name="id">Dados de uma vaga específica.</param>
        /// <returns>Vaga encontrada.</returns>
        /// <response code= "201">Sucesso.</response>
        /// <response code= "400">Dados-Não-Encontrados.</response>
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var JobVacancy = _repository.GetById(id);

            if (JobVacancy == null)
                return NotFound();

            return Ok(JobVacancy);
        }

        /// <summary>
        /// Cadastrar uma vaga de emprego.
        /// </summary>
        /// <remarks>
        ///{
        ///"title": "Dev Jr .NET",
        ///"description": "vaga para sustentação de aplicações .Net core",
        ///"company": "Lucas dev,
        ///"isRemote": true,
        ///"salaryRange": "3000 - 5000"
        ///}
        ///</remarks>
        /// <param name="model">Dados da vaga.</param>
        /// <returns>Objeto recém-criado.</returns>
        /// <response code= "201">Sucesso.</response>
        /// <response code= "400">Dados-inválidos.</response>
    [HttpPost]

        public IActionResult Post(AddJobVacancyInputModel model) {
        
            Log.Information("POST JobVacancy chamado");
            
            var jobVacancy = new JobVacancy(
                model.Title,
                model.Description,
                model.Company,
                model.IsRemote,
                model.SalaryRange
            );

            if(jobVacancy.Title.Length > 30)  
                return BadRequest("Título precisa ter menos de 30 caracteres.");
            

            _repository.Add(jobVacancy);

            return CreatedAtAction(
                "GetById",
                 new { id = jobVacancy.Id },
                 jobVacancy);
        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, UpdateJobVacancyInputModel model) {

            var jobVacancy = _repository.GetById(id);

            if (jobVacancy == null)
                return NotFound();

            jobVacancy.Update(model.Title, model.Description);

            _repository.Update(jobVacancy);

            return NoContent();
        }

    }
}