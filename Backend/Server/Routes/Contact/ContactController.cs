using Microsoft.AspNetCore.Mvc;

namespace Backend.Server.Routes.Contact;

[ApiController]
[Route("api/v1/[controller]")]
public class ContactController(ContactService ContactService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllContacts()
    {
        var contacts = await ContactService.GetAll();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        var contact = await ContactService.GetById(id);
        return Ok(contact);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContact([FromBody] CreateContactDto createContactDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var contact = await ContactService.Create(createContactDto);
        return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] UpdateContactDto updateContactDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var contact = await ContactService.Update(id, updateContactDto);
        return Ok(contact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        try
        {
            await ContactService.Delete(id);
            return Ok(new { message = "Contato removido com sucesso" });
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}