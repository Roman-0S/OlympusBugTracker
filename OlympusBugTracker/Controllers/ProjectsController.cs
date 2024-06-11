using Microsoft.AspNetCore.Authorization;
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

        #region Project

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


        [HttpGet("member")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetMemberProjects()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<ProjectDTO> projects = await _projectService.GetMemberProjectsAsync(_userId, _companyId.Value);

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
        [Authorize(Roles = "Admin, ProjectManager")]
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
            if (projectId != projectDTO.Id) return BadRequest();

            try
            {
                if (_companyId is not null)
                {
                    await _projectService.UpdateProjectAsync(projectDTO, _companyId.Value);

                    return Ok();
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

        #endregion

        #region Project Manager

        [HttpGet("{projectId:int}/members")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetProjectMembers([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                ProjectDTO? project = await _projectService.GetProjectByIdAsync(projectId, _companyId.Value);

                if (project is not null)
                {
                    IEnumerable<UserDTO> members = await _projectService.GetProjectMembersAsync(projectId, _companyId.Value);

                    return Ok(members);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpGet("{projectId:int}/manager")]
        public async Task<ActionResult<UserDTO>> GetProjectManager([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                UserDTO? manager = await _projectService.GetProjectManagerAsync(projectId, _companyId.Value);

                if (manager is not null)
                {
                    return Ok(manager);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        [HttpPut("{projectId:int}/Add/Member")]
        public async Task<IActionResult> AddProjectMember([FromRoute] int projectId, [FromBody] string memberId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                bool isAdmin = User.IsInRole("Admin");
                bool isPM = User.IsInRole("ProjectManager");

                if (isAdmin || isPM)
                {
                    await _projectService.AddMemberToProjectAsync(projectId, memberId, _userId);

                    return Ok();
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


        [HttpPut("{projectId:int}/Remove/Member")]
        public async Task<IActionResult> RemoveProjectMember([FromRoute] int projectId, [FromBody] string memberId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                bool isAdmin = User.IsInRole("Admin");
                bool isPM = User.IsInRole("ProjectManager");

                if (isAdmin || isPM)
                {
                    await _projectService.RemoveMemberFromProjectAsync(projectId, memberId, _userId);

                    return Ok();
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


        [HttpPut("{projectId:int}/Assign/Manager")]
        public async Task<IActionResult> AssignProjectManager([FromRoute] int projectId, [FromBody] string managerId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                bool isAdmin = User.IsInRole("Admin");

                if (isAdmin)
                {
                    await _projectService.AssignProjectManagerAsync(projectId, managerId, _userId);

                    return Ok();
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


        [HttpPut("{projectId:int}/Remove/Manager")]
        public async Task<IActionResult> RemoveProjectManager([FromRoute] int projectId)
        {
            try
            {
                if (_companyId is null) return BadRequest();

                bool isAdmin = User.IsInRole("Admin");

                if (isAdmin)
                {
                    await _projectService.RemoveProjectManagerAsync(projectId, _userId);

                    return Ok();
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

        #endregion

    }
}
