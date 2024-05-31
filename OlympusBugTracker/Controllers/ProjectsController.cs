﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OlympusBugTracker.Client.Models;
using OlympusBugTracker.Client.Services.Interfaces;
using OlympusBugTracker.Helpers.Extensions;

namespace OlympusBugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private string _userId => User.GetUserId()!;
        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        private readonly IProjectDTOService _projectService;

        public ProjectsController(IProjectDTOService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<ProjectDTO> projects = await _projectService.GetAllProjectsAsync(_companyId.Value);

                    return Ok(projects);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpGet("archived")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetArchivedProjects()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<ProjectDTO> projects = await _projectService.GetArchivedProjectsAsync(_companyId.Value);

                    return Ok(projects);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpGet("project/{projectId:int}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is not null)
                {
                    ProjectDTO? project = await _projectService.GetProjectByIdAsync(projectId, _companyId.Value);

                    if (project is not null)
                    {
                        return Ok(project);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> AddProject([FromBody] ProjectDTO projectDTO)
        {
            try
            {
                if (_companyId is not null)
                {
                    ProjectDTO projectAdded = await _projectService.AddProjectAsync(projectDTO, _companyId.Value);

                    return Ok(projectAdded);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> UpdateProject([FromRoute] int projectId, [FromBody] ProjectDTO projectDTO)
        {
            try
            {
                if (_companyId is not null)
                {
                    ProjectDTO? projectToUpdate = await _projectService.GetProjectByIdAsync(projectId, _companyId.Value);

                    if (projectToUpdate is not null)
                    {
                        await _projectService.UpdateProjectAsync(projectDTO, _companyId.Value);

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPut("archive/{projectId:int}")]
        public async Task<IActionResult> ArchiveProject([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is not null)
                {
                    ProjectDTO? projectToArchive = await _projectService.GetProjectByIdAsync(projectId, _companyId.Value);

                    if (projectToArchive is not null)
                    {
                        await _projectService.ArchiveProjectAsync(projectId, _companyId.Value);

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPut("restore/{projectId:int}")]
        public async Task<IActionResult> RestoreProject([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is not null)
                {
                    ProjectDTO? projectToRecover = await _projectService.GetProjectByIdAsync(projectId, _companyId.Value);

                    if (projectToRecover is not null)
                    {
                        await _projectService.RestoreProjectAsync(projectId, _companyId.Value);

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
