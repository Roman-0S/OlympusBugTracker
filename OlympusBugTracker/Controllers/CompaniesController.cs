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
    public class CompaniesController : ControllerBase
    {
        private string _userId => User.GetUserId()!;

        private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

        private readonly ICompanyDTOService _companyService;

        public CompaniesController(ICompanyDTOService companyService)
        {
            _companyService = companyService;
        }


        [HttpGet]
        public async Task<ActionResult<CompanyDTO>> GetCompany()
        {
            try
            {
                if (_companyId is not null)
                {
                    CompanyDTO? company = await _companyService.GetCompanyByIdAsync(_companyId.Value);

                    if (company is not null)
                    {
                        return Ok(company);
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


        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetCompanyMembers()
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<UserDTO> users = await _companyService.GetCompanyMembersAsync(_companyId.Value);

                    return Ok(users);
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


        [HttpGet("members/member/{userId}")]
        public async Task<ActionResult<string>> GetUserRole([FromRoute] string userId)
        {
            try
            {
                if (_companyId is not null)
                {
                    string role = await _companyService.GetUserRoleAsync(userId, _companyId.Value);

                    return Ok(role);
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


        [HttpGet("members/{role}")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersInRole([FromRoute] string role)
        {
            try
            {
                if (_companyId is not null)
                {
                    IEnumerable<UserDTO> users = await _companyService.GetUsersInRoleAsync(role, _companyId.Value);

                    return Ok(users);
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


        [HttpPut]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO companyDTO)
        {
            if (!User.IsInRole("Admin")) return BadRequest();

            try
            {
                if (_companyId is not null)
                {
                    if (companyDTO.Id == _companyId.Value)
                    {
                        CompanyDTO? company = await _companyService.GetCompanyByIdAsync(_companyId.Value);

                        if (company is not null)
                        {
                            await _companyService.UpdateCompanyAsync(companyDTO, _userId);

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


        [HttpPut("members/member/role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Role)) return BadRequest();

            if (!User.IsInRole("Admin")) return BadRequest();

            try
            {
                if (_companyId is not null)
                {
                    await _companyService.UpdateUserRoleAsync(userDTO, _userId);

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

    }
}
