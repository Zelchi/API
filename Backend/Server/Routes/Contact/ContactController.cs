using Microsoft.AspNetCore.Mvc;

namespace Backend.Server.Routes.Contact;

[ApiController]
[Route("api/v1/[controller]")]
public class ContactController(ContactService contactService) : ControllerBase
{
    private readonly ContactService ContactService = contactService;

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
        var deleted = await ContactService.Delete(id);

        if (!deleted)
            return NotFound(new { message = "Contato não encontrado" });

        return Ok(new { message = "Contato removido com sucesso" });
    }
}