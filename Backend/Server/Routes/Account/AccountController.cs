using Microsoft.AspNetCore.Mvc;

namespace Backend.Server.Routes.Account;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController(AccountService AccountService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        try
        {
            var accounts = await AccountService.GetAll();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(int id)
    {
        try
        {
            var account = await AccountService.GetById(id);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var account = await AccountService.Create(createAccountDto);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountDto updateAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var account = await AccountService.Update(id, updateAccountDto);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        try
        {
            await AccountService.Delete(id);
            return Ok(new { message = "Conta removida com sucesso" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
